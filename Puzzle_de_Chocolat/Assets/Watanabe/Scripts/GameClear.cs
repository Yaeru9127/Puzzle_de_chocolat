using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Image clearImage; // �N���A���ʂ�\������摜

    // �󋵂ɉ������N���A���ʂ̃X�v���C�g
    public Sprite noRetryFastClearSprite;       // ���g���C�Ȃ����ŒZ
    public Sprite retryOrNoRetryClearSprite;    // ���g���C�Ȃ� or �ŒZ�N���A�łȂ�
    public Sprite retryClearSprite;             // ���g���C����

    // �ŒZ�N���A�������
    public int minStepsToClear = 10;       // �ŒZ�X�e�b�v��
    public int minRetriesToClear = 2;      // ���g���C����Ɣ��肷���

    public int stepsTaken = 0; // ���݂̃X�e�b�v��

    // �X�e�b�v�������Z����i�Ăяo�����Ŏg�p�j
    public void AddStep()
    {
        stepsTaken++;
    }

    // �N���A���o�����s
    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);
        AudioManager.Instance.PlaySE("Game clear");

        // �Q�[���N���A�ς݃t���O�𗧂Ă� �� GameOver��h��
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        // ����ɉ����ăX�v���C�g�ύX
        if (retry == 0 && stepsTaken <= minStepsToClear)
        {
            // �ŒZ���m�[���g���C
            clearImage.sprite = noRetryFastClearSprite;
        }
        else if (retry >= minRetriesToClear || stepsTaken > minStepsToClear)
        {
            // ���g���C���� or �ŒZ�łȂ�
            clearImage.sprite = retryClearSprite;
        }
        else
        {
            // �m�[���g���C�����ŒZ�ł��Ȃ�
            clearImage.sprite = retryOrNoRetryClearSprite;
        }
    }

    // ���U���g�V�[���Ɉړ�
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
