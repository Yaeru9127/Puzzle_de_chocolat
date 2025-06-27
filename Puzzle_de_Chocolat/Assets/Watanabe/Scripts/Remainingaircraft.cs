using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    // �c�@
    public List<GameObject> lifeSprites;

    // �����X�v���C�g�i0�`99�j
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
        // �c�@�����񌅑Ή�
        int tens = currentLife / 10;  // �\�̈�
        int ones = currentLife % 10;  // ��̈�

        // 1���̎��́A���̂܂ܕ\��
        if (currentLife < 10)
        {
            numberDisplay.sprite = numberSprites[currentLife];  // 0-9�̃X�v���C�g
        }
        else
        {
            // �񌅂̂Ƃ��A���l�ɉ����ăX�v���C�g��ύX
            numberDisplay.sprite = numberSprites[currentLife];  // 10-99�̃X�v���C�g
        }

        // ������0�̂Ƃ��AImage����\���ɂȂ�Ȃ��悤��
        numberDisplay.gameObject.SetActive(true); // ��ɕ\��
    }
}
