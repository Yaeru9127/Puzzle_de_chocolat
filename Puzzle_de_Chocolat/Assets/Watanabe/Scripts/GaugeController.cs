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

    [SerializeField] private Remainingaircraft remainingAircraft; // �� �c�@�Ǘ��N���X�ւ̎Q�Ƃ�ǉ�

    [Header("�Q�[�W�ݒ�")]
    [Tooltip("�Q�[�W����������̂ɂ����鎞�ԁi�b�j")]
    [SerializeField] private float increaseDuration = 0.5f;

    [Tooltip("�f�t�H���g�̃Q�[�W�����ʁi�V�[�����Ƃɒ����j")]
    [SerializeField] private int defaultIncreaseAmount = 1;

    private const int maxValue = 10;
    private Queue<int> increaseQueue = new Queue<int>();
    private bool isIncreasing = false;

    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount);

        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

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

        // �� �Q�[�W�����������^�C�~���O�Ŏc�@�����炷
        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        // �Q�[�W���ő�ɂȂ�����Q�[���I�[�o�[
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
