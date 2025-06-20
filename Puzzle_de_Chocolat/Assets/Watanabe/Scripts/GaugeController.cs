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

    [Header("ゲージ設定")]
    [Tooltip("ゲージが増加するのにかかる時間（秒）")]
    [SerializeField] private float increaseDuration = 0.5f;

    [Tooltip("デフォルトのゲージ増加量（シーンごとに調整）")]
    [SerializeField] private int defaultIncreaseAmount = 1;

    // ゲージの最大値（固定値）
    private const int maxValue = 10;

    // ゲージ増加リクエストを保持するキュー
    private Queue<int> increaseQueue = new Queue<int>();

    // 現在ゲージが増加中かどうか
    private bool isIncreasing = false;

    /// <summary>
    /// 外部から呼び出す（デフォルトの増加量でゲージを増加）
    /// </summary>
    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    /// <summary>
    /// 外部から呼び出す（指定した量でゲージを増加）
    /// </summary>
    /// <param name="increaseAmount">増加量</param>
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount);

        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    /// <summary>
    /// ゲージ増加キューを処理するコルーチン
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
    /// ゲージを滑らかに増加させる処理
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

        // ゲージが最大になったらゲームオーバー
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
