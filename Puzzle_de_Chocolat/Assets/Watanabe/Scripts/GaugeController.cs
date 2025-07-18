using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲージの制御を行うクラス。
/// オブジェクトの破壊によりゲージが増加し、
/// 一定値に達するとゲームオーバーを発生させる処理を含む。
/// </summary>
public class GaugeController : MonoBehaviour
{
    [Header("UI関連")]
    [SerializeField] private Image gaugeFillImage; // ゲージの進行度を表すUI Image
    [Tooltip("ゲージ残り回数を数字スプライトで表示するUI Image")]
    [SerializeField] private Image gaugeNumberDisplay; // 残り回数を表示するUI
    [SerializeField] private GameOverController gameOverController; // ゲームオーバー処理クラスへの参照
    [SerializeField] private Remainingaircraft remainingAircraft; // 残機UIなどの処理クラスへの参照

    [Header("ゲージ設定")]
    [SerializeField] private float increaseDuration = 0.5f; // ゲージが増加するアニメーションの所要時間
    [SerializeField] private int defaultIncreaseAmount = 1; // デフォルトの増加量（1回の増加あたり）
    [SerializeField] private int maxValue = 6; // ゲージの最大値（到達でゲームオーバー）

    private int currentValue = 0; // 現在のゲージの値
    private Queue<int> increaseQueue = new Queue<int>(); // ゲージ増加のキュー（複数増加に対応）
    private bool isIncreasing = false; // ゲージが現在増加中かどうかのフラグ

    public bool gaugeIncreased = false; // ゲージが1度でも増加したか
    public static int gaugeIncreaseCount = 0; // ゲージの増加が何回発生したか

    void Start()
    {
        gaugeIncreased = false;
        gaugeIncreaseCount = 0; // ステージ開始時にリセット
        UpdateGaugeNumberDisplay(); // 初期の残り回数表示更新
    }

    /// <summary>
    /// オブジェクト破壊時に呼ばれる（増加量デフォルト）
    /// </summary>
    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    /// <summary>
    /// オブジェクト破壊時に呼ばれる（指定量の増加）
    /// </summary>
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount); // 増加要求をキューに追加
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue()); // キュー処理開始
        }
    }

    /// <summary>
    /// キューに入っている増加処理を順番に実行
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
    /// ゲージを指定量だけスムーズに増加させる処理
    /// </summary>
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        if (gaugeFillImage == null)
            yield break;

        gaugeIncreased = true;
        gaugeIncreaseCount++; // 増加回数カウント

        int oldValue = currentValue;
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);

        float startFill = (float)oldValue / maxValue;
        float endFill = (float)currentValue / maxValue;

        float elapsed = 0f;
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;
            gaugeFillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsed / increaseDuration);
            yield return null;
        }

        gaugeFillImage.fillAmount = endFill;
        UpdateGaugeNumberDisplay(); // 数字UI更新

        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife(); // 残機を1減らす
        }

        // ゲージが最大値に達したらゲームオーバー表示
        if (currentValue >= maxValue && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }

    /// <summary>
    /// 残り回数の数字スプライトを更新
    /// </summary>
    private void UpdateGaugeNumberDisplay()
    {
        if (gaugeNumberDisplay == null || remainingAircraft == null)
            return;

        int remaining = Mathf.Clamp(maxValue - currentValue, 0, 99);
        Sprite sprite = remainingAircraft.GetNumberSprite(remaining);

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
