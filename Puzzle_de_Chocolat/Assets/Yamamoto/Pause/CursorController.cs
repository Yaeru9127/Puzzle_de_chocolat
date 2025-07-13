using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGUI;
using Cysharp.Threading.Tasks.Triggers;

public class CursorController : MonoBehaviour
{
    public static CursorController cc { get; private set; }

    private InputSystem_Manager manager;
    private InputSystem_Actions action;
    public InputAction input;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private GameObject cursorobj;
    public GameObject instance;
    private float speed;

    private GameObject currentUI;
    private RectTransform rect;
    private PointerEventData pointerData;
    private EventSystem eventSystem;

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
        currentUI = null;
        SetEventSystems();

        //�e�X�g
        ChangeCursorEnable(true);

        DeviceCheck();
    }

    /// <summary>
    /// �V�[�����[�h���Ɋe�ϐ���ݒ肷��֐�
    /// </summary>
    /// �V�[�����[�h�̂��тɖ���Ăяo��
    public void SetEventSystems()
    {
        eventSystem = EventSystem.current;
        pointerData = new PointerEventData(eventSystem);
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
                instance = Instantiate(cursorobj, Vector3.zero, Quaternion.identity);
            }
            else
            {
                instance.transform.position = Vector3.zero;
                instance.SetActive(true);
            }

            //Image�Ȃ̂�Canvas�̎q�I�u�W�F�N�g�ɐݒ�
            GameObject canvas = GameObject.Find("Canvas");
            instance.transform.SetParent(canvas.transform);

            //�ʒu�𒲐�
            SetCursor();

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
    /// GamePad���쎞�ɃJ�[�\���̐ݒ������֐�
    /// </summary>
    public void SetCursor()
    {
        instance.transform.SetAsLastSibling();
        instance.transform.position = Vector2.zero;
        instance.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary>
    /// �J�[�\���̃I���I�t��ݒ肷��֐�
    /// </summary>
    /// <param name="torf"></param> true�Ȃ�\���Afalse�Ȃ��\��
    public void ChangeCursorEnable(bool torf)
    {
        Cursor.visible = torf;
        if (instance != null)
        {
            instance.SetActive(torf);
            SetCursor();

        }
    }

    /// <summary>
    /// �J�[�\�����쎞�Ƀ{�^���A�j���[�V���������s����֐�
    /// </summary>
    /// <param name="hitUI"></param>
    private void CheckUI(GameObject hitUI)
    {
        GameObject obj = GetParentObject(hitUI);

        //���ݑI�����Ă���UI�ƑO�ɑI������UI���Ⴄ�Ȃ�
        if (obj != currentUI)
        {
            //�蓮��PointerEnter / PointerExit �𑗂�
            if (currentUI != null)
            {
                ExecuteEvents.Execute<IPointerExitHandler>(currentUI, pointerData, ExecuteEvents.pointerExitHandler);
            }

            if (obj != null)
            {
                ExecuteEvents.Execute<IPointerEnterHandler>(obj, pointerData, ExecuteEvents.pointerEnterHandler);
                EventSystem.current.SetSelectedGameObject(obj);
            }

            currentUI = obj;
        }
    }

    /// <summary>
    /// �e�I�u�W�F�N�g��Button�����邩�ǂ���
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private GameObject GetParentObject(GameObject obj)
    {
        if (obj == null) return null;

        Transform current = obj.transform;
        while (current != null)
        {
            if (current.GetComponent<Button>() != null || current.GetComponent<Slider>() != null)
            {
                return current.gameObject;
            }
            current = current.parent;
        }

        return null;
    }

    /// <summary>
    /// GamePad�����UI�𑀍삷��֐�
    /// </summary>
    private void GamePadClick(GameObject hitUI, float movex)
    {
        GameObject obj = GetParentObject (hitUI);
        if (obj == hitUI) return;

        //�{�^��
        if (hitUI.GetComponent<UnityEngine.UI.Button>() != null)
        {
            UnityEngine.UI.Button button = hitUI.GetComponent<UnityEngine.UI.Button>();
            ExecuteEvents.Execute<ISubmitHandler>(hitUI, pointerData, ExecuteEvents.submitHandler);
        }
        //�X���C�_�[
        if (hitUI.GetComponent<Slider>() != null)
        {
            if (Mathf.Abs(movex) > 0.1f)
            {
                Slider slider = hitUI.GetComponent<Slider>();
                slider.value += movex * Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instance == null) return;

        //���͂�ǂݍ���
        float avalue = action.GamePad.Click.ReadValue<float>();
        Vector2 read = action.GamePad.Point.ReadValue<Vector2>();

        Vector2 now = (Vector2)instance.transform.position;
        instance.transform.position = now + read * speed * Time.deltaTime;

        //UI�v�f��Raycast
        rect = instance.GetComponent<RectTransform>();
        pointerData.position = RectTransformUtility.WorldToScreenPoint(Camera.main, rect.position);
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        GameObject hitUI = null;
        if (raycastResults.Count > 0)
        {
            GameObject candidate = raycastResults[0].gameObject;
            if (candidate != instance) hitUI = candidate;
        }

        CheckUI(hitUI);

        if (avalue > 0.5f && hitUI != null) GamePadClick(hitUI, read.x);
    }
}
