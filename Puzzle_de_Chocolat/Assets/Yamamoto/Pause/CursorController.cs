using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CursorController : MonoBehaviour
{
    public static CursorController cc { get; private set; }

    private InputSystem_Manager manager;
    private InputSystem_Actions action;

    public InputAction input;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private GameObject cursorobj;
    public GameObject instance;

    private float speed = 5f;
    private GameObject currentUI;
    private RectTransform rect;
    private PointerEventData pointerData;
    private EventSystem eventSystem;
    private InputAction east;

    private void Awake()
    {
        if (cc == null) cc = this;
        else if (cc != null) Destroy(this);

        DontDestroyOnLoad(this.gameObject);
        manager = InputSystem_Manager.manager;

        if (manager == null)
        {
            Debug.LogError("InputSystem_Manager が見つかりません。");
            return;
        }

        action = manager.GetActions();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void Start()
    {
        currentUI = null;
        east = action.GamePad.Click;
        SetEventSystems();
        DeviceCheck();
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SetEventSystems();
        DeviceCheck();
    }

    public void SetEventSystems()
    {
        eventSystem = EventSystem.current;
        pointerData = new PointerEventData(eventSystem);
    }

    private void DeviceCheck()
    {
        bool deviceCheck = Gamepad.all.Count > 0;

        if (deviceCheck)
        {
            manager.GamePadOn();
            manager.MouseOff();

            if (instance == null)
            {
                instance = Instantiate(cursorobj, Vector3.zero, Quaternion.identity);
                instance.transform.parent = GameObject.Find("Canvas").transform;
            }

            //AttachCursorToSceneCanvas();
            instance.SetActive(true);
            SetCursor();
        }
        else
        {
            manager.MouseOn();
            manager.GamePadOff();
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            ChangeCursorEnable(true);

            if (instance != null) instance.SetActive(false);
        }
    }

    public void SetCursor()
    {
        instance.transform.SetAsLastSibling();
        RectTransform rt = instance.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;

        instance.GetComponent<Image>().raycastTarget = false;
    }

    public void ChangeCursorEnable(bool torf)
    {
        Cursor.visible = torf;
        if (instance != null)
        {
            instance.SetActive(torf);
            SetCursor();
        }
    }

    private void CheckUI(GameObject hitUI)
    {
        GameObject obj = GetParentObject(hitUI);

        if (obj != currentUI)
        {
            if (currentUI != null)
                ExecuteEvents.Execute<IPointerExitHandler>(currentUI, pointerData, ExecuteEvents.pointerExitHandler);

            if (obj != null)
            {
                ExecuteEvents.Execute<IPointerEnterHandler>(obj, pointerData, ExecuteEvents.pointerEnterHandler);
                EventSystem.current.SetSelectedGameObject(obj);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            currentUI = obj;
        }
    }

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

    private void GamePadClick(GameObject hitUI, float movex, bool east)
    {
        GameObject obj = GetParentObject(hitUI);
        if (obj == hitUI) return;

        if (hitUI.GetComponent<Button>() != null)
        {
            ExecuteEvents.Execute<ISubmitHandler>(hitUI, pointerData, ExecuteEvents.submitHandler);
        }

        if (hitUI.GetComponent<Slider>() != null && east)
        {
            if (Mathf.Abs(movex) > 0.1f)
            {
                Slider slider = hitUI.GetComponent<Slider>();
                slider.value += movex * Time.deltaTime;
            }
        }
    }

    private void Update()
    {
        if (instance == null || !instance.activeSelf) return;

        float avalue = action.GamePad.Click.ReadValue<float>();
        bool isEastPressed = east != null && east.ReadValue<float>() > 0.5f;
        Vector2 read = action.GamePad.Point.ReadValue<Vector2>();

        Vector2 now = (Vector2)instance.transform.position;
        instance.transform.position = now + read * speed * Time.deltaTime;

        rect = instance.GetComponent<RectTransform>();

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rect.position);
        pointerData.position = screenPos;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        GameObject hitUI = null;
        if (raycastResults.Count > 0)
        {
            GameObject candidate = raycastResults[0].gameObject;
            if (candidate != instance) hitUI = candidate;
        }

        CheckUI(hitUI);

        if (avalue > 0.5f && hitUI != null)
        {
            GamePadClick(hitUI, read.x, isEastPressed);
        }
    }
}
