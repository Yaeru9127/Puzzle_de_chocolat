using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    // UI��Slider�R���|�[�l���g�i�Q�[�W�j���A�T�C��
    public Slider gaugeSlider;

    // �Q�[���I�[�o�[������S������X�N���v�g�ւ̎Q��
    public GameOverController gameOverController;

    // �Q�[�W�̍ő�l
    private int maxValue = 10;

    // �Q�[�W����������̂ɂ����鎞�ԁi�b�j
    private float increaseDuration = 0.5f;

    // �Q�[�W�̑������N�G�X�g��ێ�����L���[
    private Queue<int> increaseQueue = new Queue<int>();

    // ���݃Q�[�W�𑝉������ǂ���
    private bool isIncreasing = false;

    // �Q�[�W���������̃g���K�[�i�Ⴆ�ΓG���j�󂳂ꂽ�Ƃ��ɌĂяo���j
    public void OnObjectDestroyed()
    {
        // �������N�G�X�g���L���[�ɒǉ�
        increaseQueue.Enqueue(1);

        // �܂������������łȂ���΃R���[�`�����J�n
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    // �L���[�ɗ��܂����������N�G�X�g�����Ԃɏ�������R���[�`��
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

    // �Q�[�W�����炩�ɑ���������R���[�`��
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        // �X���C�_�[�����ݒ�̏ꍇ�͏����𒆒f
        if (gaugeSlider == null)
            yield break;

        float startValue = gaugeSlider.value;
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue);

        float elapsed = 0f;

        // �w�肳�ꂽ���ԂŃQ�[�W�����炩�ɕω�
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration);
            yield return null;
        }

        // �ŏI�I�Ȓl��ݒ�
        gaugeSlider.value = endValue;

        // �Q�[�W���ő�l�ɒB������Q�[���I�[�o�[�������Ăяo��
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
