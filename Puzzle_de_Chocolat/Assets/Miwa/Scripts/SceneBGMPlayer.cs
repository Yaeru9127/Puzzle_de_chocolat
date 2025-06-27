using UnityEngine;

public class SceneBGMPlayer : MonoBehaviour
{
    public int bgmClipIndex;

    public bool loopBGM = true;

     void Start()
    {
        //BGM���Đ�
        if(AudioManager.Instance !=null)
        {
            AudioManager.Instance.PlayBGM(bgmClipIndex, loopBGM);
        }
    }

    void OnDisable()
    {
        //�V�[���𗣂�鎞��BGM�̍Đ����~�߂܂�
        if(AudioManager.Instance !=null)
        {
            AudioManager.Instance.StopBGM();
        }
    }
}
