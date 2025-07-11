using UnityEngine;

public class SEButton : MonoBehaviour
{
    // Inspector‚ÅŠ„‚è“–‚Ä‚éAudioClip‚Ì–¼‘O‚Æ“¯‚¶‚É‚·‚é
    public string buttanClickSoundName = "ENTER"; 

    public void OnMyButtanClick()
    {
        // SE‚ğÄ¶‚·‚é
        if (AudioManager.Instance != null)
        {
            // AudioManager‚ÌseClips‚É“o˜^‚µ‚½–¼‘O‚ÅSE‚ğÄ¶
            AudioManager.Instance.PlaySE(buttanClickSoundName);
        }
    }
}