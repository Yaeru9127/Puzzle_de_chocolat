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
    private GameOverController gameOverController;

    // 現在の残機数
    public int currentLife;

    private void Awake()
    {
        if (remain == null) remain = this;
        else if (remain != null) Destroy(this.gameObject);
    }

    void Start()
    {
        gameOverController = GameOverController.over;
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    public void ReduceLife()
    {
        if (currentLife > 0)
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

    void UpdateLifeDisplay()
    {
        int tens = currentLife / 10;
        int ones = currentLife % 10;

        if (currentLife < 10)
        {
            numberDisplay.sprite = numberSprites[currentLife];
        }
        else
        {
            numberDisplay.sprite = numberSprites[currentLife];
        }

        numberDisplay.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        //シーンを跨ぐときにメモリから消す
        if (remain == this) remain = null;
    }
}
