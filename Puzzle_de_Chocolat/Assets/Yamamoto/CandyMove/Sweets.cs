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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sm = SweetsManager.sm;
        tm = TileManager.tm;
    }

    /// <summary>
    /// お菓子を作れるかチェックする関数
    /// </summary>
    public GameObject TryMake()
    {
        Debug.Log("in TryMake");
        Sweets sweets = null;   //移動先のお菓子のスクリプト
        GameObject movedsweets = null;

        GameObject now = tm.GetNowMass(this.gameObject);    //今いるマスを探す

        //移動先のマスに別のお菓子があるか調べる
        foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
        {
            //アタッチされているスクリプトが違う && 座標が同じ
            if ((this.gameObject.GetComponent<Sweets>() != pair.Value) && ((Vector2)this.transform.position == pair.Key))
            {
                sweets = pair.Value;
                movedsweets = sweets.gameObject;
            }
        }

        //移動先のマスにお菓子がない場合
        if (sweets == null)
        {
            return movedsweets;
        }
        //移動先のマスにお菓子がある場合
        else
        {
            //材料を比較して作れるかどうかを決める
            /*比較条件は今後のレシピの増減によって変わる*/
            switch (material)
            {
                case Material.Butter:
                    if (sweets.material == Material.Butter)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
                case Material.Sugar:
                    if (sweets.material == Material.Sugar)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
                case Material.Egg:
                    if (sweets.material == Material.Egg)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
                case Material.Milk:
                    if (sweets.material == Material.Milk)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
            }
        }

        return movedsweets;
    }

    /// <summary>
    /// お菓子を作る関数
    /// </summary>
    /// <param name="comparison"></param> 移動先のお菓子のオブジェクト
    public void MakeSweets(GameObject comparison)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
