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

    [Header("ゲージ評価設定")]
    [Tooltip("noRetryFastClear を許容するゲージ増加回数（例: 3）")]
    public int allowedGaugeCount = 3;

    //public bool wasEat;
    public int wasEat =0;
    public bool wasMaked = false;


    //お菓子が作られた記録するを追加
    public bool hasPannacotta = false;
    public bool hasThiramisu = false;

    public int star = 3;
    private void Awake()
    {
        if (clear == null) clear = this;
        else if (clear != null) Destroy(this.gameObject);
    }

    private void Start()
    {
        cc = CursorController.cc;
        //v wasEat = false;
        wasMaked = false;
        //増やしました
        hasPannacotta = false;
        hasThiramisu = false;
    }
    //お菓子が作られたときに呼び出すwを追加
    public void MadeSweets(string sweetsName)
    {
       if(sweetsName =="pannacotta")
        {
            hasPannacotta = true;
        }
       else if(sweetsName =="thiramisu")
        {
            hasThiramisu = true;
        }
            
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

        ////ここから
        //int star = 3;
        //if (wasEat >= 1)
        //{
        //    star--;
        //    //Debug.Log("eat");
        //}
        //if (stepsTaken > 4)
        //{
        //    star--;
        //    //Debug.Log("step is over 4");
        //}
        //if (stepsTaken > 6)
        //{
        //    star--;
        //    //Debug.Log("step is over 6");
        //}
        //switch (star)
        //{
        //    case 0:
        //        clearImage.sprite = star0;
        //        break;
        //    case 1:
        //        clearImage.sprite = star1;
        //        break;
        //    case 2:
        //        clearImage.sprite = star2;
        //        break;
        //    case 3:
        //        clearImage.sprite = star3;
        //        break;
        //    default:
        //        clearImage.sprite = null;
        //        break;
        //}

        if (!hasPannacotta)
        {
            star--;
            //Debug.Log("eat");
        }
        if (!hasThiramisu)
        {
            star--;
            //Debug.Log("step is over 4");
        }
        if (wasEat >= 3)
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
    }

    // リザルトシーンに移動
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
