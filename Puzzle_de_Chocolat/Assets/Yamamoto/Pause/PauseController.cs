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

    //��������I�u�W�F�N�g
    [SerializeField] private GameObject Mouse;
    [SerializeField] private GameObject GamePad;

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
        SetOperationObject();

        //�����\����ԂȂ��\���ɂ���
        if (pauseobj.activeSelf) pauseobj.SetActive(false);
    }

    /// <summary>
    /// ������@�ɂ���ĕ\������I�u�W�F�N�g��ς���
    /// </summary>
    private void SetOperationObject()
    {
        //������@�ɂ���Ĕ���
        GameObject operation = null;
        if (Gamepad.all.Count > 0)  //GamePad
        {
            operation = Instantiate(GamePad,
            new Vector2(6.4f, -2.7f), Quaternion.identity);
        }
        else
        {
            operation = Instantiate(Mouse,
                new Vector2(6.4f, -2.7f), Quaternion.identity);
        }

        //�ꏊ��ݒ�
        operation.transform.position = new Vector3(6.45f, -2.5f, 0);
    }

    /// <summary>
    /// ���[�U�[���͂Ń|�[�Y�ɂ���֐�
    /// </summary>
    public void SetPause()
    {
        //�v���C���[������I�t�AUI������I��
        //manager.PlayerOff();

        //bool deviceCheck = Gamepad.all.Count > 0;
        //if (deviceCheck) manager.GamePadOn();
        //else manager.MouseOn();

        //�|�[�Y�p�l����\������
        pauseobj.SetActive(true);

        //�J�[�\���̕\���A����̃I���I�t
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
        //bool deviceCheck = Gamepad.all.Count > 0;
        //if (deviceCheck) manager.GamePadOff();
        //else manager.MouseOff();
        //manager.PlayerOn();
        
    }

    /// <summary>
    /// OnClick.�|�[�Y��ʂ���^�C�g���֖߂�
    /// </summary>
    public void ReturnTitle()
    {
        //�^�C�g���V�[����ǂݍ���
        SceneManager.LoadScene("TitleScene");
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
