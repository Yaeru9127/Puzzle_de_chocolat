using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager tm {  get; private set; }

    //マスオブジェクト格納のDictionary(オブジェクト, "位置関係")
    public Dictionary<GameObject, Vector2> tiles = new Dictionary<GameObject, Vector2>();

    private void Awake()
    {
        if (tm == null) tm = this;
        else if (tm != null) Destroy(this.gameObject);
        GetAllMass();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// すべてのマスを取得する関数
    /// </summary>
    public void GetAllMass()
    {
        //初期化
        tiles.Clear();

        //自身の子オブジェクトの中からTileスクリプトを持つオブジェクトを探す
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<Tile>())
            {
                //マスリストに追加
                Tile tile = this.gameObject.transform.GetChild(i).GetComponent<Tile>(); ;
                tiles.Add(this.gameObject.transform.GetChild(i).gameObject, tile.gameObject.transform.position);

                //隣接マスを取得
                tile.GetNeighborTiles();
            }
        }

        ////デバッグ
        //foreach (KeyValuePair<GameObject, Vector2> dictionary in tiles)
        //{
        //    Debug.Log($"{dictionary.Key.name}, {dictionary.Value}");
        //}
    }

    /// <summary>
    /// 今いるマスを取得する関数
    /// </summary>
    /// <param name="obj"></param> 比較するオブジェクト
    public GameObject GetNowMass(GameObject obj)
    {
        //Debug.Log("GetNowMass() called");
        //Debug.Log(obj.name +" : " + obj.transform.position);
        GameObject nowmass = null;

        //オブジェクトの座標が一致しているか
        foreach (KeyValuePair<GameObject, Vector2> pair in tiles)
        {
            //Debug.Log(pair.Key.transform.position);
            if((Vector2)obj.transform.position == (Vector2)pair.Key.transform.position)
            {
                nowmass = pair.Key;
            }
        }

        return nowmass;
    }

    private void OnDestroy()
    {
        //シーンを跨ぐときにメモリから消す
        if (tm == this) tm = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
