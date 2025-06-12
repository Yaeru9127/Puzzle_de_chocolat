using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    private TileManager tm;
    private SweetsManager sm;
    private ScreenController cc;

    //プレイヤーが向いている向き
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    [SerializeField] private Sprite[] directionSprites = new Sprite[4];
    /*directionSprites => 0:↑ , 1:↓ , 2:← , 3:→*/
    private GameObject nowmass;          //今いるマス
    private float speed;                //マス間の移動速度
    private GameObject sweets;          //一時的なお菓子変数
    private bool inProcess;             //処理中フラグ

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        manager = InputSystem_Manager.manager;
        tm = TileManager.tm;
        sm = SweetsManager.sm;
        cc = ScreenController.cc;

        actions = manager.GetActions();
        nowmass = tm.GetNowMass(this.gameObject);
        manager.PlayerOn();
        manager.UIOff();
        speed = 0.4f;
        inProcess = false;
    }

    /// <summary>
    /// 入力値から向きを算出する関数
    /// </summary>
    /// <param name="dir"></param>         入力値
    /// <param name="button"></param>      (X or Shift) or (A or C)ボタンを押しているか
    private void CheckDirection(Vector2 dir, float button)
    {
        if (!inProcess) inProcess = true;
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
            inProcess = false;
            return;
        }

        //プレイヤーの向きを設定
        SetPlayerDirection(directo);

        //入力方向にある次のマスを取得
        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttileobj = nowtile.ReturnNextMass(directo);

        /*次のマスが存在する場合*/
        //移動
        if (nexttileobj != null)
        {
            //デバッグ
            //Debug.Log($"next mass is {nexttileobj}");

            //移動チェック
            TryMove(nexttileobj, button, directo);
        }
        //食べる
        else if (nexttileobj != null)
        {
            //食べるお菓子のスクリプトを取得
            Sweets eatnext = sm.GetSweets(nexttileobj.transform.position);

            //nullじゃなかったら食べる処理へ
            if (eatnext != null)
            {
                //お菓子を食べる
                eatnext.EatSweets();

                //食料ゲージの増加
                Debug.Log("food gauge is increase");
            }
            else Debug.Log("script of to eat is null");

            inProcess = false;
            return;
        }
        //次のマスが存在しない場合
        else
        {
            Debug.Log($"next mass is null");
            inProcess = false;
            return;
        }
    }

    /// <summary>
    /// 入力値からプレイヤーの向きを設定する関数
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private void SetPlayerDirection(Vector2 dir)
    {
        //入力値から判断
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (dir == Vector2.up)
        {
            direction = Direction.Up;
            renderer.sprite = directionSprites[0];
        }
        else if (dir == Vector2.down)
        {
            direction = Direction.Down;
            renderer.sprite = directionSprites[1];
        }
        else if (dir == Vector2.left)
        {
            direction = Direction.Left;
            renderer.sprite = directionSprites[2];
        }
        else if (dir == Vector2.right)
        {
            direction = Direction.Right;
            renderer.sprite = directionSprites[3];
        }
    }

    /// <summary>
    /// 移動できるかチェックする関数
    /// </summary>
    /// <param name="next"></param> 移動先のマス
    /// <param name="X"></param>    X or Shiftのボタンの押している値
    /// <param name="dire"></param> 入力された移動方向
    private void TryMove(GameObject next, float X, Vector2 dire)
    {
        //目の前のマスにあるお菓子を取得
        Sweets sweetsscript;
        sweetsscript = sm.GetSweets(next.transform.position);

        //目の前にお菓子がない場合、後ろのマスを検索する
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

            //後ろのマスがある
            if (backmass != null)
            {
                //向いている方向と逆方向のマスにお菓子があるかを探す
                if (reverse != Vector2.zero)
                {
                    //Debug.Log("search in reverse direction mass");
                    sweetsscript = sm.GetSweets(backmass.transform.position);
                }
                else //方向が上下左右ではない場合（たぶんないけど）
                {
                    inProcess = false;
                    return;
                }
            }
            //else Debug.Log("back mass is null");
        }

        /*//デバッグ
        if (sweetsscript != null) Debug.Log($"{sweetsscript.gameObject.name}");
        else Debug.Log("sweetsscript is null");*/

        GameObject nextnextmass = null;     //上書き用マスオブジェクト
        Sweets nextnextsweets = null;       //上書き用お菓子スクリプト

        //前か後ろのマスにお菓子があったら
        if (sweetsscript != null)
        {
            //X or Shiftを押している
            if (X > 0.5f)
            {
                //移動できないお菓子だったらreturn
                if (!sweetsscript.canMove)
                {
                    inProcess = false;
                    return;
                }

                //----------------------------------------------------
                //向いている方向にお菓子を移動させる
                //後ろのマス変数がnull => 後ろのマスを探していない
                if (backmass == null)
                {
                    /*↓お菓子の先のマスにお菓子があるか探す*/
                    //お菓子の先のマスを取得
                    nextnextmass = next.GetComponent<Tile>().ReturnNextMass(dire);

                    //お菓子の先のマスのお菓子を取得
                    foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
                    {
                        //移動させるお菓子の座標 と (Vector2)お菓子のその先のマス の座標を比較
                        if (nextnextmass != null && pair.Key == (Vector2)nextnextmass.transform.position)
                        {
                            nextnextsweets = pair.Value;

                            break;
                        }
                    }
                }
                //----------------------------------------------------
                //向いている方向とは逆方向に移動する
                //後ろのマス変数がnull以外 => 後ろのマスにお菓子がある
                else if (backmass != null)
                {
                    //自分の後ろのマスを取得
                    nextnextmass = backmass;
                }

                //----------------------------------------------------
                //お菓子の先のマスがない or 後ろにマスがない
                if (nextnextmass == null)
                {
                    inProcess = false;
                    return;
                }

                nextnextmass = next;

                //お菓子を自身の子オブジェクトにする
                sweets = sweetsscript.gameObject;
                sweets.transform.SetParent(this.gameObject.transform);
            }
            //X or Shiftを押していない
            else
            {
                //移動先のマスにお菓子があるか探す
                foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
                {
                    //お菓子があったらreturn
                    if ((Vector2)next.transform.position == pair.Key)
                    {
                        inProcess = false;
                        return;
                    }
                }

                //移動先のマスにお菓子がない
                nextnextmass = next;
            }
        }
        else
        {
            nextnextmass = next;
        }

        MoveMass(nextnextmass, sweetsscript, nextnextsweets).Forget();
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="next"></param>         移動先のマスオブジェクト
    /// <param name="sweetsscript"></param> 移動させるお菓子スクリプト
    /// <param name="beyond"></param>       移動先のマスにあるお菓子スクリプト
    /// canmake = null ; 移動させるお菓子の先のマスにお菓子がない
    /// canmake != null; 移動させるお菓子の先のマスにお菓子がある
    private async UniTask MoveMass(GameObject next, Sweets sweetsscript, Sweets beyond)
    {
        //移動先のマスにお菓子オブジェクトがある
        if (beyond != null)
        {
            //"移動するお菓子"と"移動先のお菓子"で作れるか
            //作れる = true  作れない = false
            if (!sweetsscript.TryMake(beyond))
            {
                inProcess = false;
                return;
            }
        }

        //移動先の場所の設定
        Vector3 pos = next.transform.position;
        pos.z = -5;

        //移動が終わるまで処理を待つ
        await this.gameObject.transform.DOMove(pos, speed)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();

        //現在地を更新
        nowmass = tm.GetNowMass(this.gameObject);

        //お菓子変数がnullじゃない && 自身の子オブジェクトが0以上
        // = 移動するお菓子がある
        if (sweets != null && this.gameObject.transform.childCount != 0)
        {
            //お菓子を作れるとき
            if (sweetsscript != null && beyond != null) sweetsscript.MakeSweets(beyond.gameObject);
            //作れないとき
            else
            {
                //お菓子オブジェクトの親を初期化
                sweets.transform.SetParent(sm.gameObject.transform);
                sweets = null;
            }

            /*工程数をひとつ減らす*/
            Debug.Log("decrease remaining num");
        }

        //お菓子の位置を更新
        sm.SearchSweets();

        //クリアチェック
        cc.ClearCheck((Vector2)this.transform.position);

        //処理フラグ更新
        inProcess = false;
    }

    /// <summary>
    /// お菓子が食べれるかチェックする関数
    /// </summary>
    /// <param name="dire"></param> Direction = 向いている方向
    private void TryEat(Direction dire)
    {
        if (!inProcess) inProcess = true;

        //向いている方向から位置関係Vector2を取得
        Vector2 original = Vector2.zero;
        switch (dire)
        {
            case Direction.Up:
                original = Vector2.up;
                break;
            case Direction.Down:
                original = Vector2.down;
                break;
            case Direction.Left:
                original = Vector2.left;
                break;
            case Direction.Right:
                original = Vector2.right;
                break;
        }

        //方向nullチェック
        if (original == Vector2.zero)
        {
            inProcess = false;
            return;
        }

        //向いている方向の次のマスを取得
        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttile = nowtile.ReturnNextMass(original);

        //マスのnullチェック
        if (nexttile == null)
        {
            inProcess = false;
            return;
        }

        //次のマスにあるお菓子のスクリプトを取得
        Sweets eatnext = sm.GetSweets(nexttile.transform.position);

        //お菓子スクリプトがnull or 食べれないお菓子 なら
        if (eatnext == null)
        {
            inProcess = false;
            return;
        }

        if (eatnext.canEat)
        {
            //お菓子を食べる
            eatnext.EatSweets();

            //食料ゲージの増加
            Debug.Log("food gauge is increase");
        }
        else Debug.Log("this food can not eat");


        //処理フラグの更新
        inProcess = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ユーザー入力を受け取る
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();        //移動入力値
        float xvalue = actions.Player.SweetsMove.ReadValue<float>();    //X or KeyCode.Shift
        float avalue = actions.Player.Eat.ReadValue<float>();           //A or KeyCode.C

        //移動
        if (vec2 != Vector2.zero && !inProcess)
        {
            CheckDirection(vec2, xvalue);
        }
        //食べる
        else if (!inProcess && avalue > 0.5f)
        {
            TryEat(direction);
        }
        

    }
}
