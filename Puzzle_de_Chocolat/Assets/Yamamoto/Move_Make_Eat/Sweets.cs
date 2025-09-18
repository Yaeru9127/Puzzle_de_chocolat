using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Sweets : MonoBehaviour
{
    /*メモ
     *お菓子は親オブジェクトをSweetsParentに設定して生成する*/

    private SweetsManager sm;
    private StageManager stage;
    private GameClear clear;

    //お菓子材料enum
    public enum Material
    {
        Butter,             //バター
        Sugar,              //砂糖
        Egg,                //卵
        Milk,               //牛乳
        Maked,              //合体後
        None                //その他
    }
    public Material material;

    public GameObject pair;     //2マスお菓子のペアオブジェクト変数
    private new string name;    //製菓後の名前変数
    public bool canMove;        //移動できるかの判定用
    public bool canEat;         //食べれるかの判定用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        sm = SweetsManager.sm;
        stage = StageManager.stage;
        clear = GameClear.clear;
        name = null;
        SetPosition();
    }

    /// <summary>
    /// お菓子を現在のマスの中心にセットする関数
    /// </summary>
    private void SetPosition()
    {
        Collider2D[] col = Physics2D.OverlapPointAll((Vector2)this.gameObject.transform.position);
        foreach (Collider2D col2 in col)
        {
            if (col2.gameObject.GetComponent<Tile>())
            {
                this.gameObject.transform.position = new Vector3(col2.gameObject.transform.position.x, col2.gameObject.transform.position.y, this.gameObject.transform.position.z);
            }
        }
    }

    /// <summary>
    /// お菓子を作れるかチェックする関数
    /// </summary>
    /// <param name="comparison"></param> 比較するスクリプト
    public bool TryMake(Sweets comparison)
    {
        //デバッグ
        /*Debug.Log("in TryMake");
        Debug.Log(material);
        Debug.Log(comparison.material);*/

        //初期化
        name = null;

        //材料を比較して作れるかどうかを決める
        //（作れる材料じゃなかったらfalseのままreturn）
        //（作れる材料だったらレシピの名前にnameを変更）
        /*比較条件は今後のレシピの増減によって変わる*/
        switch (material)
        {
            //-----------------------------------------------------------------------
            //バター
            case Material.Butter:
                if (stage.stagenum == 0)
                {
                    if (comparison.material == Material.Sugar) name = "scone";
                    else if (comparison.material == Material.Milk) name = "shortbread";
                    else return false;
                }
                else if (stage.stagenum == 1)
                {
                    if (comparison.material == Material.Sugar) name = "pretzel";
                    else if (comparison.material == Material.Egg) name = "baumkuchen";
                    else return false;
                }
                else if (stage.stagenum == 2)
                {
                    if (comparison.material == Material.Milk) name = "pannacotta";
                    else if (comparison.material == Material.Sugar) name = "maritozzo";
                    else return false;
                }
                else if (stage.stagenum == 3)
                {
                    if (comparison.material == Material.Egg) name = "madeleine";
                    else if (comparison.material == Material.Milk) name = "macaroon";
                    else return false;
                }
                break;
            //-----------------------------------------------------------------------
            //砂糖
            case Material.Sugar:
                if (stage.stagenum == 0)
                {
                    if (comparison.material == Material.Butter) name = "scone";
                    //else if (comparison.material == Material.Egg) name = "canulé";
                    else return false;
                }
                else if (stage.stagenum == 1)
                {
                    if (comparison.material == Material.Butter) name = "pretzel";
                    else return false;
                }
                else if (stage.stagenum == 2)
                {
                    if (comparison.material == Material.Milk) name = "tiramisu";
                    else if (comparison.material == Material.Butter) name = "maritozzo";
                    else return false;
                }
                else if (stage.stagenum == 3)
                {
                    if (comparison.material == Material.Egg) name = "canulé";
                    else return false;
                }
                break;
            //-----------------------------------------------------------------------
            //卵
            case Material.Egg:
                if (stage.stagenum == 0)
                {
                    if (comparison.material == Material.Sugar) name = "canulé";
                    else return false;
                }
                else if (stage.stagenum == 1)
                {
                    if (comparison.material == Material.Butter) name = "baumkuchen";
                    else return false;
                }
                else if (stage.stagenum == 2)
                {
                    return false;
                }
                else if (stage.stagenum == 3)
                {
                    if (comparison.material == Material.Butter) name = "madeleine";
                    else if (comparison.material == Material.Sugar) name = "canulé";
                    else return false;
                }
                break;
            //-----------------------------------------------------------------------
            //牛乳
            case Material.Milk:
                if (stage.stagenum == 0)
                {
                    if (comparison.material == Material.Butter) name = "shortbread";
                    else return false;
                }
                else if (stage.stagenum == 1)
                {
                    return false;
                }
                else if (stage.stagenum == 2)
                {
                    if (comparison.material == Material.Butter) name = "pannacotta";
                    else if (comparison.material == Material.Sugar) name = "tiramisu";
                    else return false;
                }
                else if (stage.stagenum == 3)
                {
                    if (comparison.material == Material.Butter) name = "macaroon";
                    else return false;
                }
                break;
            //-----------------------------------------------------------------------
            //それ以外
            default:
                return false;
        }
        //お菓子が作られた時だけ実行を追加
        if(name!=null)
        {
            if(GameClear.clear !=null)
            {
                //作ったお菓子の名前を伝える
                GameClear.clear.MadeSweets(name);
            }
        }
       
        //作れる材料ならここでreturn
        return true;
    }

    /// <summary>
    /// お菓子を作る関数
    /// </summary>
    /// <param name="comparison"></param> 移動先のお菓子のオブジェクト
    public async UniTask MakeSweets(GameObject comparison)
    {
        //製菓後のGameObjectを取得
        GameObject changed = sm.GetMakedSweets(name);

        /*製菓後のGameObjectのnullチェック*/
        if (changed != null)
        {
            //合体先のマスの上に製菓後のお菓子を配置
            Vector3 changedpos = this.gameObject.transform.position;
            GameObject inst = Instantiate(changed, changedpos, Quaternion.identity);
            inst.transform.SetParent(sm.gameObject.transform);

            //合体後のenumに変更する
            Sweets instscript = inst.GetComponent<Sweets>();
            instscript.material = Material.Maked;

            //名前の初期化
            name = null;

            //製菓前のオブジェクトを削除
            Destroy(comparison);
            if (pair != null) Destroy(pair);
            Destroy(this.gameObject);
        }
        //else Debug.Log("dont get sprite");

        await UniTask.Yield();
    }

    /// <summary>
    /// お菓子を食べる関数
    /// </summary>
    public async UniTask EatSweets()
    {
        //このお菓子が食べれたら
        if (canEat)
        {
            //Debug.Log("delicious!!!");

            //お菓子を削除
            if (pair != null) Destroy(pair);
            Destroy(this.gameObject);

            await UniTask.DelayFrame(1);

            //お菓子の位置を更新
            sm.SearchSweets();
            sm.SetEffect();

            //食べたフラグを更新
            clear.AddWasEat();
            //Debug.Log("食べた回数: " + clear.wasEat);
        }
        //else Debug.Log("this sweets is can not eat");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
