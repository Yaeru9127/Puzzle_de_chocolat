using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController pause {  get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;

    [SerializeField] private GameObject pauseParent;   //�|�[�Y�p�l���I�u�W�F�N�g

    //��������I�u�W�F�N�g
    [SerializeField] private GameObject Keyboard;
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
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();
        //SetOperationObject();

        //�����\����ԂȂ��\���ɂ���
        if (pauseParent.activeSelf) pauseParent.SetActive(false);
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
            operation = Instantiate(GamePad);
        }
        else  //Keyboard
        {
            operation = Instantiate(Keyboard);
        }

        //�ꏊ��ݒ�
        operation.transform.position = new Vector3(6.45f, -2.5f, 0);
    }

    /// <summary>
    /// ���[�U�[���͂Ń|�[�Y�ɂ���֐�
    /// </summary>
    public void SetPause()
    {
        //�v���C���[������I�t
        manager.PlayerOff();

        //bool deviceCheck = Gamepad.all.Count > 0;
        //if (deviceCheck) manager.GamePadOn();
        //else manager.MouseOn();

        //�|�[�Y�p�l����\������
        pauseParent.SetActive(true);
    }

    /// <summary>
    /// OnClick.�|�[�Y��ʂ���Q�[���֖߂�
    /// </summary>
    public void ReturnGame()
    {
        //�|�[�Y�p�l�����\���ɂ���
        pauseParent.SetActive(false);

        //�v���C���[������I��
        manager.PlayerOn();
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
