using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ★追加：シーン管理のための名前空間

public class SEPlayer : MonoBehaviour
{
    // Inspectorで割り当てたAudioClipの名前と同じにする
    public string buttanClickSoundName = "ENTER";

    // ★追加：移行したいシーンの名前をInspectorで設定
    [Header("Scene Transition Settings")]
    public string targetSceneName;

    public void OnMyButtanClick()
    {
        // 1. SEを再生する
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySE(buttanClickSoundName);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance が見つかりません。SEは再生されませんでしたが、シーン移行を続行します。", this);
        }

        // 2. シーンを移行する
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("移行するシーン名が設定されていません。Inspectorで 'Target Scene Name' を設定してください。", this);
        }
    }
}

