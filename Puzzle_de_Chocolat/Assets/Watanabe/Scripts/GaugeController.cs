using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

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
    private GameOverController over; // ゲームオーバー処理クラスへの参照
    private Remainingaircraft remain; // 残機UIなどの処理クラスへの参照

    [Header("ゲージ設定")]
    [SerializeField] private float increaseDuration = 0.5f; // ゲージが増加するアニメーションの所要時間
    [SerializeField] private int defaultIncreaseAmount = 1; // デフォルトの増加量（1回の増加あたり）
    [SerializeField] public int maxValue; // ゲージの最大値

    public int currentValue = 0; // 現在のゲージの値
    private Queue<int> increaseQueue = new Queue<int>(); // ゲージ増加のキュー（複数増加に対応）
    private bool isIncreasing = false; // ゲージが現在増加中かどうかのフラグ

    public bool gaugeIncreased = false; // ゲージが1度でも増加したか
    public static int gaugeIncreaseCount = 0; // ゲージの増加が何回発生したか

    void Start()
    {
        over = GameOverController.over;
        remain = Remainingaircraft.remain;

        gaugeIncreased = false;
        gaugeIncreaseCount = 0; // ステージ開始時にリセット
        UpdateGaugeNumberDisplay(); // 初期の残り回数表示更新
    }

    /// <summary>
    /// オブジェクト破壊時に呼ばれる（増加量デフォルト）
    /// </summary>
    //public void OnObjectDestroyed()
    //{
    //    OnObjectDestroyed(defaultIncreaseAmount);
    //}

    /// <summary>
    /// オブジェクト破壊時に呼ばれる（指定量の増加）
    /// </summary>
    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount); // 増加要求をキューに追加
        if (!isIncreasing)
        {
            ProcessQueueAsync().Forget(); // ★ Coroutine → UniTask 
        }
    }

    /// <summary>
    /// キューに入っている増加処理を順番に実行（Coroutine → UniTask）
    /// </summary>
    private async UniTaskVoid ProcessQueueAsync()
    {
        isIncreasing = true;

        while (increaseQueue.Count > 0)
        {
            int amount = increaseQueue.Dequeue();
            await IncreaseGaugeSmoothlyAsync(amount); // ★ Coroutine → await
        }

        isIncreasing = false;
    }

    /// <summary>
    /// ゲージを指定量だけスムーズに増加させる処理（Coroutine → UniTask）
    /// </summary>
    private async UniTask IncreaseGaugeSmoothlyAsync(int amount)
    {
        if (gaugeFillImage == null)
            return;

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
            await UniTask.Yield(); // コルーチンの代替（次のフレームまで待つ）
        }

        gaugeFillImage.fillAmount = endFill;
        UpdateGaugeNumberDisplay(); // 数字UI更新

        if (remain != null)
        {
            remain.ReduceLife(); // 残機を1減らす
        }

        // ゲージが最大値に達したらゲームオーバー表示
        if (currentValue >= maxValue && over != null)
        {
            over.ShowGameOver();
        }
    }

    /// <summary>
    /// 残り回数の数字スプライトを更新
    /// </summary>
    private void UpdateGaugeNumberDisplay()
    {
        if (gaugeNumberDisplay == null || remain == null)
            return;

        int remaining = Mathf.Clamp(maxValue - currentValue, 0, 99);
        Sprite sprite = remain.GetNumberSprite(remaining);

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
