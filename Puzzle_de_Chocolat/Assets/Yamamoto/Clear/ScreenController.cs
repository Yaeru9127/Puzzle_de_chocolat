using System.Collections;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static ScreenController cc { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;

    /*�c��H�������Ǘ�����I�u�W�F�N�g��script�ϐ�������*/

    /// <summary>
    /// �N���A����Ȃ�
    /// </summary>
    [SerializeField] private GameObject clearJudg;      //�S�[������I�u�W�F�N�g
    [SerializeField] private GameObject clearImage;     //�N���A�p�l��
    [SerializeField] private GameObject overImage;      //�Q�[���I�[�o�[�p�l��
    [SerializeField] private GameObject pauseImage;     //�|�[�Y�p�l��
    [SerializeField] private GameObject nextTextObject; //�Ñ��e�L�X�g�I�u�W�F�N�g
    private bool onPause;

    private void Awake()
    {
        if (cc == null) cc = this;
        else Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();

        //�e�p�l�����I�t
        clearImage.SetActive(false);
        //overImage.SetActive(false);
        //pauseImage.SetActive(false);
        nextTextObject.SetActive(false);

        onPause = false;
    }

    /// <summary>
    /// �|�[�Y��ʕ\���֐�
    /// </summary>
    private void SetPause()
    {
        onPause = true;
        pauseImage.SetActive(true);

        //�ꎞ�I�Ƀv���C���[������I�t�ɂ���
        manager.PlayerOff();

        //UI������I���ɂ���
        manager.UIOn();
    }

    /// <summary>
    /// �N���A���`�F�b�N����֐�
    /// </summary>
    /// <param name="playerpos"></param> �S�[���}�X�̍��W
    public void ClearCheck(Vector2 playerpos)
    {
        /*�c��H�����ŃN���A or �Q�[���I�[�o�[��ݒ�*/

        //���N���A
        //�S�[���̃}�X�̍��W�ƃv���C���[�̍��W�������Ȃ�
        if ((Vector2)clearJudg.transform.position == playerpos)
        {
            //�v���C���[������I�t, UI������I��
            manager.PlayerOff();
            manager.UIOn();

            //�N���A��ʂ�\��
            clearImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //��|�[�Y��� && �|�[�Y�{�^���������ꂽ�� && �|�[�Y�p�l�����I�t���
        if (!onPause && actions.Player.Pause.WasPressedThisFrame() && !pauseImage.activeSelf)
        {
            SetPause();
        }
    }
}
