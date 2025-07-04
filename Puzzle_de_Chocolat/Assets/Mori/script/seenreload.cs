using UnityEngine;
using UnityEngine.SceneManagement;

public class scenereload : MonoBehaviour
{
    public GameObject confirmationDialog;

    void Start()
    {
        confirmationDialog.SetActive(false);
        Debug.Log("現在のリロードカウント: " + ReloadCountManager.Instance.ReloadCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            ShowConfirmationDialog();
        }

        HandleConfirmationInput();
    }

    void ShowConfirmationDialog()
    {
        confirmationDialog.SetActive(true);
        Debug.Log("リロードしますか？[Y] はい、[N] いいえ");
    }

    void HandleConfirmationInput()
    {
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            OnConfirmReload();
        }

        if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            OnCancelReload();
        }
    }

    void OnConfirmReload()
    {
        confirmationDialog.SetActive(false);
        ReloadCountManager.Instance.IncrementReloadCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCancelReload()
    {
        confirmationDialog.SetActive(false);
        Debug.Log("リロードがキャンセルされました");
    }

    void OnApplicationQuit()
    {
        ReloadCountManager.Instance.ResetReloadCount();
    }
}
