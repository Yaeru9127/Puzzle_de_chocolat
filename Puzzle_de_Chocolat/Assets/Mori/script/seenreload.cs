using UnityEngine;
using UnityEngine.SceneManagement;

public class scenereload : MonoBehaviour
{
    private const string ReloadCountKey = "SceneReloadCount";

    // Canvas内の確認ダイアログPanelを格納する変数
    public GameObject confirmationDialog;

    void Start()
    {
        // 現在のリロードカウントをログに出力（デバッグ用）
        int reloadCount = PlayerPrefs.GetInt(ReloadCountKey, 0);
        Debug.Log("現在のリロードカウント: " + reloadCount);

        // 初期状態では確認ダイアログを非表示に
        confirmationDialog.SetActive(false);
    }

    void Update()
    {
        // RキーまたはYボタンでリロードを確認
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3)) // Yボタン (ジョイスティック)
        {
            ShowConfirmationDialog();
        }

        // 確認ダイアログでのキー入力を処理
        HandleConfirmationInput();
    }

    // 確認ダイアログを表示する
    void ShowConfirmationDialog()
    {
        confirmationDialog.SetActive(true); // ダイアログを表示
        Debug.Log("リロードしますか？[Y] はい、[N] いいえ");
    }

    // 確認ダイアログでのユーザー入力を処理
    void HandleConfirmationInput()
    {
        // 「はい」を選択: Yキー または Aボタン
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton0)) // Aボタン (ジョイスティック)
        {
            OnConfirmReload();
        }

        // 「いいえ」を選択: Nキー または Bボタン
        if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.JoystickButton1)) // Bボタン (ジョイスティック)
        {
            OnCancelReload();
        }
    }

    // 確認ボタンが押されたときの処理（リロード実行）
    void OnConfirmReload()
    {
        confirmationDialog.SetActive(false); // ダイアログを非表示

        int reloadCount = PlayerPrefs.GetInt(ReloadCountKey, 0);
        reloadCount++;
        PlayerPrefs.SetInt(ReloadCountKey, reloadCount);
        PlayerPrefs.Save(); // 明示的に保存

        // シーンをリロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // キャンセルボタンが押されたときの処理（リロードをキャンセル）
    void OnCancelReload()
    {
        confirmationDialog.SetActive(false); // ダイアログを非表示
        Debug.Log("リロードがキャンセルされました");
    }

    // 終了時にリセットを行いたい場合
    void OnApplicationQuit()
    {
        // アプリケーション終了時にカウントをリセット
        PlayerPrefs.SetInt(ReloadCountKey, 0);
        PlayerPrefs.Save();
    }
}//ほかのコードのボタンに関与しないようにする