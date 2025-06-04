using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

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
    private float speed;                //マス間の移動速度
    private bool onMove;
    private GameObject sweets;

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
        speed = 1f;
        onMove = false;
    }

    //現在地を取得する関数
    private void GetNowMass()
    {
        //当たり判定で取得
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(0.1f, 0.1f), 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject && collider.GetComponent<Tile>())
            {
                nowmass = collider.gameObject;
            }
        }
    }

    private Vector2 SetDirection(string directionstring)
    {
        Vector2 target = Vector2.zero;

        //方向に変換
        switch (directionstring)
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
        }

        return target;
    }

    /// <summary>
    /// 入力値から向きを算出する関数
    /// </summary>
    /// <param name="dir"></param>     入力値
    /// <param name="xbutton"></param> Xボタンを押しているか
    private void CheckDirection(Vector2 dir, bool xbutton)
    {
        onMove = true;

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

        //目の前のマスにお菓子があるか
        sweets = TileManager.tm.GetForwardMass(nowmass, SetDirection(direction.ToString()));
        if (sweets != null) sweets.transform.SetParent(this.gameObject.transform);
        Debug.Log(this.gameObject.transform.childCount);
        //Xを押していなかったらリセットする
        if (!xbutton)
        {
            if (sweets != null)
            {
                sweets.transform.SetParent(null);
                sweets = null;
            }
        }

        Tile tile = nowmass.GetComponent<Tile>();
        Vector2 di = SetDirection(direction.ToString());
        GameObject next = tile.ReturnNextMass(di);

        //子オブジェクトが存在しない && 目の前にお菓子がない
        if (this.gameObject.transform.childCount == 0 && sweets == null)
        {
            //次のマスがある = マスの外側に移動しようとしていない
            if (next != null)
            {
                MoveMass(next);
            }
            //マスの外側に移動しようとしている
            else
            {
                return;
            }
        }
        //目の前にお菓子がある
        else
        {
            //お菓子のその先のマスを取得
            Tile nexttile = next.GetComponent<Tile>();

            //もし先のマスが存在したら
            if (nexttile.ReturnNextMass(di) != null)
            {
                MoveMass(next);
            }
            //マスの外側に移動しようとしている
            else
            {
                //初期化する
                sweets.transform.SetParent(null);
                sweets = null;
                return;
            }
        }
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="next"></param>   次のマスオブジェクト
    private void MoveMass(GameObject next)
    {
        Vector3 pos = next.transform.position;
        pos.z = -5;

        this.gameObject.transform.DOMove(pos, speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            GetNowMass();
            onMove = false;
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
        float xvalue = actions.Player.CandyMove.ReadValue<float>();
        if (vec2 != Vector2.zero && !onMove)
        {
            CheckDirection(vec2, xvalue > 0.7f);
        }
    }
}
