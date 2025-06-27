using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    // 残機
    public List<GameObject> lifeSprites;

    // 数字スプライト（0～99）
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
        // 残機数が二桁対応
        int tens = currentLife / 10;  // 十の位
        int ones = currentLife % 10;  // 一の位

        // 1桁の時は、そのまま表示
        if (currentLife < 10)
        {
            numberDisplay.sprite = numberSprites[currentLife];  // 0-9のスプライト
        }
        else
        {
            // 二桁のとき、数値に応じてスプライトを変更
            numberDisplay.sprite = numberSprites[currentLife];  // 10-99のスプライト
        }

        // 数字が0のとき、Imageが非表示にならないように
        numberDisplay.gameObject.SetActive(true); // 常に表示
    }
}
