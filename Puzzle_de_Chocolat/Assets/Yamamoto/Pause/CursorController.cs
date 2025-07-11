using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public static CursorController cc { get; private set; }

    private InputSystem_Manager manager;
    private InputSystem_Actions action;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private GameObject cursorobj;
    public GameObject instance;
    public InputAction input;
    private float speed;

    private void Awake()
    {
        if (cc == null) cc = this;
        else if (cc != null) Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        manager = InputSystem_Manager.manager;
        action = manager.GetActions();
        speed =5f;

        //�e�X�g
        ChangeCursorEnable(true);

        DeviceCheck();
    }

    /// <summary>
    /// �R���g���[���[�̐ڑ������m����֐�
    /// </summary>
    private void DeviceCheck()
    {
        // �R���g���[���[�̐ڑ������m
        bool deviceCheck = Gamepad.all.Count > 0;

        //�R���g���[���[���ڑ�����Ă�����
        if (deviceCheck)
        {
            //GamePad������I��
            input = action.GamePad.Point;
            manager.GamePadOn();

            //�}�E�X������I�t
            manager.MouseOff();

            //�J�[�\���I�u�W�F�N�g����������Ă��邩�ɂ���ď�����ς���
            if (instance == null)
            {
                instance = Instantiate(cursorobj, new Vector3(0, 0, -7), Quaternion.identity);
            }
            else
            {
                instance.transform.position = new Vector3(0, 0, -7);
                instance.SetActive(true);
            }

            DontDestroyOnLoad(instance);
        }
        //�R���g���[���[���ڑ�����Ă��Ȃ�������
        else
        {
            //�}�E�X������I��
            input = action.Mouse.Point;
            manager.MouseOn();

            //�Q�[���p�b�h������I�t
            manager.GamePadOff();

            //�摜���J�[�\���̈ʒu�ɃZ�b�g����
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

            if (instance != null) instance.SetActive(false);
        }
    }

    /// <summary>
    /// �J�[�\���̃I���I�t��ݒ肷��֐�
    /// </summary>
    /// <param name="torf"></param> true�Ȃ�\���Afalse�Ȃ��\��
    public void ChangeCursorEnable(bool torf)
    {
        Cursor.visible = torf;
    }

    private void GamePadClick()
    {
        //�I�����Ă���UI���i�[
        GameObject select = EventSystem.current.currentSelectedGameObject;

        //�I��UI�����݂��Ȃ� || �I��UI���{�^���ł͂Ȃ� => return
        if (select == null || select.GetComponent<ButtonController>() == null) return;

        //�I��UI�����݂��� && �I��UI���{�^���ł���
        if (select != null && select.GetComponent<ButtonController>() != null)
        {
            UnityEngine.UI.Button button = select.GetComponent<UnityEngine.UI.Button>();

            if (button != null)
            {
                Debug.Log(button.gameObject.name);
                button.onClick.Invoke();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null) return;

        //���͂�ǂݍ���
        Vector3 read = new Vector3(input.ReadValue<Vector2>().x, input.ReadValue<Vector2>().y, 0);

        //�J�[�\���I�u�W�F�N�g�����݂�����J�[�\���𓮂���
        if (instance != null)
        {
            Vector3 now = new Vector3(instance.transform.position.x, instance.transform.position.y, -7);
            instance.transform.position = now + read * speed * Time.deltaTime;

            GamePadClick();
        }
    }
}
