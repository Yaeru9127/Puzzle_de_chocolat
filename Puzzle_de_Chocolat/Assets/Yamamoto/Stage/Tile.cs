using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Tile : MonoBehaviour
{
    //隣のマスとその"位置関係"のDictionary
    public Dictionary<GameObject, Vector2> neighbor = new Dictionary<GameObject, Vector2>();
    //位置関係のための配列
    private Vector2[] direction = new Vector2[]
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    [SerializeField] private Sprite hibi;   //ひびマスSprite
    public bool canBreak;                   //壊れるマス設定変数

    /// <summary>
    /// 隣接するマスの取得関数
    /// </summary>
    public void GetNeighborTiles()
    {
        //初期化
        neighbor.Clear();

        foreach (var dir in direction)
        {
            //当たり判定で取得
            Vector2 distance = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;

            //自身の場所 + SpriteRendererのサイズ * Vector2の上下左右方向 = 当たり判定ポイント
            Vector2 center = (Vector2)this.gameObject.transform.position + distance * dir;
            Collider2D[] hitobj = Physics2D.OverlapPointAll(center);

            foreach (var hitobj2 in hitobj)
            {
                //当たり判定のポイントにあるオブジェクトを格納
                if (hitobj2 != null && hitobj2.gameObject != this.gameObject && hitobj2.GetComponent<Tile>())
                {
                    //デバッグ
                    //Debug.Log($"{this.gameObject.name}, {hitobj.gameObject.name}");

                    //重複していなかったら追加
                    if (!neighbor.ContainsKey(hitobj2.gameObject)) neighbor.Add(hitobj2.gameObject, dir);
                }
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

    /// <summary>
    /// 次のマスを返す関数
    /// </summary>
    /// <param name="pos"></param> 位置関係の変数
    /// <returns></returns>
    public GameObject ReturnNextMass(Vector2 pos)
    {
        GameObject mass = null;
        
        //位置関係からオブジェクトを探す
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            if (pair.Key == null) continue;
            //位置関係が一致したときのオブジェクト
            if (pair.Key.GetComponent<Tile>() && pair.Value == pos && pair.Key != null)
            {
                //デバッグ
                //Debug.Log($"{this.gameObject.name} : {pair.Key.name} , {pair.Value}");
                mass = pair.Key;
                break;
            }
        }

        /*//デバッグ
        if (mass == null) Debug.Log("mass is null");
        else Debug.Log(mass.name);*/
        return mass;
    }

    /// <summary>
    /// マスのひび入れ、マスの削除関数
    /// </summary>
    public async UniTask ChangeSprite()
    {
        //壊れないマスならreturn
        if (!canBreak) return;

        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();

        //Spriteがひびマスでないなら、ひびマスに設定
        if (renderer.sprite != hibi && renderer != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = hibi;
            //Debug.Log("log");
        }
        //Spriteがひびマスなら
        else if (renderer.sprite == hibi)
        {
            Destroy(this.gameObject);
            await UniTask.NextFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
