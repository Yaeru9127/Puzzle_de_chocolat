using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager tm {  get; private set; }
    public Dictionary<GameObject, Vector2> tiles = new Dictionary<GameObject, Vector2>();


    private void Awake()
    {
        if (tm == null)
        {
            tm = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        GetAllMass();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    /// <summary>
    /// 目の前のマスにあるお菓子の有無を取得する関数
    /// </summary>
    /// <param name="center"></param>    探すマスのオブジェクト
    /// <returns></returns>
    public GameObject SearchSweets(GameObject center)
    {
        GameObject sweets = null;

        //マスの座標に当たり判定を設置
        Collider2D[] hits = Physics2D.OverlapPointAll((Vector2)center.transform.position);
        foreach (Collider2D hitobj in hits)
        {
            //お菓子オブジェクトがあったら
            if (hitobj.gameObject.GetComponent<Sweets>())
            {
                sweets = hitobj.gameObject;
                break;
            }
        }

        /*//デバッグ
        if (sweets == null) Debug.Log("sweets is null");
        else Debug.Log("sweets is not null");*/

        return sweets;
    }

    //すべてのマスを取得する関数
    public void GetAllMass()
    {
        //初期化
        tiles.Clear();

        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            Tile tile = this.gameObject.transform.GetChild(i).GetComponent<Tile>(); ;
            tiles.Add(this.gameObject.transform.GetChild(i).gameObject, tile.GetTilePos());
        }

        /*//デバッグ用
        foreach (KeyValuePair<GameObject, Vector2> dictionary in tiles)
        {
            Debug.Log($"{dictionary.Key.name}, {dictionary.Value}");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
