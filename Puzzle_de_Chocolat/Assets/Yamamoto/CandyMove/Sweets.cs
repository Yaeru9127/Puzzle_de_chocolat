using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Sweets : MonoBehaviour
{
    /*メモ
     *お菓子は親オブジェクトをSweetsParentに設定して生成する*/

    private SweetsManager sm;
    private TileManager tm;

    //お菓子材料enum
    public enum Material
    {
        Butter,
        Sugar,
        Egg,
        Milk
    }
    public Material material;

    public bool canmake;        //お菓子の作れるフラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        sm = SweetsManager.sm;
        tm = TileManager.tm;
        canmake = false;
    }

    /// <summary>
    /// お菓子を作れるかチェックする関数
    /// </summary>
    /// <param name="comparison"></param> 比較するスクリプト
    public bool TryMake(Sweets comparison)
    {
        Debug.Log("in TryMake");

        //材料を比較して作れるかどうかを決める
        //（材料が同じだったらfalseのままreturn）

        /*比較条件は今後のレシピの増減によって変わる*/
        //バター
        if (material == Material.Butter && comparison.material == Material.Butter) return false;
        //砂糖
        else if (material == Material.Sugar && comparison.material == Material.Sugar) return false;
        //卵
        else if (material == Material.Egg && comparison.material == Material.Egg) return false;
        //牛乳
        else if (material == Material.Milk && comparison.material == Material.Milk) return false;


        //ここまでくるということは材料が違うものであるということ
        return true;
    }

    /// <summary>
    /// お菓子を作る関数
    /// </summary>
    /// <param name="comparison"></param> 移動先のお菓子のオブジェクト
    public void MakeSweets(GameObject comparison)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canmake) return;

        /*お菓子の合体処理*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
