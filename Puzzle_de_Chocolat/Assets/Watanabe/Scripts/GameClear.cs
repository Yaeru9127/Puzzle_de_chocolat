using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Image clearImage;

    public Sprite noRetryFastClearSprite;       // ①リトライなし＆最短クリア
    public Sprite retryOrNoRetryClearSprite;    // ②リトライあり＆最短 or リトライなし
    public Sprite retryClearSprite;             // ③リトライあり

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

        // 最短かつリトライなし
        if (retryCount == 0 && stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = noRetryFastClearSprite;
        }
        // リトライあり or ノーリトライだけど最短でない
        else if (retryCount == 0 || stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = retryOrNoRetryClearSprite;
        }
        // 完全にリトライありでのクリア
        else
        {
            clearImage.sprite = retryClearSprite;
        }
    }
}
