using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainingaircraft : MonoBehaviour
{
    // �c�@�A�C�R���i��s�@�Ȃǁj
    public List<GameObject> lifeSprites;

    // �����X�v���C�g�i0?9�j
    public Sprite[] numberSprites;

    // �����\���p�iUI�pImage�j
    public Image numberDisplay;

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
                GameOver();
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

    void GameOver()
    {
        Debug.Log("Game Over");
        // �����ŃV�[���J�ڂȂ�
    }
}
