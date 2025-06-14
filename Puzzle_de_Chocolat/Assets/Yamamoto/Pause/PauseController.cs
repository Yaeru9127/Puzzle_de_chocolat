using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController pause {  get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    [SerializeField] private CursorController cc;

    [SerializeField] private GameObject pauseobj;   //ポーズパネルオブジェクト

    private void Awake()
    {
        if (pauseobj == null) pause = this;
        else if (pause != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();

        //もし表示状態なら非表示にする
        if (pauseobj.activeSelf) pauseobj.SetActive(false);
    }

    /// <summary>
    /// ユーザー入力でポーズにする関数
    /// </summary>
    public void SetPause()
    {
        //プレイヤー操作をオフ、UI操作をオン
        manager.PlayerOff();
        manager.UIOn();

        //ポーズパネルを表示する
        pauseobj.SetActive(true);

        //カーソルを表示
        cc.ChangeCursorEnable(true);
    }

    /// <summary>
    /// OnClick.ポーズ画面からゲームへ戻る
    /// </summary>
    public void ReturnGame()
    {
        //カーソルを非表示
        cc.ChangeCursorEnable(false);

        //ポーズパネルを非表示にする
        pauseobj.SetActive(false);

        //UI操作をオフ、プレイヤー操作をオン
        manager.UIOff();
        manager.PlayerOn();
        
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
