using System.Collections;
using UnityEngine;

public class ClearCheckController : MonoBehaviour
{
    public static ClearCheckController cc { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;

    [SerializeField] private GameObject clearJudg;      //ゴール判定オブジェクト
    [SerializeField] private GameObject clearImage;          //クリアパネル
    [SerializeField] private GameObject overImage;           //ゲームオーバーパネル
    [SerializeField] private GameObject nextTextObject; //催促テキストオブジェクト

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
        
        clearImage.SetActive(false);
        //overImage.SetActive(false);
        nextTextObject.SetActive(false);
    }

    /// <summary>
    /// クリアをチェックする関数
    /// </summary>
    /// <param name="playerpos"></param>
    public void ClearCheck(Vector2 playerpos)
    {
        actions = manager.GetActions();
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


    private IEnumerator WaitDisplayCanNext()
    {
        //x秒待つ
        yield return new WaitForSeconds(3);

        nextTextObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //クリア画面 or ゲームオーバー画面が表示されている
        //= ゲームが終了している
        //if (clearImage.GetComponent<GameObject>().activeSelf || overImage.GetComponent<GameObject>().activeSelf)
        //{
        //    Debug.Log("game is end");
        //}
    }
}
