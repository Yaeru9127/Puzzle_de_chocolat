using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private TileManager tm;

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
        tm = TileManager.tm;
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
        else if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            //yの値が大きい => 上
            if (dir.y > 0) directo = Vector2.up;

            //yの値が小さい => 下
            else directo = Vector2.down;
        }

        //入力値が0だったらreturn
        if (directo == Vector2.zero)
        {
            onMove = false;
            return;
        }

        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttileobj = nowtile.ReturnNextMass(directo);
        TryMove(nexttileobj, xbutton, directo);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param> 移動先のマス
    /// <param name="X"></param>    X or Shiftのボタンの押している値
    /// <param name="dire"></param> 入力された移動方向
    private void TryMove(GameObject next, float X, Vector2 dire)
    {
        //次のマスにあるお菓子を取得
        Sweets sweetsscript;
        sweetsscript = tm.GetSweets(next.transform.position);

        //進行方向にお菓子がない場合、後ろのマスを検索する
        GameObject backmass = null;
        if (sweetsscript == null)
        {
            //後ろの位置関係Vector2を取得
            Vector2 reverse = Vector2.zero;
            if (dire == Vector2.up) reverse = Vector2.down;
            else if (dire == Vector2.down) reverse = Vector2.up;
            else if (dire == Vector2.left) reverse = Vector2.right;
            else if (dire == Vector2.right) reverse = Vector2.left;
            backmass = nowmass.GetComponent<Tile>().ReturnNextMass(reverse);

            if (backmass != null)
            {
                //後ろのマスを取得後、そのマスにお菓子があるかを探す
                if (reverse != Vector2.zero)
                {
                    GameObject nextback = nowmass.GetComponent<Tile>().ReturnNextMass(reverse);

                    //後ろのマスが存在する場合
                    if (nextback != null)
                    {
                        //後ろのマスにお菓子があるかを探す
                        sweetsscript = tm.GetSweets(nextback.transform.position);
                    }
                }
                else //方向が上下左右ではない場合（たぶんないけど）
                {
                    onMove = false;
                    return;
                }
                
            }
        }

        /*//デバッグ
        if (sweetsscript != null) Debug.Log($"{sweetsscript.gameObject.name}");
        else Debug.Log("sweetsscript is null");*/

        Tile nextnexttile;
        GameObject nextnextmass = null;

        //次のマスにお菓子があったら
        if (sweetsscript != null)
        {
            //X or Shiftを押していたら
            if (X > 0.5f)
            {
                //後ろのマス変数がnull => 後ろのマスを探していない
                if (backmass == null)
                {
                    //お菓子の先のマスを取得
                    nextnexttile = next.GetComponent<Tile>();
                    nextnextmass = nextnexttile.ReturnNextMass(dire);
                }
                //後ろのマス変数がnull以外 => 後ろのマスにお菓子がある
                else if (backmass != null)
                {
                    //自分の後ろのマスを取得
                    nextnexttile = backmass.GetComponent<Tile>();
                    nextnextmass = backmass;
                }

                //お菓子の先のマスがない or 後ろにマスがない
                if (nextnextmass == null)
                {
                    onMove = false;
                    return;
                }

                nextnextmass = next;

                //お菓子を自身の子オブジェクトにする
                sweets = sweetsscript.gameObject;
                sweets.transform.SetParent(this.gameObject.transform);
            }
            //X or Shiftを押していない && お菓子オブジェクトがある
            else if (X > 0.5f && sweetsscript == null)
            {

            }
            //X or Shiftを押していない or お菓子オブジェクトがない
            else
            {
                Debug.Log("sweets is not null");
                onMove = false;
                return;
            }
        }
        else
        {
            nextnextmass = next;
        }

        MoveMass(nextnextmass).Forget();
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="next"></param>   移動先のマスオブジェクト
    private async UniTask MoveMass(GameObject next)
    {
        Vector3 pos = next.transform.position;
        pos.z = -5;

        await this.gameObject.transform.DOMove(pos, speed)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();

        //現在地を更新
        GetNowMass();

        if (sweets != null && this.gameObject.transform.childCount != 0)
        {
            sweets.transform.SetParent(tm.gameObject.transform);
            sweets = null;
        }

        tm.SearchSweets();

        //移動フラグ更新
        onMove = false;
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
