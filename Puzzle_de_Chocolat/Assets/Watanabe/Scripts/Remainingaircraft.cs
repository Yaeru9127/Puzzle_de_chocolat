using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    public static Remainingaircraft remain { get; private set; }

    // 残機
    public List<GameObject> lifeSprites;

    // 数字スプライト（0～99）
    public Sprite[] numberSprites;

    // 数字表示用（UI用Image）
    public Image numberDisplay;

    // GameOver を制御するクラスへの参照
    public GameOverController gameOverController;

    // 現在の残機数
    public int currentLife;

    // ゲームクリア済みフラグ ← 追加
    public bool isGameCleared = false;

    private void Awake()
    {
        if (remain == null) remain = this;
        else if (remain != null) Destroy(this.gameObject);
    }

    void Start()
    {
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    public void ReduceLife()
    {
        // ゲームクリア後は処理しない
        if (currentLife > 0 && !isGameCleared)
        {
            currentLife--;

            lifeSprites[currentLife].SetActive(false);
            UpdateLifeDisplay();

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

    public void UpdateLifeDisplay()
    {
        if (currentLife >= 0 && currentLife < numberSprites.Length)
        {
            numberDisplay.sprite = numberSprites[currentLife];
            numberDisplay.gameObject.SetActive(true);
        }
    }

    // 数字スプライト取得用（他クラスから呼び出す用）
    public Sprite GetNumberSprite(int value)
    {
        if (numberSprites != null && value >= 0 && value < numberSprites.Length)
        {
            return numberSprites[value];
        }
        return null;
    }
}
