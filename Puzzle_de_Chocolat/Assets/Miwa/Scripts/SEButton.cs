using UnityEngine;

public class SEButton : MonoBehaviour
{
    // Inspector�Ŋ��蓖�Ă�AudioClip�̖��O�Ɠ����ɂ���
    public string buttanClickSoundName = "ENTER"; 

    public void OnMyButtanClick()
    {
        // SE���Đ�����
        if (AudioManager.Instance != null)
        {
            // AudioManager��seClips�ɓo�^�������O��SE���Đ�
            AudioManager.Instance.PlaySE(buttanClickSoundName);
        }
    }
}