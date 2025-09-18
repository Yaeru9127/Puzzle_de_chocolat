using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[�W�̐�����s���N���X�B
/// �I�u�W�F�N�g�̔j��ɂ��Q�[�W���������A
/// ���l�ɒB����ƃQ�[���I�[�o�[�𔭐������鏈�����܂ށB
/// </summary>
public class GaugeController : MonoBehaviour
{
    [Header("UI�֘A")]
    [SerializeField] private Image gaugeFillImage; // �Q�[�W�̐i�s�x��\��UI Image
    [Tooltip("�Q�[�W�c��񐔂𐔎��X�v���C�g�ŕ\������UI Image")]
    [SerializeField] private Image gaugeNumberDisplay; // �c��񐔂�\������UI
    private GameOverController over; // �Q�[���I�[�o�[�����N���X�ւ̎Q��
    private Remainingaircraft remain; // �c�@UI�Ȃǂ̏����N���X�ւ̎Q��

    [Header("�Q�[�W�ݒ�")]
    [SerializeField] private float increaseDuration = 0.5f; // �Q�[�W����������A�j���[�V�����̏��v����
    [SerializeField] private int defaultIncreaseAmount = 1; // �f�t�H���g�̑����ʁi1��̑���������j
    [SerializeField] public int maxValue; // �Q�[�W�̍ő�l

    public int currentValue = 0; // ���݂̃Q�[�W�̒l
    private Queue<int> increaseQueue = new Queue<int>(); // �Q�[�W�����̃L���[�i���������ɑΉ��j
    private bool isIncreasing = false; // �Q�[�W�����ݑ��������ǂ����̃t���O

    public bool gaugeIncreased = false; // �Q�[�W��1�x�ł�����������
    public static int gaugeIncreaseCount = 0; // �Q�[�W�̑��������񔭐�������

    void Start()
    {
        over = GameOverController.over;
        remain = Remainingaircraft.remain;

        gaugeIncreased = false;
        gaugeIncreaseCount = 0; // �X�e�[�W�J�n���Ƀ��Z�b�g
        UpdateGaugeNumberDisplay(); // �����̎c��񐔕\���X�V
    }

    /// <summary>
    /// �I�u�W�F�N�g�j�󎞂ɌĂ΂��i�����ʃf�t�H���g�j
    /// </summary>
    //public void OnObjectDestroyed()
    //{
    //    OnObjectDestroyed(defaultIncreaseAmount);
    //}

    /// <summary>
    /// �I�u�W�F�N�g�j�󎞂ɌĂ΂��i�w��ʂ̑����j
    /// </summary>
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount); // �����v�����L���[�ɒǉ�
        if (!isIncreasing)
        {
            ProcessQueueAsync().Forget(); // �� Coroutine �� UniTask 
        }
    }

    /// <summary>
    /// �L���[�ɓ����Ă��鑝�����������ԂɎ��s�iCoroutine �� UniTask�j
    /// </summary>
    private async UniTaskVoid ProcessQueueAsync()
    {
        isIncreasing = true;

        while (increaseQueue.Count > 0)
        {
            int amount = increaseQueue.Dequeue();
            await IncreaseGaugeSmoothlyAsync(amount); // �� Coroutine �� await
        }

        isIncreasing = false;
    }

    /// <summary>
    /// �Q�[�W���w��ʂ����X���[�Y�ɑ��������鏈���iCoroutine �� UniTask�j
    /// </summary>
    private async UniTask IncreaseGaugeSmoothlyAsync(int amount)
    {
        if (gaugeFillImage == null)
            return;

        gaugeIncreased = true;
        gaugeIncreaseCount++; // �����񐔃J�E���g

        int oldValue = currentValue;
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);

        float startFill = (float)oldValue / maxValue;
        float endFill = (float)currentValue / maxValue;

        float elapsed = 0f;
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;
            gaugeFillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsed / increaseDuration);
            await UniTask.Yield(); // �R���[�`���̑�ցi���̃t���[���܂ő҂j
        }

        gaugeFillImage.fillAmount = endFill;
        UpdateGaugeNumberDisplay(); // ����UI�X�V

        if (remain != null)
        {
            remain.ReduceLife(); // �c�@��1���炷
        }

        // �Q�[�W���ő�l�ɒB������Q�[���I�[�o�[�\��
        if (currentValue >= maxValue && over != null)
        {
            over.ShowGameOver();
        }
    }

    /// <summary>
    /// �c��񐔂̐����X�v���C�g���X�V
    /// </summary>
    private void UpdateGaugeNumberDisplay()
    {
        if (gaugeNumberDisplay == null || remain == null)
            return;

        int remaining = Mathf.Clamp(maxValue - currentValue, 0, 99);
        Sprite sprite = remain.GetNumberSprite(remaining);

        if (sprite != null)
        {
            gaugeNumberDisplay.sprite = sprite;
            gaugeNumberDisplay.gameObject.SetActive(true);
        }
        else
        {
            gaugeNumberDisplay.gameObject.SetActive(false);
        }
    }
}
