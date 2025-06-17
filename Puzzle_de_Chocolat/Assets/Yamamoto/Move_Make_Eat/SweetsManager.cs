using System.Collections.Generic;
using UnityEngine;

//製菓後のお菓子を格納するクラス
[System.Serializable]
public class MakedSweetsPair
{
    public GameObject makedSweets;
    public string makedName;
}

public class SweetsManager : MonoBehaviour
{
    public static SweetsManager sm { get; private set; }

    //お菓子オブジェクト格納のDictionary<座標, スクリプト>
    public Dictionary<Vector2, Sweets> sweets = new Dictionary<Vector2, Sweets>();

    //インスペクター設定用のList
    public List<MakedSweetsPair> mixtures = new List<MakedSweetsPair>();

    [SerializeField] private GaugeController gaugeCC;

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
        else if (sm != null) Destroy(this.gameObject);
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

        //座標で検索
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

    /// <summary>
    /// 製菓後のお菓子を取得する関数
    /// </summary>
    /// <param name="name"></param> 検索用の名前変数
    /// <returns></returns>
    public GameObject GetMakedSweets(string name)
    {
        GameObject returnobject = null;

        //名前で検索
        foreach (MakedSweetsPair pair in mixtures)
        {
            if (pair.makedName == name)
            {
                returnobject = pair.makedSweets;
            }
        }

        return returnobject;
    }

    /// <summary>
    /// 食料ゲージを減らす関数を呼ぶ関数
    /// </summary>
    /// スクリプトの設定の関係上、ここから呼び出す(仮)
    public void CallDecreaseFoodGauge()
    {
        gaugeCC.OnObjectDestroyed();
    }

    private void OnDestroy()
    {
        //シーンを跨ぐときにメモリから消す
        if (sm == this) sm = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
