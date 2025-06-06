using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Retryボタン → ゲームプレイシーンに遷移
    public void GoToGameScene()
    {
        SceneManager.LoadScene("gauge"); // 実際のゲームシーン名に置き換える
    }

    // Stageボタン → ステージ選択画面に遷移
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect"); // ステージ選択シーン名に置き換える
    }

    // Titleボタン → タイトル画面に遷移
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title"); // タイトルシーン名に置き換える
    }
}
