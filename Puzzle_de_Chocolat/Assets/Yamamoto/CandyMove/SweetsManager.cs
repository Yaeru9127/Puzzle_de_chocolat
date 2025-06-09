using System.Collections.Generic;
using UnityEngine;

//合体後のお菓子を格納するクラス
[System.Serializable]
public class SpriteStringPair
{
    public Sprite makedSprite;
    public string makedName;
}

public class SweetsManager : MonoBehaviour
{
    public static SweetsManager sm { get; private set; }

    //お菓子オブジェクト格納のDictionary<座標, スクリプト>
    public Dictionary<Vector2, Sweets> sweets = new Dictionary<Vector2, Sweets>();

    //インスペクター設定用のList
    public List<SpriteStringPair> mixtures = new List<SpriteStringPair>();

    /*レシピ　メモ
     *プレッツェル : pretzel  バター + 砂糖
     *バームクーヘン : baumkuchen  卵 + バター
     *ティラミス : tiramisu  牛乳 + 砂糖
     *パンナコッタ : pannacotta  牛乳 + バター
     *マカロン : macaroon  卵 + 牛乳
     *カヌレ : canulé  卵 + 砂糖
     *Inspecterのstringには上記の名前で設定すること*/

    private void Awake()
    {
        //初期化
        if (sm == null) sm = this;
        else Destroy(sm);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SearchSweets();
    }

    /// <summary>
    /// お菓子の配列の中から目的のお菓子を見つけ出す関数
    /// </summary>
    /// <param name="pos"></param> 探すマスの座標
    public Sweets GetSweets(Vector2 pos)
    {
        Sweets returnsweetts = null;
        foreach (Vector2 sweetspos in sweets.Keys)
        {
            if (pos == sweetspos) returnsweetts = sweets[sweetspos];
        }

        return returnsweetts;
    }

    /// <summary>
    /// マス上のすべてのお菓子を取得する関数
    /// </summary>
    /// <returns></returns>
    public void SearchSweets()
    {
        sweets.Clear();

        //自身の子オブジェクトの中からSweetsスクリプトを持つオブジェクトを探す
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<Sweets>())
            {
                sweets.Add(this.gameObject.transform.GetChild(i).gameObject.transform.position, this.gameObject.transform.GetChild(i).gameObject.GetComponent<Sweets>());
            }
        }

        /*//デバッグ
        foreach (var sw in sweets)
        {
            Debug.Log($"Key : {sw.Key} , Value : {sw.Value.gameObject.name}");
        }*/
    }

    public Sprite GetMakedSprite(string name)
    {
        Sprite returnsprite = null;

        //名前で検索
        foreach (SpriteStringPair pair in mixtures)
        {
            if (pair.makedName == name)
            {
                returnsprite = pair.makedSprite;
            }
        }

        return returnsprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
