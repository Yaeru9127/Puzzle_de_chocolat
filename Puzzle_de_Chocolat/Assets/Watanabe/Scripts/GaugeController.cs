using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeController : MonoBehaviour
{
    // ゲージとして使用する UI の Slider をインスペクターから設定
    public Slider gaugeSlider;

    // ゲージの最大値
    private int maxValue = 10;

    // ゲージを増加させるのにかかる時間（秒）
    private float increaseDuration = 0.5f;

    // オブジェクトが破壊されたときに呼び出す関数
    public void OnObjectDestroyed()
    {
        // ゲージを1増やす処理をコルーチンで開始
        StartCoroutine(IncreaseGaugeSmoothly(1));
    }

    // ゲージを滑らかに増加させるコルーチン
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        // 現在のゲージ値を取得
        float startValue = gaugeSlider.value;

        // 増加後の値を計算（maxValueを超えないようにClampで制限）
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue);

        // 経過時間の初期化
        float elapsed = 0f;

        // increaseDuration 秒間かけてゲージを滑らかに変化させる
        while (elapsed < increaseDuration)
        {
            // 経過時間を加算
            elapsed += Time.deltaTime;

            // 線形補間（Lerp）でゲージの値を更新
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration);

            // 1フレーム待機してから次のループへ
            yield return null;
        }

        // 最終的な値を正確にセット（Lerpの誤差を防ぐため）
        gaugeSlider.value = endValue;
    }
}
