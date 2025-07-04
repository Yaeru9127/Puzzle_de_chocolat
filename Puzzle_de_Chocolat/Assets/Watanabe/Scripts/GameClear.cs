using UnityEngine;
using UnityEngine.UI;

public class GameClear
{
    public Image clearImage;  // ゲームクリア結果を表示する画像（UI）

    // それぞれのクリア状態に応じたスプライト
    private  Sprite noRetryFastClearSprite;       // ①リトライなし＆最短クリア
    private  Sprite retryOrNoRetryClearSprite;    // ②リトライあり＆最短 or リトライなし
    private  Sprite retryClearSprite;             // ③リトライあり

    public int minStepsToClear = 10;  // 最短クリアに必要な手順数
    public int minRetriesToClear = 2; // 最低リトライ回数（この数値以上ならリトライありになる）

    ///public int retryCount = 0;  // リトライ回数
    public int stepsTaken = 0;  // クリアまでにかかったステップ数

    // リトライ回数を1増やすメソッド
    //public void AddRetry()
    //{
    //    retryCount++;
    //}

    // ステップ数を1増やすメソッド
    public void AddStep()
    {
        stepsTaken++;
    }

    // ゲームクリア結果を表示するメソッド
    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);  // クリア結果画像を表示する

        // 最短クリアで、リトライなしの場合
        if (retry == 0 && stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = noRetryFastClearSprite;  // 最短クリアでリトライなしスプライトを設定
        }
        // リトライ回数が最低リトライ回数以上の場合、リトライあり
        else if (retry >= minRetriesToClear || stepsTaken > minStepsToClear)
        {
            clearImage.sprite = retryClearSprite;  // リトライありスプライトを設定
        }
        // 最短クリアではないがリトライがない場合
        else
        {
            clearImage.sprite = retryOrNoRetryClearSprite;  // リトライありまたは最短クリアではないスプライトを設定
        }
    }
}
