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
    private GameObject buttonObject;

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

        //テスト
        ChangeCursorEnable(true);

        DeviceCheck();
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
                instance = Instantiate(cursorobj, new Vector3(0, 0, -3), Quaternion.identity);
            }
            else
            {
                instance.transform.position = new Vector3(0, 0, -3);
                instance.SetActive(true);
            }

            //ImageなのでCanvasの子オブジェクトに設定
            GameObject canvas = GameObject.Find("Canvas");
            instance.transform.SetParent(canvas.transform);

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
    }

    /// <summary>
    /// GamePadでの操作時にボタンをクリックする関数
    /// </summary>
    private void GamePadButtonSelect()
    {
        //選択しているUIを格納
        GameObject select = EventSystem.current.currentSelectedGameObject;

        //選択UIが存在しない => return
        if (select == null)
        {
            buttonObject = null;
            return;
        }

        UnityEngine.UI.Button bt = select.GetComponent<UnityEngine.UI.Button>();

        //選択UIがボタンではない => return
        if (bt == null) return;

        //確認デバッグ
        Debug.Log(bt.name);

        //ボタンオブジェクトを格納
        buttonObject = bt.gameObject;
    }

    private void GamePadClick()
    {
        //選択するボタンがない => return
        if (buttonObject == null) return;

        UnityEngine.UI.Button button = buttonObject.GetComponent<UnityEngine.UI.Button>();
        
        //ボタンComponentがない => return
        if (button == null) return;

        //ボタンのOnClickを実行
        button.onClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null) return;

        //入力を読み込む
        Vector3 read = new Vector3(input.ReadValue<Vector2>().x, input.ReadValue<Vector2>().y, 0);

        //カーソルオブジェクトが存在したらカーソルを動かす
        if (instance != null)
        {
            Vector3 now = new Vector3(instance.transform.position.x, instance.transform.position.y, -7);
            instance.transform.position = now + read * speed * Time.deltaTime;

            GamePadButtonSelect();

            float avalue = action.GamePad.Click.ReadValue<float>();
            if (avalue > 0.5f) GamePadClick();
        }
    }
}
