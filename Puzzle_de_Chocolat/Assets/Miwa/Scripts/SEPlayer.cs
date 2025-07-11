using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ���ǉ��F�V�[���Ǘ��̂��߂̖��O���

public class SEPlayer : MonoBehaviour
{
    // Inspector�Ŋ��蓖�Ă�AudioClip�̖��O�Ɠ����ɂ���
    public string buttanClickSoundName = "ENTER";

    // ���ǉ��F�ڍs�������V�[���̖��O��Inspector�Őݒ�
    [Header("Scene Transition Settings")]
    public string targetSceneName;

    public void OnMyButtanClick()
    {
        // 1. SE���Đ�����
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySE(buttanClickSoundName);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance ��������܂���BSE�͍Đ�����܂���ł������A�V�[���ڍs�𑱍s���܂��B", this);
        }

        // 2. �V�[�����ڍs����
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("�ڍs����V�[�������ݒ肳��Ă��܂���BInspector�� 'Target Scene Name' ��ݒ肵�Ă��������B", this);
        }
    }
}

