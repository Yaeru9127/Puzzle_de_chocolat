using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController pause {  get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    [SerializeField] private CursorController cc;

    [SerializeField] private GameObject pauseobj;   //�|�[�Y�p�l���I�u�W�F�N�g

    private void Awake()
    {
        if (pauseobj == null) pause = this;
        else if (pause != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();

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
        manager.UIOn();

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
        manager.UIOff();
        manager.PlayerOn();
        
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
