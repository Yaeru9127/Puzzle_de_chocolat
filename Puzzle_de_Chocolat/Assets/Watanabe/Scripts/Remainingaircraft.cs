using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    public static Remainingaircraft remain { get; private set; }

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
        // 残機数を初期化
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    // 残機を1つ減らす
    public void ReduceLife()
    {
        // ゲームクリア後はGameOverを発生させない
        if (currentLife > 0 && !isGameCleared)
        {
            currentLife--;

            // 表示用スプライトを非表示に
            lifeSprites[currentLife].SetActive(false);

            // 数字を更新
            UpdateLifeDisplay();

            // 残機ゼロならGameOver処理
            if (currentLife <= 0)
            {
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
