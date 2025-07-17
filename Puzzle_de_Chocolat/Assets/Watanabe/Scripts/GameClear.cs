using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Image clearImage;

    public Sprite noRetryFastClearSprite;        // リトライなし＆最短
    public Sprite retryOrNoRetryClearSprite;     // リトライなし or 最短でない
    public Sprite retryClearSprite;              // リトライあり

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

        // ★GameOverを無効化
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
