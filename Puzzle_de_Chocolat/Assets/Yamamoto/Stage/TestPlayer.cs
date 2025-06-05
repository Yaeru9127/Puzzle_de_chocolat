using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private TileManager tm = TileManager.tm;

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
        GetNowMass();

        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();
        actions = inputmanager.GetActions();
        inputmanager.PlayerOn();
        speed = 0.5f;
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

    /// <summary>
    /// 方向設定関数
    /// </summary>
    /// <param name="directionstring"></param> enumをstringに変える
    /// <returns></returns>
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
    private void CheckDirection(Vector2 dir, float xbutton)
    {
        if (!onMove) onMove = true;
        Vector2 directo = Vector2.zero;
        //左右の入力方向が同じ(たぶんないと思うけど)
        if (Mathf.Abs(dir.x) == Mathf.Abs(dir.y))
        {
            //yの入力値で判断
            if ((dir.x > 0 && dir.y > 0) || (dir.x < 0 && dir.y > 0)) directo = Vector2.up;
            else directo = Vector2.down;
        }
        //xの方がyより大きい
        else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            //xの値が大きい => 右
            if (dir.x > 0) directo = Vector2.right;

            //xの値が小さい => 左
            else directo = Vector2.left;
        }
        //yの方がxより大きい
        else
        {
            //yの値が大きい => 上
            if (dir.y > 0) directo = Vector2.up;

            //yの値が小さい => 下
            else directo = Vector2.down;
        }

        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttileobj;
        nexttileobj = nowtile.ReturnNextMass(directo);
        TryMove(nexttileobj, xbutton);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param> 移動先のマスの座標
    /// <param name="X"></param>    X or Shiftのボタンの押している値
    private void TryMove(GameObject next, float X)
    {
        //次のマスにあるお菓子を取得
        GameObject sweets = tm.GetSweets(next.transform.position).gameObject;

        //次のマスにお菓子があったら
        if (sweets != null)
        {
            //X or Shiftを押していたら
            if (X > 0.5f)
            {
                sweets.transform.SetParent(this.gameObject.transform);
            }
            //X or Shiftを押していなかったら
            else
            {
                onMove = false;
                return;
            }
        }

        MoveMass(next);
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="next"></param>   移動先のマスオブジェクト
    private void MoveMass(GameObject next)
    {
        Vector3 pos = next.transform.position;
        pos.z = -5;

        this.gameObject.transform.DOMove(pos, speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            //現在地を更新
            GetNowMass();

            //移動フラグ更新
            onMove = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();
        float xvalue = actions.Player.CandyMove.ReadValue<float>();
        if (vec2 != Vector2.zero && !onMove)
        {
            CheckDirection(vec2, xvalue);
        }
    }
}
