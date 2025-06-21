using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanGoal : MonoBehaviour
{
    public static CanGoal cg {  get; private set; }

    private TileManager tm;
    private SweetsManager sm;

    [SerializeField] private TestPlayer playerscript;   //プレイヤー変数
    [SerializeField] private GameObject goal;           //ゴールマスオブジェクト

    //ゴールできるかの判定時に使う検索済み格納関数
    private List<GameObject> searched = new List<GameObject>();

    private void Awake()
    {
        if (cg == null) cg = this;
        else if (cg != null) Destroy(cg);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        tm = TileManager.tm;
        sm = SweetsManager.sm;
        searched.Clear();
    }

    /// <summary>
    /// ゴールできるかを判定する関数
    /// </summary>
    /// <param name="now"></param> 基準となるマスのスクリプト
    /// 残り移動数が0になったときにこの関数を実行する
    /// true = ゴールに到達できる  false = ゴールに到達できない
    public bool CanMassThrough(Tile now)
    {
        //検索済みリストに重複があったらreturn
        if (searched.Contains(now.gameObject)) return false;

        //リストに追加
        searched.Add(now.gameObject);

        //基準マスの隣接マスを検索
        foreach (KeyValuePair<GameObject, Vector2> pair in now.neighbor)
        {
            //隣接マスにお菓子があったら次のマスへ
            Sweets sweets = sm.GetSweets(pair.Key.transform.position);
            if (sweets != null) continue;

            //お菓子のない隣接マスがゴールマスなら
            if (pair.Key == goal)
            {
                //デバッグ
                //Debug.Log($"serachmass : {now.gameObject.name}  goal : {pair.Key.name}");

                return true;
            }

            //隣接マスの隣接マスを検索
            if (CanMassThrough(pair.Key.GetComponent<Tile>())) return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        //シーンを跨ぐときにメモリから消す
        if (cg == this) cg = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
