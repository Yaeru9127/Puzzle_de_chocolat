using System.Collections.Generic;
using UnityEngine;

public class CanGoal : MonoBehaviour
{
    private TileManager tm;
    private SweetsManager sm;

    [SerializeField] private TestPlayer playerscript;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject check;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tm = TileManager.tm;
        sm = SweetsManager.sm;
    }

    /// <summary>
    /// ゴールできるかを判定する関数
    /// </summary>
    /// <param name="now"></param> 基準となるマスのスクリプト
    private void CanMassThrough(Tile now, GameObject test)
    {
        foreach (KeyValuePair<GameObject, Vector2> pair in tm.tiles)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        //テスト用
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject ob = Instantiate(check);
            CanMassThrough(playerscript.ReturnNowTileScript(), ob);
        }
    }
}
