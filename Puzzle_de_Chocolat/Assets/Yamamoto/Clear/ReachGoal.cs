using UnityEngine;
using System.Collections.Generic;

public class ReachGoal : MonoBehaviour
{
    private TileManager tm;
    List<GameObject[][]> path = new List<GameObject[][]>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tm = TileManager.tm;
    }

    //private List<GameObject[][]> GetRowCol()
    //{
    //    List<GameObject[][]> returnlist = new List<GameObject[][]>();

    //    //‚½‚Ä
    //    int row;
    //    //‚æ‚±
    //    int col;

    //    foreach (KeyValuePair<GameObject, Vector2> pair in tm.tiles)
    //    {
            
    //    }

    //    return returnlist;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
