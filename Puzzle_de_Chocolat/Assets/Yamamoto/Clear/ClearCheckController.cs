using System.Collections;
using UnityEngine;

public class ClearCheckController : MonoBehaviour
{
    public static ClearCheckController cc { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;

    [SerializeField] private GameObject clearJudg;      //�S�[������I�u�W�F�N�g
    [SerializeField] private GameObject clearImage;          //�N���A�p�l��
    [SerializeField] private GameObject overImage;           //�Q�[���I�[�o�[�p�l��
    [SerializeField] private GameObject nextTextObject; //�Ñ��e�L�X�g�I�u�W�F�N�g

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
        
        clearImage.SetActive(false);
        //overImage.SetActive(false);
        nextTextObject.SetActive(false);
    }

    /// <summary>
    /// �N���A���`�F�b�N����֐�
    /// </summary>
    /// <param name="playerpos"></param>
    public void ClearCheck(Vector2 playerpos)
    {
        actions = manager.GetActions();
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


    private IEnumerator WaitDisplayCanNext()
    {
        //x�b�҂�
        yield return new WaitForSeconds(3);

        nextTextObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //�N���A��� or �Q�[���I�[�o�[��ʂ��\������Ă���
        //= �Q�[�����I�����Ă���
        //if (clearImage.GetComponent<GameObject>().activeSelf || overImage.GetComponent<GameObject>().activeSelf)
        //{
        //    Debug.Log("game is end");
        //}
    }
}
