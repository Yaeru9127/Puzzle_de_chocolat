using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    public static Remainingaircraft remain { get; private set; }

    private GameClear clear;

    // 残機（UI上に表示するアイコンなど）
    public List<GameObject> lifeSprites;

    // 数字スプライト（0～99）
    public Sprite[] numberSprites;

    // 数字表示用のImage（UI）
    public Image numberDisplay;

    // GameOverを管理するスクリプトへの参照
    public GameOverController gameOverController;

    // 現在の残機数
    public int currentLife;

    // ゲームクリア済みかどうかを示すフラグ ← GameOver防止用
    public bool isGameCleared = false;

    private void Awake()
    {
        // シングルトン化（複数生成防止）
        if (remain == null) remain = this;
        else if (remain != null) Destroy(this.gameObject);
    }

    void Start()
    {
        clear = GameClear.clear;

        // 残機数を初期化
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    public void ReduceLife()
    {
        // ゲームクリア済みなら無視
        if (currentLife > 0 && !isGameCleared)
        {
            currentLife--;
            clear.AddStep();

            // 表示用スプライトを非表示に
            lifeSprites[currentLife].SetActive(false);

            // 数字を更新
            UpdateLifeDisplay();

            // --- GameOver前にゴール判定を追加 ---
            if (currentLife <= 0)
            {
                // ゴールに到達していたら GameClear を優先
                if (CanGoal.cg != null && CanGoal.cg.IsPlayerOnGoal())
                {
                    isGameCleared = true; // GameOverを止める
                    Debug.Log("ゴールに到達していたので GameOver 回避");
                    return;
                }

                // GameOver処理
                if (gameOverController != null)
                {
                    numberDisplay.gameObject.SetActive(false);
                    gameOverController.ShowGameOver();
                }
                else
                {
                    Debug.Log("Game Over");
                }
            }
        }
    }


    // 数字表示を更新（スプライト切替）
    public void UpdateLifeDisplay()
    {
        if (currentLife >= 0 && currentLife < numberSprites.Length)
        {
            numberDisplay.sprite = numberSprites[currentLife];
            numberDisplay.gameObject.SetActive(true);
        }
    }

    // 外部から数値スプライトを取得する関数
    public Sprite GetNumberSprite(int value)
    {
        if (numberSprites != null && value >= 0 && value < numberSprites.Length)
        {
            return numberSprites[value];
        }
        return null;
    }

}
