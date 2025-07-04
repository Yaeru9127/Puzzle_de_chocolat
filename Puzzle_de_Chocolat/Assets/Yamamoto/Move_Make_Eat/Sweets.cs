using UnityEngine;

public class Sweets : MonoBehaviour
{
    /*メモ
     *お菓子は親オブジェクトをSweetsParentに設定して生成する*/

    private SweetsManager sm;

    //お菓子材料enum
    public enum Material
    {
        Butter,             //バター
        Sugar,              //砂糖
        Egg,                //卵
        Milk,               //牛乳
        Maked,              //合体後
        None                //作れない材料
    }
    public Material material;

    private new string name;    //製菓後の名前変数
    public bool canMove;        //移動できるかの判定用
    public bool canEat;         //食べれるかの判定用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        sm = SweetsManager.sm;
        name = null;
    }

    /// <summary>
    /// お菓子を作れるかチェックする関数
    /// </summary>
    /// <param name="comparison"></param> 比較するスクリプト
    public bool TryMake(Sweets comparison)
    {
        //Debug.Log("in TryMake");

        //材料を比較して作れるかどうかを決める
        //（作れる材料じゃなかったらfalseのままreturn）
        //（作れる材料だったらレシピの名前にnameを変更）
        /*比較条件は今後のレシピの増減によって変わる*/
        switch (material)
        {
            //-----------------------------------------------------------------------
            //バター
            case Material.Butter:
                if (comparison.material == Material.Sugar)name = "pretzel";
                else if (comparison.material == Material.Egg) name = "baumkuchen";
                else if (comparison.material == Material.Milk) name = "pannacotta";
                else return false;
                break;
            //-----------------------------------------------------------------------
            //砂糖
            case Material.Sugar:
                if (comparison.material == Material.Butter) name = "pretzel";
                else if (comparison.material == Material.Egg) name = "canulé";
                else if (comparison.material == Material.Milk) name = "tiramisu";
                else return false;
                break;
            //-----------------------------------------------------------------------
            //卵
            case Material.Egg:
                if (comparison.material == Material.Butter) name = "baumkuchen";
                else if (comparison.material == Material.Sugar) name = "canulé";
                else if (comparison.material == Material.Milk) name = "macaroon";
                else return false;
                break;
            //-----------------------------------------------------------------------
            //牛乳
            case Material.Milk:
                if (comparison.material == Material.Butter) name = "pannacotta";
                else if (comparison.material == Material.Sugar) name = "tiramisu";
                else if (comparison.material == Material.Egg) name = "macaroon";
                else return false;
                break;
        }

        //ここまでくるということは作れる材料であるということ
        return true;
    }

    /// <summary>
    /// お菓子を作る関数
    /// </summary>
    /// <param name="comparison"></param> 移動先のお菓子のオブジェクト
    public void MakeSweets(GameObject comparison)
    {
        //製菓後のGameObjectを取得
        GameObject changed = sm.GetMakedSweets(name);

        /*GameObjectのnullチェック*/
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

            //製菓前のふたつのオブジェクトを削除
            Destroy(comparison);
            Destroy(this.gameObject);
        }
        else Debug.Log("dont get sprite");
    }

    /// <summary>
    /// お菓子を食べる関数
    /// </summary>
    public void EatSweets()
    {
        //このお菓子が食べれたら
        if (canEat)
        {
            //Debug.Log("delicious!!!");
            Destroy(this.gameObject);

            //お菓子の位置を更新
            sm.SearchSweets();
        }
        else Debug.Log("this sweets is can not eat");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
