using System.Collections;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static ScreenController cc { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;

    /*残り工程数を管理するオブジェクトやscript変数を入れる*/

    /// <summary>
    /// クリア判定など
    /// </summary>
    [SerializeField] private GameObject clearJudg;      //ゴール判定オブジェクト
    [SerializeField] private GameObject clearImage;     //クリアパネル
    [SerializeField] private GameObject overImage;      //ゲームオーバーパネル
    [SerializeField] private GameObject pauseImage;     //ポーズパネル
    [SerializeField] private GameObject nextTextObject; //催促テキストオブジェクト
    private bool onPause;

    private void Awake()
    {
        if (cc == null) cc = this;
        else Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();

        //各パネルをオフ
        clearImage.SetActive(false);
        //overImage.SetActive(false);
        //pauseImage.SetActive(false);
        nextTextObject.SetActive(false);

        onPause = false;
    }

    /// <summary>
    /// ポーズ画面表示関数
    /// </summary>
    private void SetPause()
    {
        onPause = true;
        pauseImage.SetActive(true);

        //一時的にプレイヤー操作をオフにする
        manager.PlayerOff();

        //UI操作をオンにする
        manager.UIOn();
    }

    /// <summary>
    /// クリアをチェックする関数
    /// </summary>
    /// <param name="playerpos"></param> ゴールマスの座標
    public void ClearCheck(Vector2 playerpos)
    {
        /*残り工程数でクリア or ゲームオーバーを設定*/

        //↓クリア
        //ゴールのマスの座標とプレイヤーの座標が同じなら
        if ((Vector2)clearJudg.transform.position == playerpos)
        {
            //プレイヤー操作をオフ, UI操作をオン
            manager.PlayerOff();
            manager.UIOn();

            //クリア画面を表示
            clearImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //非ポーズ状態 && ポーズボタンが押されたら && ポーズパネルがオフ状態
        if (!onPause && actions.Player.Pause.WasPressedThisFrame() && !pauseImage.activeSelf)
        {
            SetPause();
        }
    }
}
