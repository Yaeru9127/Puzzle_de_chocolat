using UnityEngine;
using UnityEngine.SceneManagement;

public class scenereload : MonoBehaviour
{
    private const string ReloadCountKey = "SceneReloadCount";

    // Canvas���̊m�F�_�C�A���OPanel���i�[����ϐ�
    public GameObject confirmationDialog;

    void Start()
    {
        // ���݂̃����[�h�J�E���g�����O�ɏo�́i�f�o�b�O�p�j
        int reloadCount = PlayerPrefs.GetInt(ReloadCountKey, 0);
        Debug.Log("���݂̃����[�h�J�E���g: " + reloadCount);

        // ������Ԃł͊m�F�_�C�A���O���\����
        confirmationDialog.SetActive(false);
    }

    void Update()
    {
        // R�L�[�܂���Y�{�^���Ń����[�h���m�F
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3)) // Y�{�^�� (�W���C�X�e�B�b�N)
        {
            ShowConfirmationDialog();
        }

        // �m�F�_�C�A���O�ł̃L�[���͂�����
        HandleConfirmationInput();
    }

    // �m�F�_�C�A���O��\������
    void ShowConfirmationDialog()
    {
        confirmationDialog.SetActive(true); // �_�C�A���O��\��
        Debug.Log("�����[�h���܂����H[Y] �͂��A[N] ������");
    }

    // �m�F�_�C�A���O�ł̃��[�U�[���͂�����
    void HandleConfirmationInput()
    {
        // �u�͂��v��I��: Y�L�[ �܂��� A�{�^��
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton0)) // A�{�^�� (�W���C�X�e�B�b�N)
        {
            OnConfirmReload();
        }

        // �u�������v��I��: N�L�[ �܂��� B�{�^��
        if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.JoystickButton1)) // B�{�^�� (�W���C�X�e�B�b�N)
        {
            OnCancelReload();
        }
    }

    // �m�F�{�^���������ꂽ�Ƃ��̏����i�����[�h���s�j
    void OnConfirmReload()
    {
        confirmationDialog.SetActive(false); // �_�C�A���O���\��

        int reloadCount = PlayerPrefs.GetInt(ReloadCountKey, 0);
        reloadCount++;
        PlayerPrefs.SetInt(ReloadCountKey, reloadCount);
        PlayerPrefs.Save(); // �����I�ɕۑ�

        // �V�[���������[�h
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // �L�����Z���{�^���������ꂽ�Ƃ��̏����i�����[�h���L�����Z���j
    void OnCancelReload()
    {
        confirmationDialog.SetActive(false); // �_�C�A���O���\��
        Debug.Log("�����[�h���L�����Z������܂���");
    }

    // �I�����Ƀ��Z�b�g���s�������ꍇ
    void OnApplicationQuit()
    {
        // �A�v���P�[�V�����I�����ɃJ�E���g�����Z�b�g
        PlayerPrefs.SetInt(ReloadCountKey, 0);
        PlayerPrefs.Save();
    }
}//�ق��̃R�[�h�̃{�^���Ɋ֗^���Ȃ��悤�ɂ���