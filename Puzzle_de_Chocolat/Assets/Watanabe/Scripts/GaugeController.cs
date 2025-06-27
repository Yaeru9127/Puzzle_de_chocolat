using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI�֘A")]
    [Tooltip("�Q�[�W�o�[�̃X���C�_�[�R���|�[�l���g")]
    [SerializeField] private Slider gaugeSlider;

    [Tooltip("GameOver���Ǘ�����N���X")]
    [SerializeField] private GameOverController gameOverController;

    [Tooltip("�c�@���Ǘ�����N���X")]
    [SerializeField] private Remainingaircraft remainingAircraft;

    [Header("�Q�[�W�ݒ�")]
    [Tooltip("�Q�[�W����������܂ł̎��ԁi�b�j")]
    [SerializeField] private float increaseDuration = 0.5f;

    [Tooltip("�ʏ�̃Q�[�W�����ʁi�w�肪�Ȃ��ꍇ�j")]
    [SerializeField] private int defaultIncreaseAmount = 1;

    // �Q�[�W�̍ő�l�i�Œ�j
    private const int maxValue = 10;

    // �Q�[�W�������N�G�X�g��ێ�����L���[
    private Queue<int> increaseQueue = new Queue<int>();

    // ���݃Q�[�W�����������ǂ����̃t���O
    private bool isIncreasing = false;

    /// <summary>
    /// �f�t�H���g�̑����ʂŃQ�[�W�𑝉�������i�O���Ăяo���p�j
    /// </summary>
    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    /// <summary>
    /// �w�肵���ʂŃQ�[�W�𑝉������A�c�@�����炷
    /// </summary>
    /// <param name="increaseAmount">�Q�[�W�̑�����</param>
    public void OnObjectDestroyed(int increaseAmount)
    {
        // �Q�[�W�������N�G�X�g���L���[�ɒǉ�
        increaseQueue.Enqueue(increaseAmount);

        // �c�@�����炷
        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        // ���������������Ă��Ȃ���΃R���[�`���J�n
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    /// <summary>
    /// �Q�[�W�����L���[����������R���[�`��
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        isIncreasing = true;

        while (increaseQueue.Count > 0)
        {
            int amount = increaseQueue.Dequeue();
            yield return StartCoroutine(IncreaseGaugeSmoothly(amount));
        }

        isIncreasing = false;
    }

    /// <summary>
    /// �Q�[�W�����炩�ɑ���������R���[�`��
    /// </summary>
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        if (gaugeSlider == null)
            yield break;

        float startValue = gaugeSlider.value;
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue);

        float elapsed = 0f;

        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration);
            yield return null;
        }

        gaugeSlider.value = endValue;

        // �Q�[�W���ő�ɂȂ�����GameOver����
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
