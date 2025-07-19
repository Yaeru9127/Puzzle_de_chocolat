using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public static GameClear clear { get; private set; }

    private CursorController cc;

    [Header("UI")]
    public Image clearImage;

    public Sprite star3;            //星3   Sprite
    public Sprite star2;            //星2   Sprite
    public Sprite star1;            //星1   Sprite
    public Sprite star0;            //星0   Sprite

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

        int star = 3;
        if (wasEat)
        {
            star--;
            //Debug.Log("eat");
        }
        if (stepsTaken > 4)
        {
            star--;
            //Debug.Log("step is over 4");
        }
        if (stepsTaken > 6)
        {
            star--;
            //Debug.Log("step is over 6");
        }
        switch (star)
        {
            case 0:
                clearImage.sprite = star0;
                break;
            case 1:
                clearImage.sprite = star1;
                break;
            case 2:
                clearImage.sprite = star2;
                break;
            case 3:
                clearImage.sprite = star3;
                break;
            default:
                clearImage.sprite = null;
                break;
        }

        ////手数 <= 4    => 4手以内
        //if (stepsTaken <= 4)
        //{
        //    if (wasEat) clearImage.sprite = star2;      //4手以内で食べたら
        //    else clearImage.sprite = star3;             //4手以内で食べてないなら
        //}
        ////6 <= 手数 >= 5
        //else if (stepsTaken >= 5 && stepsTaken <= 6)
        //{
        //    if (wasEat) clearImage.sprite = star1;      //5,6手で食べたら
        //    else clearImage.sprite = star2;             //5,6手で食べてないなら
        //}
        //else
        //{
        //    clearImage.sprite = star1;
        //}

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
