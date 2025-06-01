using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //隣のマスとその"位置関係"のDictionary
    private Dictionary<GameObject, Vector2> neighbor = new Dictionary<GameObject, Vector2>();
    private Vector2 tilepos;        //マスの座標
                                    //位置関係のための配列
    Vector2[] direction = new Vector2[]
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    private void Awake()
    {
        tilepos = this.gameObject.transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNeighborTiles();
    }

    //隣接するマスの取得関数
    private void GetNeighborTiles()
    {
        foreach (var dir in direction)
        {
            //当たり判定で取得
            Vector2 distance = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;

            //自身の場所 + SpriteRendererのサイズをかけた場所 * Vector2の上下左右方向 = 当たり判定ポイント
            Vector2 center = (Vector2)this.gameObject.transform.position + distance * dir;
            Collider2D hitobj = Physics2D.OverlapPoint(center);

            //当たり判定のポイントにあるオブジェクトを格納
            if (hitobj != null && hitobj.gameObject != this.gameObject)
            {
                //デバッグ
                //Debug.Log($"{this.gameObject.name}, {hitobj.gameObject.name}");

                //重複していなかったら追加
                if (!neighbor.ContainsKey(hitobj.gameObject)) neighbor.Add(hitobj.gameObject, dir);

            }
        }

        /*//デバッグ
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            Debug.Log($"{this.gameObject.name}: object => {pair.Key.name}, pos => {pair.Value}");
        }*/
    }

    /*//デバッグ（当たり判定描画関数）
    private void OnDrawGizmos()
    {
        Vector2 size = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.gameObject.transform.position, size);
    }*/

    //マスのPositionを返す関数
    public Vector2 GetTilePos()
    {
        return tilepos;
    }

    //次のマスを返す関数
    public GameObject ReturnNextMass(string direction)
    {
        Vector2 target = Vector2.zero;
        GameObject mass = null;

        //方向に変換
        switch (direction)
        {
            case "Up": target = Vector2.up; break;
            case "Down": target = Vector2.down; break;
            case "Left": target = Vector2.left; break;
            case "Right": target = Vector2.right; break;
            default: target = Vector2.zero; break;
        }

        //nullチェック
        if (target == Vector2.zero)
        {
            Debug.Log("direction is missing!!");
            return mass;
        }

        //方向と位置関係からオブジェクトを探す
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            if (pair.Value == target)
            {
                //デバッグ
                //Debug.Log($"{pair.Key.name} : {pair.Value}");
                mass = pair.Key;
                break;
            }
        }

        /*//デバッグ
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            Debug.Log($"{this.gameObject.name} => {pair.Key.name} : {pair.Value}");
        }
        if (mass == null) Debug.Log("mass is null");
        else Debug.Log(mass.name);*/
        return mass;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
