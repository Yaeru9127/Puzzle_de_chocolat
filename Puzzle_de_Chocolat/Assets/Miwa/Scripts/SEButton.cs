using UnityEngine;

public class SEButton : MonoBehaviour
{
    // Inspectorで割り当てるAudioClipの名前と同じにする
    public string buttanClickSoundName = "ENTER"; 

    public void OnMyButtanClick()
    {
        // SEを再生する
        if (AudioManager.Instance != null)
        {
            // AudioManagerのseClipsに登録した名前でSEを再生
            AudioManager.Instance.PlaySE(buttanClickSoundName);
        }
    }
}