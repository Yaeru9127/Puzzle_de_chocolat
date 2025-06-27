using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    // 残機として表示するアイコン（UIオブジェクト）
    public List<GameObject> lifeSprites;

    // 0〜9 の数字スプライト（数字表示用）
    public Sprite[] numberSprites;

    // 数字表示に使う UI Image（2桁表示用）
    public Image[] numberDisplays;

    // GameOver 表示処理を持つクラス
    public GameOverController gameOverController;

    // 現在の残機数（lifeSprites.Countからスタート）
    private int currentLife;

    void Start()
    {
        // 初期残機数を設定
        currentLife = lifeSprites.Count;

        // 数字とアイコン表示を初期化
        UpdateLifeDisplay();
    }

    // 残機を1つ減らす処理
    public void ReduceLife()
    {
        if (currentLife > 0)
        {
            // 残機を1減らす
            currentLife--;

            // 該当するアイコンを非表示にする
            lifeSprites[currentLife].SetActive(false);

            // 数字表示更新
            UpdateLifeDisplay();

            // ゲームオーバー処理（0以下になったとき）
            if (currentLife <= 0)
            {
                // 数字表示をすべて非表示にする
                foreach (var display in numberDisplays)
                {
                    display.gameObject.SetActive(false);
                }

                // GameOver処理を呼び出す
                if (gameOverController != null)
                {
                    gameOverController.ShowGameOver();
                }
                else
                {
                    Debug.Log("Game Over");
                }
            }
        }
    }

    // 残機数をUIに反映する処理（2桁数字）
    void UpdateLifeDisplay()
    {
        int tens = currentLife / 10;  // 10の位
        int ones = currentLife % 10;  // 1の位

        // 10の位の表示
        if (tens >= 0 && tens < numberSprites.Length && numberDisplays.Length > 0)
        {
            numberDisplays[0].sprite = numberSprites[tens];

            // 10未満のときは非表示にして0を見せないようにする
            numberDisplays[0].gameObject.SetActive(tens > 0 || currentLife >= 10);
        }

        // 1の位の表示（常に表示）
        if (ones >= 0 && ones < numberSprites.Length && numberDisplays.Length > 1)
        {
            numberDisplays[1].sprite = numberSprites[ones];
            numberDisplays[1].gameObject.SetActive(true);
        }
    }
}
