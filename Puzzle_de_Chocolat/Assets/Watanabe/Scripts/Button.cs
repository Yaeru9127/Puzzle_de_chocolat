using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Retry�{�^�� �� �Q�[���v���C�V�[���ɑJ��
    public void GoToGameScene()
    {
        SceneManager.LoadScene("gauge"); // ���ۂ̃Q�[���V�[�����ɒu��������
    }

    // Stage�{�^�� �� �X�e�[�W�I����ʂɑJ��
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect"); // �X�e�[�W�I���V�[�����ɒu��������
    }

    // Title�{�^�� �� �^�C�g����ʂɑJ��
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title"); // �^�C�g���V�[�����ɒu��������
    }
}
