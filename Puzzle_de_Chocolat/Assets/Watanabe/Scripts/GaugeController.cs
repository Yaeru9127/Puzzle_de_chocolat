using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    // UIのSliderコンポーネント（ゲージ）をアサイン
    public Slider gaugeSlider;

    // ゲームオーバー処理を担当するスクリプトへの参照
    public GameOverController gameOverController;

    // ゲージの最大値
    private int maxValue = 10;

    // ゲージが増加するのにかかる時間（秒）
    private float increaseDuration = 0.5f;

    // ゲージの増加リクエストを保持するキュー
    private Queue<int> increaseQueue = new Queue<int>();

    // 現在ゲージを増加中かどうか
    private bool isIncreasing = false;

    // ゲージ増加処理のトリガー（例えば敵が破壊されたときに呼び出す）
    public void OnObjectDestroyed()
    {
        // 増加リクエストをキューに追加
        increaseQueue.Enqueue(1);

        // まだ増加処理中でなければコルーチンを開始
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    // キューに溜まった増加リクエストを順番に処理するコルーチン
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

    // ゲージを滑らかに増加させるコルーチン
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        // スライダーが未設定の場合は処理を中断
        if (gaugeSlider == null)
            yield break;

        float startValue = gaugeSlider.value;
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue);

        float elapsed = 0f;

        // 指定された時間でゲージを滑らかに変化
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration);
            yield return null;
        }

        // 最終的な値を設定
        gaugeSlider.value = endValue;

        // ゲージが最大値に達したらゲームオーバー処理を呼び出す
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
