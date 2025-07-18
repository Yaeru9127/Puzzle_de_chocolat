using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public static GameClear clear { get; private set; }

    private CursorController cc;

    [Header("UI")]
    public Image clearImage;

    public Sprite noRetryFastClearSprite;               //星3   Sprite
    public Sprite retryOrNoRetryClearSprite;            //星2   Sprite
    public Sprite retryClearSprite;                     //星1   Sprite

    [Header("クリア評価設定")]
    public int minStepsToClear;                    // 最短ステップ数
    public int marginSteps;                         //
    public int stepsTaken = 0;                          // 現在のステップ数

    public bool wasEat;
    public bool wasMaked;

    [Header("ゲージ評価設定")]
    [Tooltip("noRetryFastClear を許容するゲージ増加回数（例: 3）")]
    public int allowedGaugeCount = 3;

    private void Awake()
    {
        if (clear == null) clear = this;
        else if (clear != null) Destroy(this.gameObject);
    }

    private void Start()
    {
        cc = CursorController.cc;
        wasEat = false;
        wasMaked = false;
    }

    // ステップ数を加算する
    public void AddStep()
    {
        stepsTaken++;
    }

    // クリア演出を実行
    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);
        AudioManager.Instance.PlaySE("Game clear");
        cc.ChangeCursorEnable(true);

        // ゲームクリア済みフラグを立てる → GameOverを防ぐ
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        if (stepsTaken <= 4)
        {
            if (wasEat) clearImage.sprite = retryOrNoRetryClearSprite;
            else clearImage.sprite = noRetryFastClearSprite;
        }
        else if (stepsTaken >= 5)
        {
            if (wasEat) clearImage.sprite = retryClearSprite;
            else clearImage.sprite = retryOrNoRetryClearSprite;
        }
        else
        {
            clearImage.sprite = retryClearSprite;
        }

        //if (stepsTaken <= minStepsToClear && !wasEat)
        //{
        //    clearImage.sprite = noRetryFastClearSprite;
        //}
        //else if (stepsTaken <= minStepsToClear + marginSteps)
        //{
        //    clearImage.sprite = retryOrNoRetryClearSprite;
        //}
        //else if (stepsTaken >= minStepsToClear + marginSteps && wasEat)
        //{
        //    clearImage.sprite = retryClearSprite;
        //}
        ///// 許容回数以内なら最短評価を許可
        //bool allowFastClear = GaugeController.gaugeIncreaseCount <= allowedGaugeCount;

        //if (stepsTaken <= minStepsToClear && allowFastClear)
        //{
        //    clearImage.sprite = noRetryFastClearSprite;
        //}
        //else if (stepsTaken <= minStepsToClear + marginSteps)
        //{
        //    clearImage.sprite = retryOrNoRetryClearSprite;
        //}
        //else
        //{
        //    clearImage.sprite = retryClearSprite;
        //}
    }

    // リザルトシーンに移動
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
