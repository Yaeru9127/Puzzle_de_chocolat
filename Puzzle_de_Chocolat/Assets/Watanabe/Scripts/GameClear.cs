using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameClear;

public class GameClear : MonoBehaviour
{
    [Serializable]
    public class StageCondition
    {
        public List<int> stepPenalties = new List<int>();
        public List<string> requiredSweets = new List<string>();
        public int eatNum;
    }

    public static GameClear clear { get; private set; }

    [Header("UI")]
    public Image clearImage;
    public Sprite star3;
    public Sprite star2;
    public Sprite star1;
    public Sprite star0;

    [Header("ゲーム評価データ")]
    [Tooltip("現在のステージ番号。")]
    public int currentStage = 1;
    public int stepsTaken = 0;
    public int wasEat = 0;
    public bool wasMaked = false;

    public Dictionary<int, StageCondition> stageConditions; 

    [HideInInspector]
    public List<string> madeSweets;

    private void Awake()
    {
        if (clear == null) 
        {
            clear = this; 
        }
        else if (clear != this)
        {
            Destroy(this.gameObject); 
        }

    }

    private void Start()
    {
        // リストを初期化
        madeSweets = new List<string>();
        stepsTaken = 0;
        wasEat = 0;
        wasMaked = false;

        SetStageConditions();
    }

    /// <summary>
    /// クリア条件設定関数
    /// </summary>
    private void SetStageConditions()
    {
        stageConditions = new Dictionary<int, StageCondition>();

        //ステージ1
        stageConditions[1] = new StageCondition
        {
            stepPenalties = new List<int> { 4, 6 },
            requiredSweets = new List<string>(),
            eatNum = 0
        };

        //ステージ2
        stageConditions[2] = new StageCondition
        {
            stepPenalties = new List<int>(),
            requiredSweets = new List<string> { "pannacotta", "tiramisu" },
            eatNum = 2
        };

        //ステージ3
        stageConditions[3] = new StageCondition
        {
            stepPenalties = new List<int> { 14 },
            requiredSweets = new List<string> { "canulé" },
            eatNum = 3
        };
    }

    /// <summary>
    /// 食べた回数を増やす関数
    /// </summary>
    public void AddWasEat()
    {
        wasEat++;
    }

    /// <summary>
    /// お菓子が作られたときに呼び出すメソッド
    /// </summary>
    public void MadeSweets(string sweetsName)
    {
        if (!madeSweets.Contains(sweetsName))
        {
            madeSweets.Add(sweetsName);
        }
    }

    /// <summary>
    /// ステップ数を加算する
    /// </summary>
    public void AddStep()
    {
        stepsTaken++;
    }

    /// <summary>
    /// クリア演出を実行し、星の数を計算する
    /// </summary>
    public void  ShowClearResult()
    {
        clearImage.gameObject.SetActive(true);
        // AudioManager.Instance.PlaySE("Game clear");

        // RemainingaircraftスクリプトのisGameClearedフラグを立てる
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        int star = 3;

        //ステージごとに設定しておく
        /*チュートリアルステージは評価条件を無視*/
        if (StageManager.stage.stagenum != 0)
        {
            StageCondition stageCondition = stageConditions[StageManager.stage.stagenum];

            // ステップ数の減点
            foreach (int step in stageCondition.stepPenalties)
            {
                //Debug.Log(penaltyStep);
                if (stepsTaken > step) star--;
            }

            // 必須のお菓子の減点
            if (stageCondition.requiredSweets != null && stageCondition.requiredSweets.Count > 0)
            {
                foreach (string sweet in stageCondition.requiredSweets)
                {
                    if (!madeSweets.Contains(sweet)) star--;
                }
            }

            // 食べた回数の減点
            if (wasEat != stageCondition.eatNum)
            {
                star--;
            }
        }

        // 星の数が0未満にならないように調整
        if (star < 0) star = 0;

        //画像の設定
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

    /// <summary>
    /// リザルトシーンに移動
    /// </summary>
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }

    /// <summary>
    /// チュートリアル専用シーン読み込み関数
    /// </summary>
    /// <param name="num"></param>
    public void TutorialSceneLoad(int num)
    {
        //数字で判断
        string scenename = "";
        switch (num)
        {
            case 1:
                scenename = "Tutorial1";
                break;
            case 2:
                scenename = "Tutorial2";
                break;
            case 3:
                scenename = "Tutorial3";
                break;
            case 4:
                scenename = "Tutorial4";
                    break;
            default:
                scenename = "StageSelect";
                break;
        }

        //シーン読み込み
        SceneManager.LoadScene(scenename);
    }
}