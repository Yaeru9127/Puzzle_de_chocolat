using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Image clearImage;

    public Sprite noRetryFastClearSprite;       // �@���g���C�Ȃ����ŒZ�N���A
    public Sprite retryOrNoRetryClearSprite;    // �A���g���C���聕�ŒZ or ���g���C�Ȃ�
    public Sprite retryClearSprite;             // �B���g���C����

    public int minStepsToClear = 10;

    private int retryCount = 0;
    private int stepsTaken = 0;

    public void AddRetry()
    {
        retryCount++;
    }

    public void AddStep()
    {
        stepsTaken++;
    }

    public void ShowClearResult()
    {
        clearImage.gameObject.SetActive(true);

        // �ŒZ�����g���C�Ȃ�
        if (retryCount == 0 && stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = noRetryFastClearSprite;
        }
        // ���g���C���� or �m�[���g���C�����ǍŒZ�łȂ�
        else if (retryCount == 0 || stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = retryOrNoRetryClearSprite;
        }
        // ���S�Ƀ��g���C����ł̃N���A
        else
        {
            clearImage.sprite = retryClearSprite;
        }
    }
}
