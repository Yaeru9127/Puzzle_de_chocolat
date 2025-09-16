using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kari : MonoBehaviour
{
    public static kari remain { get; private set; }

    private GameClear clear;

    // �c�@�iUI��ɕ\������A�C�R���Ȃǁj
    public List<GameObject> lifeSprites;

    // �����X�v���C�g�i0�`99�j
    public Sprite[] numberSprites;

    // �����\���p��Image�iUI�j
    public Image numberDisplay;

    // GameOver���Ǘ�����X�N���v�g�ւ̎Q��
    public GameOverController gameOverController;

    // ���݂̎c�@��
    public int currentLife;

    // �Q�[���N���A�ς݂��ǂ����������t���O �� GameOver�h�~�p
    public bool isGameCleared = false;

    private void Awake()
    {
        // �V���O���g�����i���������h�~�j
        if (remain == null) remain = this;
        else if (remain != null) Destroy(this.gameObject);
    }

    void Start()
    {
        clear = GameClear.clear;

        // �c�@����������
        currentLife = lifeSprites.Count;
        UpdateLifeDisplay();
    }

    /*public void ReduceLife()
    {
        // �Q�[���N���A�ς݂Ȃ疳��
        if (currentLife > 0 && !isGameCleared)
        {
            currentLife--;
            clear.AddStep();

            // �\���p�X�v���C�g���\����
            lifeSprites[currentLife].SetActive(false);

            // �������X�V
            UpdateLifeDisplay();

            // --- GameOver�O�ɃS�[�������ǉ� ---
            if (currentLife <= 0)
            {
                // �S�[���ɓ��B���Ă����� GameClear ��D��
                if (CanGoal.cg != null && CanGoal.cg.IsPlayerOnGoal())
                {
                    isGameCleared = true; // GameOver���~�߂�
                    Debug.Log("�S�[���ɓ��B���Ă����̂� GameOver ���");
                    return;
                }

                // GameOver����
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
    }*/


    // �����\�����X�V�i�X�v���C�g�ؑցj
    public void UpdateLifeDisplay()
    {
        if (currentLife >= 0 && currentLife < numberSprites.Length)
        {
            numberDisplay.sprite = numberSprites[currentLife];
            numberDisplay.gameObject.SetActive(true);
        }
    }

    // �O�����琔�l�X�v���C�g���擾����֐�
    public Sprite GetNumberSprite(int value)
    {
        if (numberSprites != null && value >= 0 && value < numberSprites.Length)
        {
            return numberSprites[value];
        }
        return null;
    }

}
