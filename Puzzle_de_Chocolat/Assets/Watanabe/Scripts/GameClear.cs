using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Image clearImage; // クリア結果を表示する画像

    // 状況に応じたクリア結果のスプライト
    public Sprite noRetryFastClearSprite;       // リトライなし＆最短
    public Sprite retryOrNoRetryClearSprite;    // リトライなし or 最短クリアでない
    public Sprite retryClearSprite;             // リトライあり

    // 最短クリア判定条件
    public int minStepsToClear = 10;       // 最短ステップ数
    public int minRetriesToClear = 2;      // リトライありと判定する回数

    public int stepsTaken = 0; // 現在のステップ数

    // ステップ数を加算する（呼び出し元で使用）
    public void AddStep()
    {
        stepsTaken++;
    }

    // クリア演出を実行
    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);
        AudioManager.Instance.PlaySE("Game clear");

        // ゲームクリア済みフラグを立てる → GameOverを防ぐ
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        // 判定に応じてスプライト変更
        if (retry == 0 && stepsTaken <= minStepsToClear)
        {
            // 最短＆ノーリトライ
            clearImage.sprite = noRetryFastClearSprite;
        }
        else if (retry >= minRetriesToClear || stepsTaken > minStepsToClear)
        {
            // リトライあり or 最短でない
            clearImage.sprite = retryClearSprite;
        }
        else
        {
            // ノーリトライだが最短でもない
            clearImage.sprite = retryOrNoRetryClearSprite;
        }
    }

    // リザルトシーンに移動
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
