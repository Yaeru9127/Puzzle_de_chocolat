using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI関連")]
    [Tooltip("ゲージバーのスライダーコンポーネント")]
    [SerializeField] private Slider gaugeSlider;

    [Tooltip("GameOverを管理するクラス")]
    [SerializeField] private GameOverController gameOverController;

    [Tooltip("残機を管理するクラス")]
    [SerializeField] private Remainingaircraft remainingAircraft;

    [Header("ゲージ設定")]
    [Tooltip("ゲージが増加するまでの時間（秒）")]
    [SerializeField] private float increaseDuration = 0.5f;

    [Tooltip("通常のゲージ増加量（指定がない場合）")]
    [SerializeField] private int defaultIncreaseAmount = 1;

    // ゲージの最大値（固定）
    private const int maxValue = 10;

    // ゲージ増加リクエストを保持するキュー
    private Queue<int> increaseQueue = new Queue<int>();

    // 現在ゲージが増加中かどうかのフラグ
    private bool isIncreasing = false;

    /// <summary>
    /// デフォルトの増加量でゲージを増加させる（外部呼び出し用）
    /// </summary>
    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    /// <summary>
    /// 指定した量でゲージを増加させ、残機も減らす
    /// </summary>
    /// <param name="increaseAmount">ゲージの増加量</param>
    public void OnObjectDestroyed(int increaseAmount)
    {
        // ゲージ増加リクエストをキューに追加
        increaseQueue.Enqueue(increaseAmount);

        // 残機も減らす
        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        // 増加処理が動いていなければコルーチン開始
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
    /// ゲージを滑らかに増加させるコルーチン
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

        // ゲージが最大になったらGameOver発動
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
