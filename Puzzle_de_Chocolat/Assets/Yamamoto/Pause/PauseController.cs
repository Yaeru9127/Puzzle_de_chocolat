using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController pause {  get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    private CursorController cc;
    private ReloadCountManager rm;

    [SerializeField] private GameObject pauseobj;   //�|�[�Y�p�l���I�u�W�F�N�g

    private void Awake()
    {
        if (pause == null) pause = this;
        else if (pause != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        cc = CursorController.cc;
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();
        rm = ReloadCountManager.Instance;

        //�����\����ԂȂ��\���ɂ���
        if (pauseobj.activeSelf) pauseobj.SetActive(false);
    }

    /// <summary>
    /// ���[�U�[���͂Ń|�[�Y�ɂ���֐�
    /// </summary>
    public void SetPause()
    {
        //�v���C���[������I�t�AUI������I��
        manager.PlayerOff();

        bool deviceCheck = Gamepad.all.Count > 0;
        if (deviceCheck) manager.GamePadOn();
        else manager.MouseOn();

        //�|�[�Y�p�l����\������
        pauseobj.SetActive(true);

        //�J�[�\����\��
        cc.ChangeCursorEnable(true);
    }

    /// <summary>
    /// OnClick.�|�[�Y��ʂ���Q�[���֖߂�
    /// </summary>
    public void ReturnGame()
    {
        //�J�[�\�����\��
        cc.ChangeCursorEnable(false);

        //�|�[�Y�p�l�����\���ɂ���
        pauseobj.SetActive(false);

        //UI������I�t�A�v���C���[������I��
        bool deviceCheck = Gamepad.all.Count > 0;
        if (deviceCheck) manager.GamePadOff();
        else manager.MouseOff();
        manager.PlayerOn();
        
    }

    /// <summary>
    /// OnClick.�|�[�Y��ʂ���^�C�g���֖߂�
    /// </summary>
    public void ReturnTitle()
    {
        //�^�C�g���V�[����ǂݍ���
        //SceneManager.LoadScene("");
    }


    /// <summary>
    /// OnClick.�V�[���̃����[�h
    /// </summary>
    public void Retry()
    {
        //���݂̃V�[���̃C���f�b�N�X�i���o�[���擾���ă����[�h
        int nowsceneindex = SceneManager.GetActiveScene().buildIndex;
        rm.IncrementReloadCount();
        SceneManager.LoadScene(nowsceneindex);
    }

    private void OnDestroy()
    {
        //�V�[�����ׂ��Ƃ��Ƀ������������
        if (pause == this) pause = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
