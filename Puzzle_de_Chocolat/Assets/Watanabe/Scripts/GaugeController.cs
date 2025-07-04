using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI関連")]
    [Tooltip("UIスライダー")]
    [SerializeField] private Slider gaugeSlider;

    [SerializeField] private GameOverController gameOverController;

    [SerializeField] private Remainingaircraft remainingAircraft; // ★ 残機管理クラスへの参照を追加

    [Header("ゲージ設定")]
    [Tooltip("ゲージが増加するのにかかる時間（秒）")]
    [SerializeField] private float increaseDuration = 0.5f;

    [Tooltip("デフォルトのゲージ増加量（シーンごとに調整）")]
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

        // ★ ゲージが増加したタイミングで残機を減らす
        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        // ゲージが最大になったらゲームオーバー
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
