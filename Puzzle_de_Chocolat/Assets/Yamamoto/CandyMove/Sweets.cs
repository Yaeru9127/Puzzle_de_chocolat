using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sweets : MonoBehaviour
{
    /*メモ
     *お菓子は親オブジェクトをSweetsParentに設定して生成する
     *製菓後のSpriteの大きさに注意*/

    private SweetsManager sm;
    private TileManager tm;

    //お菓子材料enum
    public enum Material
    {
        Butter,
        Sugar,
        Egg,
        Milk,
        None
    }
    public Material material;

    private string name;        //製菓後の名前変数
    public bool canEat;         //食べれるかの判定用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        sm = SweetsManager.sm;
        tm = TileManager.tm;
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
        //（材料が同じだったらfalseのままreturn）
        //（材料が違ったらレシピの名前にnameを変更）
        /*比較条件は今後のレシピの増減によって変わる*/
        switch (material)
        {
            //-----------------------------------------------------------------------
            //バター
            case Material.Butter:
                if (comparison.material == Material.Butter) return false;
                else if (comparison.material == Material.Sugar)name = "pretzel";
                else if (comparison.material == Material.Egg) name = "baumkuchen";
                else if (comparison.material == Material.Milk) name = "pannacotta";
                break;
            //-----------------------------------------------------------------------
            //砂糖
            case Material.Sugar:
                if (comparison.material == Material.Butter) name = "pretzel";
                else if (comparison.material == Material.Sugar) return false;
                else if (comparison.material == Material.Egg) name = "canulé";
                else if (comparison.material == Material.Milk) name = "tiramisu";
                break;
            //-----------------------------------------------------------------------
            //卵
            case Material.Egg:
                if (comparison.material == Material.Butter) name = "baumkuchen";
                else if (comparison.material == Material.Sugar) name = "canulé";
                else if (comparison.material == Material.Egg) return false;
                else if (comparison.material == Material.Milk) name = "macaroon";
                break;
            //-----------------------------------------------------------------------
            //牛乳
            case Material.Milk:
                if (comparison.material == Material.Butter) name = "pannacotta";
                else if (comparison.material == Material.Sugar) name = "tiramisu";
                else if (comparison.material == Material.Egg) name = "macaroon";
                else if (comparison.material == Material.Milk) return false;
                break;
        }

        //ここまでくるということは材料が違うものであるということ
        return true;
    }

    /// <summary>
    /// お菓子を作る関数
    /// </summary>
    /// <param name="comparison"></param> 移動先のお菓子のオブジェクト
    public void MakeSweets(GameObject comparison)
    {
        //製菓後のSpriteを取得
        Sprite changed = sm.GetMakedSprite(name);

        /*Spriteのnullチェック*/
        if (changed != null)
        {
            //Spriteの変更
            SpriteRenderer sr = comparison.GetComponent<SpriteRenderer>();
            sr.sprite = changed;

            //名前の初期化
            name = null;

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
            Debug.Log("delicious!!!");
            Destroy(this.gameObject);

            //お菓子の位置を更新
            sm.SearchSweets();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
