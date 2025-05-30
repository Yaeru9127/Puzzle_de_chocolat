using System.Collections.Generic;
using UnityEngine;

public class Remainingaircraft : MonoBehaviour
{
    // 残機を表すスプライト（またはUI）を登録するリスト
    public List<GameObject> lifeSprites;

    // 現在の残機数
    private int currentLife;

    // ゲーム開始時に残機を初期化
    void Start()
    {
        // 残機数をリストの要素数から設定
        currentLife = lifeSprites.Count;
    }

    // 残機を1つ減らす処理
    public void ReduceLife()
    {
        if (currentLife > 0)
        {
            // 残機数を1つ減らす
            currentLife--;

            // 対応するスプライトを非表示にする
            lifeSprites[currentLife].SetActive(false);

            // 残機が0以下になったらゲームオーバー処理を呼ぶ
            if (currentLife <= 0)
            {
                GameOver();
            }
        }
    }

    // ゲームオーバー時の処理
    void GameOver()
    {
        Debug.Log("Game Over");

        // 必要に応じてここにシーン遷移
    }
}
