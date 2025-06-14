using System.Collections.Generic;
using UnityEngine;

public class CanGoal : MonoBehaviour
{
    private TileManager tm;
    private SweetsManager sm;

    [SerializeField] private TestPlayer playerscript;
    [SerializeField] private GameObject goal;
    [SerializeField] private List<GameObject> searched = new List<GameObject>();

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
    /// お菓子を移動させて残り移動数が0になったときにこの関数を実行する
    private bool CanMassThrough(Tile now)
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

    // Update is called once per frame
    void Update()
    {
        //テスト用
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log($"serach is end : {CanMassThrough(playerscript.ReturnNowTileScript())}");
        }
    }
}
