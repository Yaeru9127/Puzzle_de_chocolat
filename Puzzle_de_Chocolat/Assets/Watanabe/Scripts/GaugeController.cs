using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI関連")]
    [Tooltip("UIスライダー")]
    [SerializeField] private Slider gaugeSlider; // ゲージのUIスライダー

    private GameOverController gameOverController; // ゲームオーバーを管理するコントローラー

    private Remainingaircraft remainingAircraft; // 残機管理クラスへの参照

    [Header("ゲージ設定")]
    [Tooltip("ゲージが増加するのにかかる時間（秒）")]
    [SerializeField] private float increaseDuration = 0.5f; // ゲージが増加する時間

    [Tooltip("デフォルトのゲージ増加量（シーンごとに調整）")]
    [SerializeField] private int defaultIncreaseAmount = 1; // デフォルトの増加量

    private const int maxValue = 10; // ゲージの最大値
    private Queue<int> increaseQueue = new Queue<int>(); // 増加量を順番に処理するためのキュー
    private bool isIncreasing = false; // 現在、ゲージが増加中かどうかを判定するフラグ

    private void Awake()
    {
        gameOverController = GameOverController.over;
        remainingAircraft = Remainingaircraft.remain;
    }

    // オブジェクトが破壊された際にゲージを増加させる
    /*public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount); // デフォルトの増加量でゲージを増加
    }*/

    // 引数で指定された増加量でゲージを増加させる
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount); // 増加量をキューに追加

        // まだ増加処理が行われていなければ、キューの処理を開始
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    // キューにある増加量を順に処理
    private IEnumerator ProcessQueue()
    {
        isIncreasing = true; // ゲージ増加中フラグを立てる

        // キューに増加量が残っている間、処理を続ける
        while (increaseQueue.Count > 0)
        {
            int amount = increaseQueue.Dequeue(); // キューから増加量を取り出す
            yield return StartCoroutine(IncreaseGaugeSmoothly(amount)); // ゲージをスムーズに増加させる
        }

        isIncreasing = false; // ゲージ増加処理完了
    }

    // ゲージをスムーズに増加させるコルーチン
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        if (gaugeSlider == null)
            yield break; // ゲージスライダーが設定されていなければ処理を中断

        float startValue = gaugeSlider.value; // ゲージの現在の値
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue); // 増加後の値（最大値を超えないように制限）

        float elapsed = 0f; // 経過時間

        // ゲージが増加する時間（スムーズに変化）
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime; // 経過時間を更新
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration); // ゲージ値を線形補間
            yield return null; // 次のフレームまで待機
        }

        gaugeSlider.value = endValue; // ゲージの最終値を設定

        // ゲージが増加したタイミングで残機を減らす
        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife(); // 残機を1つ減らす
        }

        // ゲージが最大値に達した場合、ゲームオーバー処理を行う
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver(); // ゲームオーバーを表示
        }
    }
}
