using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Windows;
using System;

public class CursorController : MonoBehaviour
{
    public static CursorController cc { get; private set; }

    private InputSystem_Manager manager;
    private InputSystem_Actions action;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private GameObject cursorobj;
    public GameObject instance;

    private float speed = 5f;
    private GameObject currentUI;
    private RectTransform rect;
    private PointerEventData pointerData;
    private EventSystem eventSystem;
    private float sliderCooldown = 0f;
    private float sliderCooldownDuration = 0.2f;

    private void Awake()
    {
        if (cc == null) cc = this;
        else if (cc != null) Destroy(this);

        DontDestroyOnLoad(this.gameObject);
        manager = InputSystem_Manager.manager;

        if (manager == null) return;

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
        manager.PlayerOff();
        currentUI = null;
        SetEventSystems();
        ChangeCursorEnable(true);
    }

    /// <summary>
    /// シーンロード時に実行する関数
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SetEventSystems();
        DeviceCheck();
        if (instance != null && instance.activeSelf) SetCursor();
    }

    /// <summary>
    /// GamePad操作時に必要なEventSystem関連の設定関数
    /// </summary>
    public void SetEventSystems()
    {
        eventSystem = EventSystem.current;
        pointerData = new PointerEventData(eventSystem);
    }

    /// <summary>
    /// コントローラーの有無によってカーソルオブジェクトの生成をする関数
    /// </summary>
    private void DeviceCheck()
    {
        bool deviceCheck = Gamepad.all.Count > 0;

        if (deviceCheck)    //GamePad操作設定
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                if (instance == null) instance = Instantiate(cursorobj, Vector3.zero, Quaternion.identity);
            }

            if (instance.transform.parent == null || instance.transform.parent != canvas.transform)
            {
                instance.transform.SetParent(canvas.transform, false);
            }

            if (EventSystem.current != null)
            {
                EventSystem.current.sendNavigationEvents = false;
            }
        }
        else    //Mouse操作設定
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

            if (instance != null) instance.SetActive(false);
        }
    }

    /// <summary>
    /// GamePadカーソルの初期位置を設定する関数
    /// </summary>
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

    /// <summary>
    /// カーソルの表示のオンオフと操作のオンオフを設定する関数
    /// </summary>
    /// <param name="torf"></param> GamePadカーソルのオンオフ
    public void ChangeCursorEnable(bool torf)
    {
        //UI操作をOnにする
        if (torf)
        {
            manager.PlayerOff();

            //GamePad
            if (Gamepad.all.Count > 0)
            {
                //マウスOff
                Cursor.visible = !torf;
                manager.MouseOff();

                //GamePad操作On
                manager.GamePadOn();
                GameObject canvas = GameObject.Find("Canvas");
                if (instance != null)
                {
                    instance.SetActive(torf);
                    SetCursor();
                }
                instance.transform.SetParent(canvas.transform, true);
            }
            //Mouse
            else if (Gamepad.all.Count <= 0)
            {
                //GamePadOff
                manager.GamePadOff();
                if (instance != null) instance.SetActive(false);

                //マウスOn
                Cursor.visible = torf;
            }
        }
        //UI操作をOffにする
        else if (!torf)
        {
            if (instance != null) instance.SetActive(torf);

            Cursor.visible = torf;
            manager.MouseOff();
            manager.GamePadOff();
            manager.PlayerOn();
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

    /// <summary>
    /// 選択UIの親オブジェクトを取得する関数
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

    private void GamePadClick(GameObject hitUI, float movex, bool isClick)
    {
        GameObject obj = GetParentObject(hitUI);
        if (obj == null) return;

        //Button
        if (obj.GetComponent<Button>() != null && isClick)
        {
            ExecuteEvents.Execute<ISubmitHandler>(obj, pointerData, ExecuteEvents.submitHandler);
            return;
        }

        //Slider
        if (obj.GetComponent<Slider>() != null)
        {

            if (Mathf.Abs(movex) < 0.2f) movex = 0f;

            if (isClick && Mathf.Abs(movex) > 0.2f && sliderCooldown <= 0f)
            {
                Slider slider = obj.GetComponent<Slider>();

                if (slider != null)
                {
                    slider.value += Mathf.Sign(movex) * 0.05f;
                    sliderCooldown = sliderCooldownDuration;
                }
            }

            return;
        }
    }

    private void Update()
    {
        //GamePad操作時以外は無視
        if (instance == null || !instance.activeSelf) return;

        //ユーザー入力を受け取る
        bool isClickHoled = action.GamePad.Click.IsPressed();
        Vector2 input = action.GamePad.Point.ReadValue<Vector2>();

        //カーソルの移動
        Vector2 now = (Vector2)instance.transform.position;
        instance.transform.position = now + input * speed * Time.deltaTime;

        //UIにRaycast
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
        if (sliderCooldown > 0f)
            sliderCooldown -= Time.deltaTime;

        if (hitUI != null && isClickHoled)
        {
            GamePadClick(hitUI, input.x, isClickHoled);
        }
    }

    public static implicit operator CursorController(CursorController1 v)
    {
        throw new NotImplementedException();
    }
}
