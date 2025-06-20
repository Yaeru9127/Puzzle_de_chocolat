using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI�֘A")]
    [Tooltip("UI�X���C�_�[")]
    [SerializeField] private Slider gaugeSlider;

    [SerializeField] private GameOverController gameOverController;

    [Header("�Q�[�W�ݒ�")]
    [Tooltip("�Q�[�W����������̂ɂ����鎞�ԁi�b�j")]
    [SerializeField] private float increaseDuration = 0.5f;

    [Tooltip("�f�t�H���g�̃Q�[�W�����ʁi�V�[�����Ƃɒ����j")]
    [SerializeField] private int defaultIncreaseAmount = 1;

    // �Q�[�W�̍ő�l�i�Œ�l�j
    private const int maxValue = 10;

    // �Q�[�W�������N�G�X�g��ێ�����L���[
    private Queue<int> increaseQueue = new Queue<int>();

    // ���݃Q�[�W�����������ǂ���
    private bool isIncreasing = false;

    /// <summary>
    /// �O������Ăяo���i�f�t�H���g�̑����ʂŃQ�[�W�𑝉��j
    /// </summary>
    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    /// <summary>
    /// �O������Ăяo���i�w�肵���ʂŃQ�[�W�𑝉��j
    /// </summary>
    /// <param name="increaseAmount">������</param>
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount);

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
    /// �Q�[�W�����炩�ɑ��������鏈��
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

        // �Q�[�W���ő�ɂȂ�����Q�[���I�[�o�[
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
