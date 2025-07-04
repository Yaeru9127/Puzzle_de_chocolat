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
    private GameObject instance;
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
        manager = InputSystem_Manager.manager;
        action = manager.GetActions();
        speed =5f;

        //テスト
        ChangeCursorEnable(true);

        DeviceCheck();

        DontDestroyOnLoad(this.gameObject);
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
                instance = Instantiate(cursorobj, Vector2.zero, Quaternion.identity);
            }
            else
            {
                instance.transform.position = Vector2.zero;
                instance.SetActive(true);
            }
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

    private void GamePadClick()
    {
        //選択しているUIを格納
        GameObject select = EventSystem.current.currentSelectedGameObject;

        //選択UIが存在しない || 選択UIがボタンではない => return
        if (select == null || select.GetComponent<ButtonController>() == null) return;

        //選択UIが存在する && 選択UIがボタンである
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

        //入力を読み込む
        Vector2 read = input.ReadValue<Vector2>();

        //カーソルオブジェクトが存在したらカーソルを動かす
        if (instance != null)
        {
            Vector2 now = instance.transform.position;
            instance.transform.position = now + read * speed * Time.deltaTime;

            GamePadClick();
        }
    }
}
