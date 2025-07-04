using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI�֘A")]
    [Tooltip("UI�X���C�_�[")]
    [SerializeField] private Slider gaugeSlider; // �Q�[�W��UI�X���C�_�[

    private GameOverController gameOverController; // �Q�[���I�[�o�[���Ǘ�����R���g���[���[

    private Remainingaircraft remainingAircraft; // �c�@�Ǘ��N���X�ւ̎Q��

    [Header("�Q�[�W�ݒ�")]
    [Tooltip("�Q�[�W����������̂ɂ����鎞�ԁi�b�j")]
    [SerializeField] private float increaseDuration = 0.5f; // �Q�[�W���������鎞��

    [Tooltip("�f�t�H���g�̃Q�[�W�����ʁi�V�[�����Ƃɒ����j")]
    [SerializeField] private int defaultIncreaseAmount = 1; // �f�t�H���g�̑�����

    private const int maxValue = 10; // �Q�[�W�̍ő�l
    private Queue<int> increaseQueue = new Queue<int>(); // �����ʂ����Ԃɏ������邽�߂̃L���[
    private bool isIncreasing = false; // ���݁A�Q�[�W�����������ǂ����𔻒肷��t���O

    private void Awake()
    {
        gameOverController = GameOverController.over;
        remainingAircraft = Remainingaircraft.remain;
    }

    // �I�u�W�F�N�g���j�󂳂ꂽ�ۂɃQ�[�W�𑝉�������
    /*public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount); // �f�t�H���g�̑����ʂŃQ�[�W�𑝉�
    }*/

    // �����Ŏw�肳�ꂽ�����ʂŃQ�[�W�𑝉�������
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount); // �����ʂ��L���[�ɒǉ�

        // �܂������������s���Ă��Ȃ���΁A�L���[�̏������J�n
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    // �L���[�ɂ��鑝���ʂ����ɏ���
    private IEnumerator ProcessQueue()
    {
        isIncreasing = true; // �Q�[�W�������t���O�𗧂Ă�

        // �L���[�ɑ����ʂ��c���Ă���ԁA�����𑱂���
        while (increaseQueue.Count > 0)
        {
            int amount = increaseQueue.Dequeue(); // �L���[���瑝���ʂ����o��
            yield return StartCoroutine(IncreaseGaugeSmoothly(amount)); // �Q�[�W���X���[�Y�ɑ���������
        }

        isIncreasing = false; // �Q�[�W������������
    }

    // �Q�[�W���X���[�Y�ɑ���������R���[�`��
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        if (gaugeSlider == null)
            yield break; // �Q�[�W�X���C�_�[���ݒ肳��Ă��Ȃ���Ώ����𒆒f

        float startValue = gaugeSlider.value; // �Q�[�W�̌��݂̒l
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue); // ������̒l�i�ő�l�𒴂��Ȃ��悤�ɐ����j

        float elapsed = 0f; // �o�ߎ���

        // �Q�[�W���������鎞�ԁi�X���[�Y�ɕω��j
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime; // �o�ߎ��Ԃ��X�V
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration); // �Q�[�W�l����`���
            yield return null; // ���̃t���[���܂őҋ@
        }

        gaugeSlider.value = endValue; // �Q�[�W�̍ŏI�l��ݒ�

        // �Q�[�W�����������^�C�~���O�Ŏc�@�����炷
        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife(); // �c�@��1���炷
        }

        // �Q�[�W���ő�l�ɒB�����ꍇ�A�Q�[���I�[�o�[�������s��
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver(); // �Q�[���I�[�o�[��\��
        }
    }
}
