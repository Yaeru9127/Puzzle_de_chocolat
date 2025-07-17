using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Image clearImage;

    public Sprite noRetryFastClearSprite;        // ���g���C�Ȃ����ŒZ
    public Sprite retryOrNoRetryClearSprite;     // ���g���C�Ȃ� or �ŒZ�łȂ�
    public Sprite retryClearSprite;              // ���g���C����

    public int minStepsToClear = 10;
    public int minRetriesToClear = 2;

    public int stepsTaken = 0;

    public void AddStep()
    {
        stepsTaken++;
    }

    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);
        AudioManager.Instance.PlaySE("Game clear");

        // ��GameOver�𖳌���
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        if (retry == 0 && stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = noRetryFastClearSprite;
        }
        else if (retry >= minRetriesToClear || stepsTaken > minStepsToClear)
        {
            clearImage.sprite = retryClearSprite;
        }
        else
        {
            clearImage.sprite = retryOrNoRetryClearSprite;
        }
    }

    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
