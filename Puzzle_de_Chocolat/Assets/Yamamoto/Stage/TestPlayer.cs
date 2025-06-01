using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
//using Cysharp.Threading.Tasks;
using DG.Tweening;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;

    //プレイヤーが向いている向き
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    public GameObject nowmass;          //今いるマス

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomMassSet();
        GetNowMass();

        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();
        actions = inputmanager.GetActions();
        inputmanager.PlayerOn();
    }

    //現在地を取得する関数
    private void GetNowMass()
    {
        //当たり判定で取得
        Vector2 pos = this.transform.position;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(0.1f, 0.1f), 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject && collider.GetComponent<Tile>())
            {
                nowmass = collider.gameObject;
            }
        }
    }

    //入力値から向きを算出する関数
    private void CheckDirection(Vector2 dir)
    {
        //左右の入力方向が同じ(たぶんないと思うけど)
        if (Mathf.Abs(dir.x) == Mathf.Abs(dir.y))
        {
            //yの入力値で判断
            if ((dir.x > 0 && dir.y > 0) || (dir.x < 0 && dir.y > 0)) direction = Direction.Up;
            else direction = Direction.Down;
        }
        //xの方がyより大きい
        else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            //xの値が大きい => 右
            if (dir.x > 0) direction = Direction.Right;

            //xの値が小さい => 左
            else direction = Direction.Left;
        }
        //yの方がxより大きい
        else
        {
            //yの値が大きい => 上
            if (dir.y > 0) direction = Direction.Up;

            //yの値が小さい => 下
            else direction = Direction.Down;
        }

        //デバッグ
        //Debug.Log(direction);

        GetNextMass();
    }

    //次のマスを取得する関数
    private void GetNextMass()
    {
        Tile tilescript = nowmass.GetComponent<Tile>();
        MoveMass(tilescript.ReturnNextMass(direction.ToString()));
    }

    //移動関数（UniTaskで処理したい）
    private void MoveMass(GameObject next)
    {
        Vector3 pos = next.transform.position;
        pos.z = -5;

        this.gameObject.transform.DOMove(pos, 1.5f).OnComplete ( () =>
        {
            GetNowMass();
        });
        
    }

    //テスト
    private void RandomMassSet()
    {
        int num = Random.Range(0, TileManager.tm.tiles.Count);
        KeyValuePair<GameObject, Vector2> pair = TileManager.tm.tiles.ElementAt(num);
        this.gameObject.transform.position = new Vector3(pair.Value.x, pair.Value.y, -5);
        nowmass = pair.Key;
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();
        if (vec2 != Vector2.zero)
        {
            CheckDirection(vec2);
        }

        /*//デバッグ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMove = true;
            GetNextMass();
        }*/
    }
}
