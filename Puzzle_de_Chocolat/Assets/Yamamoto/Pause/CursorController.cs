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
        SetEventSystens();

        //テスト
        ChangeCursorEnable(true);

        DeviceCheck();
    }

    public void SetEventSystens()
    {
        eventSystem = EventSystem.current;
        pointerData = new PointerEventData(eventSystem);
    }

    /// <summary>
    /// コントローラーの接続を検知する関数
    /// </summary>
    private void DeviceCheck()
    {
        // コントローラーの接続を検知
        bool deviceCheck = Gamepad.all.Count > 0;

        //コントローラーが接続されていたら
        if (deviceCheck)
        {
            //GamePad操作をオン
            input = action.GamePad.Point;
            manager.GamePadOn();

            //マウス操作をオフ
            manager.MouseOff();

            //カーソルオブジェクトが生成されているかによって処理を変える
            if (instance == null)
            {
                instance = Instantiate(cursorobj, Vector3.zero, Quaternion.identity);
            }
            else
            {
                instance.transform.position = Vector3.zero;
                instance.SetActive(true);
            }

            //ImageなのでCanvasの子オブジェクトに設定
            GameObject canvas = GameObject.Find("Canvas");
            instance.transform.SetParent(canvas.transform);

            //位置を調整
            instance.transform.SetAsLastSibling();
            instance.transform.position = Vector2.zero;
            instance.GetComponent<Image>().raycastTarget = false;

            DontDestroyOnLoad(instance);
        }
        //コントローラーが接続されていなかったら
        else
        {
            //マウス操作をオン
            input = action.Mouse.Point;
            manager.MouseOn();

            //ゲームパッド操作をオフ
            manager.GamePadOff();

            //画像をカーソルの位置にセットする
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

            if (instance != null) instance.SetActive(false);
        }
    }

    /// <summary>
    /// カーソルのオンオフを設定する関数
    /// </summary>
    /// <param name="torf"></param> trueなら表示、falseなら非表示
    public void ChangeCursorEnable(bool torf)
    {
        Cursor.visible = torf;
        if (instance != null)
        {
            instance.SetActive(torf);
            instance.transform.SetAsLastSibling();
            instance.transform.position = Vector2.zero;
            instance.GetComponent<Image>().raycastTarget = false;

        }
    }

    private void CheckButton(GameObject hitUI)
    {
        //現在選択しているUIと前に選択したUIが違うなら
        if (hitUI != currentUI)
        {
            //手動でPointerEnter / PointerExit を送る
            if (currentUI != null)
            {
                ExecuteEvents.Execute<IPointerExitHandler>(currentUI, pointerData, ExecuteEvents.pointerExitHandler);
            }

            if (hitUI != null)
            {
                ExecuteEvents.Execute<IPointerEnterHandler>(hitUI, pointerData, ExecuteEvents.pointerEnterHandler);
                EventSystem.current.SetSelectedGameObject(hitUI);
            }

            currentUI = hitUI;
        }
    }

    /// <summary>
    /// GamePad操作でUIを操作する関数
    /// </summary>
    private void GamePadClick(GameObject hitUI, float movex)
    {
        //ボタン
        if (hitUI.GetComponent<UnityEngine.UI.Button>() != null)
        {
            UnityEngine.UI.Button button = hitUI.GetComponent<UnityEngine.UI.Button>();
            ExecuteEvents.Execute<ISubmitHandler>(hitUI, pointerData, ExecuteEvents.submitHandler);
        }
        //スライダー
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

        //入力を読み込む
        float avalue = action.GamePad.Click.ReadValue<float>();
        Vector2 read = action.GamePad.Point.ReadValue<Vector2>();

        Vector2 now = (Vector2)instance.transform.position;
        instance.transform.position = now + read * speed * Time.deltaTime;

        //UI要素にRaycast
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

        CheckButton(hitUI);

        if (avalue > 0.5f && hitUI != null) GamePadClick(hitUI, read.x);
    }
}
