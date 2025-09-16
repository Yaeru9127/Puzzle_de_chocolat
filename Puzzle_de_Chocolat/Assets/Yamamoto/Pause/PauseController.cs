using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController pause {  get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;

    [SerializeField] private GameObject pauseParent;   //ポーズパネルオブジェクト

    //操作説明オブジェクト
    [SerializeField] private GameObject Keyboard;
    [SerializeField] private GameObject GamePad;

    private void Awake()
    {
        if (pause == null) pause = this;
        else if (pause != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();
        //SetOperationObject();

        //もし表示状態なら非表示にする
        if (pauseParent.activeSelf) pauseParent.SetActive(false);
    }

    /// <summary>
    /// 操作方法によって表示するオブジェクトを変える
    /// </summary>
    private void SetOperationObject()
    {
        //操作方法によって判別
        GameObject operation = null;
        if (Gamepad.all.Count > 0)  //GamePad
        {
            operation = Instantiate(GamePad);
        }
        else  //Keyboard
        {
            operation = Instantiate(Keyboard);
        }

        //場所を設定
        operation.transform.position = new Vector3(6.45f, -2.5f, 0);
    }

    /// <summary>
    /// ユーザー入力でポーズにする関数
    /// </summary>
    public void SetPause()
    {
        //プレイヤー操作をオフ
        manager.PlayerOff();

        //bool deviceCheck = Gamepad.all.Count > 0;
        //if (deviceCheck) manager.GamePadOn();
        //else manager.MouseOn();

        //ポーズパネルを表示する
        pauseParent.SetActive(true);
    }

    /// <summary>
    /// OnClick.ポーズ画面からゲームへ戻る
    /// </summary>
    public void ReturnGame()
    {
        //ポーズパネルを非表示にする
        pauseParent.SetActive(false);

        //プレイヤー操作をオン
        manager.PlayerOn();
    }

    /// <summary>
    /// OnClick.ポーズ画面からタイトルへ戻る
    /// </summary>
    public void ReturnTitle()
    {
        //タイトルシーンを読み込む
        SceneManager.LoadScene("TitleScene");
    }


    /// <summary>
    /// OnClick.シーンのリロード
    /// </summary>
    public void Retry()
    {
        //現在のシーンのインデックスナンバーを取得してリロード
        int nowsceneindex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(nowsceneindex);
    }

    private void OnDestroy()
    {
        //シーンを跨ぐときにメモリから消す
        if (pause == this) pause = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
