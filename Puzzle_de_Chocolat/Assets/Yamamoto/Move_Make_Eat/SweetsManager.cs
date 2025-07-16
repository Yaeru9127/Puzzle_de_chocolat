using System.CodeDom.Compiler;
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

    public StageManager stage;

    //お菓子オブジェクト格納のList<Dictionary<座標, スクリプト>>
    public Dictionary<Vector2, Sweets> sweets = new Dictionary<Vector2, Sweets>();

    //エフェクトオブジェクト格納のList<Dictionary<座標, オブジェクト>>
    public Dictionary<Sweets, GameObject> effects = new Dictionary<Sweets, GameObject>();

    //インスペクター設定用のList
    public List<MakedSweetsPair> mixtures = new List<MakedSweetsPair>();

    [SerializeField] private GameObject zairyouEffect;
    [SerializeField] private GameObject eatEffect;

    [SerializeField] private GaugeController gaugeCC;

    /*レシピ　メモ
     *Stage1
     *プレッツェル : pretzel    バター + 砂糖
     *バームクーヘン : baumkuchen バター + 卵
     *
     *Stage2
     *パンナコッタ : pannacotta バター + 牛乳
     *ティラミス : tiramisu     砂糖 + 牛乳
     *マリトッツォ : maritozzo  バター + 砂糖
     *
     *else
     *カヌレ : canulé           砂糖 + 卵
     *マカロン : macaroon  バター + 牛乳
     *マドレーヌ : madeleine    バター + 卵
     *
     *Inspecterのstringには上記の名前で設定すること*/

    private void Awake()
    {
        //初期化
        if (sm == null) sm = this;
        else if (sm != null) Destroy(this.gameObject);

        stage = StageManager.stage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SearchSweets(true);
    }

    /// <summary>
    /// お菓子の配列の中から目的のお菓子を見つけ出す関数
    /// </summary>
    /// <param name="pos"></param> 探すマスの座標
    public Sweets GetSweets(Vector2 pos)
    {
        Sweets returnsweetts = null;;

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
    /// <param name="add"></param> Dictionaryに追加するフラグ
    /// <returns></returns>
    public void SearchSweets(bool add)
    {
        sweets.Clear();

        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            var child = this.gameObject.transform.GetChild(i);
            var sweetsComp = child.GetComponent<Sweets>();
            if (sweetsComp != null)
            {
                Vector2 posKey = (Vector2)child.position;

                if (!sweets.ContainsKey(posKey))
                {
                    sweets.Add(posKey, sweetsComp);
                }
                else
                {
                    sweets[posKey] = sweetsComp;
                }
            }
        }

        if (add)
        {
            foreach (var kvp in sweets)
            {
                var sweetsComp = kvp.Value;

                // すでにeffectsに登録されていれば生成しない
                if (!effects.ContainsKey(sweetsComp))
                {
                    GenerateEffect(kvp.Key, sweetsComp);
                }
            }
        }
    }

    /// <summary>
    /// sweetsの子オブジェクトにエフェクトが存在するかを判定する関数
    /// </summary>
    //private bool HasEffectChild(Sweets sweetsComp)
    //{
    //    foreach (Transform child in sweetsComp.transform)
    //    {
    //        if (child.GetComponent<EatEffect>() != null) return true;
    //        if (child.GetComponent<ZairyouEffect>() != null) return true;
    //    }
    //    return false;
    //}

    /// <summary>
    /// 指定した sweets に対応するエフェクトを生成し effects に登録する
    /// </summary>
    private void GenerateEffect(Vector2 posKey, Sweets sweetsComp)
    {
        GameObject effectObj = null;

        if (sweetsComp.material == Sweets.Material.Maked)
            effectObj = eatEffect;
        else
            effectObj = zairyouEffect;

        if (effectObj != null)
        {
            Vector3 generatePos = new Vector3(posKey.x, posKey.y, 0);
            GameObject obj = Instantiate(effectObj, generatePos, Quaternion.identity);

            obj.transform.SetParent(sweetsComp.transform, true);
            obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            // sweetsをキーにエフェクトを管理
            effects[sweetsComp] = obj;
        }
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
        gaugeCC.OnObjectDestroyed(1);
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
