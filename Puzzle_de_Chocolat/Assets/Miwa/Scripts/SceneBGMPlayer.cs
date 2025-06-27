using UnityEngine;

public class SceneBGMPlayer : MonoBehaviour
{
    public int bgmClipIndex;

    public bool loopBGM = true;

     void Start()
    {
        //BGM‚ğÄ¶
        if(AudioManager.Instance !=null)
        {
            AudioManager.Instance.PlayBGM(bgmClipIndex, loopBGM);
        }
    }

    void OnDisable()
    {
        //ƒV[ƒ“‚ğ—£‚ê‚é‚ÉBGM‚ÌÄ¶‚ğ~‚ß‚Ü‚·
        if(AudioManager.Instance !=null)
        {
            AudioManager.Instance.StopBGM();
        }
    }
}
