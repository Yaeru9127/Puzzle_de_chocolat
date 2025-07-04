using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    // 残機
    public List<GameObject> lifeSprites;

    // 数字スプライト（0～9）
    public Sprite[] numberSprites;

    // 数字表示用（UI用Image）
    public Image numberDisplay;

    // GameOver を制御するクラスへの参照
    public GameOverController gameOverController;

    // 現在の残機数
    private int currentLife;

    void Start()
    {
        // 残機数をリストの数から初期化
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    public void ReduceLife()
    {
        if (currentLife > 0)
        {
            currentLife--;

            // 残機アイコン非表示
            lifeSprites[currentLife].SetActive(false);

            // 数字も更新
            UpdateLifeDisplay();

            if (currentLife <= 0)
            {
                if (gameOverController != null)
                {
                    numberDisplay.gameObject.SetActive(false); // 数字のImageを非表示にする
                    gameOverController.ShowGameOver();
                }

                else
                {
                    Debug.Log("Game Over");
                }
            }
        }
    }

    void UpdateLifeDisplay()
    {
        // 数字スプライトが用意されていれば更新
        if (currentLife >= 0 && currentLife < numberSprites.Length)
        {
            numberDisplay.sprite = numberSprites[currentLife];
        }
    }
}
