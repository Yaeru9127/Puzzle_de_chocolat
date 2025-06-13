using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    // �c�@
    public List<GameObject> lifeSprites;

    // �����X�v���C�g�i0�`9�j
    public Sprite[] numberSprites;

    // �����\���p�iUI�pImage�j
    public Image numberDisplay;

    // GameOver �𐧌䂷��N���X�ւ̎Q��
    public GameOverController gameOverController;

    // ���݂̎c�@��
    private int currentLife;

    void Start()
    {
        // �c�@�������X�g�̐����珉����
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    public void ReduceLife()
    {
        if (currentLife > 0)
        {
            currentLife--;

            // �c�@�A�C�R����\��
            lifeSprites[currentLife].SetActive(false);

            // �������X�V
            UpdateLifeDisplay();

            if (currentLife <= 0)
            {
                if (gameOverController != null)
                {
                    numberDisplay.gameObject.SetActive(false); // ������Image���\���ɂ���
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
        // �����X�v���C�g���p�ӂ���Ă���΍X�V
        if (currentLife >= 0 && currentLife < numberSprites.Length)
        {
            numberDisplay.sprite = numberSprites[currentLife];
        }
    }
}
