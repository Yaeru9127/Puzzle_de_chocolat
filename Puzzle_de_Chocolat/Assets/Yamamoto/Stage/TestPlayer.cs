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
    private PauseController pause;
    private CanGoal cg;
    private CursorController cc;
    private Remainingaircraft remaining;
    private GameOverController goc;

    //プレイヤーが向いている向き
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    /*directionSprites => 0:↑ , 1:↓ , 2:← , 3:→*/
    [SerializeField] private Sprite[] directionSprites = new Sprite[4];

    [SerializeField] private GameObject controllers;
    private GameObject nowmass;         //今いるマス
    private float speed;                //マス間の移動速度
    private bool inProcess;             //処理中フラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        manager = InputSystem_Manager.manager;
        tm = TileManager.tm;
        sm = SweetsManager.sm;
        pause = PauseController.pause;
        cg = CanGoal.cg;
        cc = CursorController.cc;
        remaining = controllers.GetComponent<Remainingaircraft>();
        goc = controllers.GetComponent<GameOverController>();

        //cc.ChangeCursorEnable(false);
        actions = manager.GetActions();
        nowmass = tm.GetNowMass(this.gameObject);
        manager.PlayerOn();
        manager.GamePadOff();
        speed = 0.4f;
        inProcess = false;
    }

    /// <summary>
    /// 現在のマスのスクリプトを返す関数
    /// </summary>
    /// <returns></returns>
    public Tile ReturnNowTileScript()
    {
        return nowmass.GetComponent<Tile>();
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
        Tile nowtile = ReturnNowTileScript();
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
        //次のマスが存在しない場合
        else if (nexttileobj == null)
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

        GameObject newnextmass = null;     //マスオブジェクト
        Sweets nextnextsweets = null;      //お菓子スクリプト
        Sweets pairnextsweets = null;      //ペアのお菓子スクリプト

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
                    /*↓お菓子の先のマスを探す*/
                    //お菓子の先のマスを取得
                    newnextmass = next.GetComponent<Tile>().ReturnNextMass(dire);
                    
                    //2マスのお菓子の場合はペアのお菓子の先のマスも探す
                    if (sweetsscript.pair != null)
                    {
                        //ペアのお菓子のマスを取得
                        GameObject pairmass = tm.GetNowMass(sweetsscript.pair);

                        //ペアのお菓子の先のマスを取得
                        GameObject pairnextmass = pairmass.GetComponent<Tile>().ReturnNextMass(dire);

                        //ペアのお菓子の先のマスがあったら
                        if (pairnextmass != null)
                        {
                            //ペアのお菓子の先のマスにあるお菓子を探す
                            foreach (KeyValuePair<Vector2, Sweets> sweetspair in sm.sweets)
                            {
                                //座標で検索
                                //ペアのお菓子の先のマスにあるお菓子の座標 == ペアのお菓子の先のマスの座標
                                if (sweetspair.Key == (Vector2)pairmass.transform.position)
                                {
                                    //ペアのお菓子変数に設定
                                    pairnextsweets = sweetspair.Value;
                                }
                            }
                        }
                        //ペアのお菓子の先のマスがなかったら
                        else
                        {
                            inProcess = false;
                            return;
                        }
                    }

                    //移動させるお菓子の先のマスを探す
                    foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
                    {
                        //座標で検索
                        //移動させるお菓子の先のマスがある &&
                        //移動させるお菓子の先のマスにあるお菓子の座標 == 移動させるお菓子の先のマスの座標
                        if (newnextmass != null && pair.Key == (Vector2)newnextmass.transform.position)
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
                    newnextmass = backmass;
                }

                //----------------------------------------------------
                //お菓子の先のマスがない or 後ろにマスがない
                if (newnextmass == null)
                {
                    inProcess = false;
                    return;
                }

                newnextmass = next;

                //お菓子を自身の子オブジェクトにする
                sweetsscript.gameObject.transform.SetParent(this.gameObject.transform);
                if (sweetsscript.pair != null)
                {
                    //移動用に親オブジェクトを設定
                    sweetsscript.pair.transform.SetParent(this.gameObject.transform);
                    Debug.Log(sweetsscript.pair.name);
                }
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
                newnextmass = next;
            }
        }
        else
        {
            newnextmass = next;
        }

        /*//次のマスのトラップを取得
        Trap trap = null;
        Collider2D[] col = Physics2D.OverlapPointAll(newnextmass.transform.position);
        foreach (Collider2D col2 in col)
        {
            if (col2.gameObject.GetComponent<Trap>())
            {
                trap = col2.gameObject.GetComponent<Trap>();
            }
        }
        if (trap != null)
        {
            //移動前の段階で次のマスのトラップが作用する場合はそのトラップを処理
        }*/

        //ペアのお菓子のスクリプトを取得
        Sweets pairsweetsscript = null;
        if (sweetsscript != null && sweetsscript.pair != null && sweetsscript.pair.GetComponent<Sweets>())
        {
            pairsweetsscript = sweetsscript.pair.GetComponent<Sweets>();
        }

        MoveMass(newnextmass, sweetsscript, nextnextsweets, pairsweetsscript, pairnextsweets).Forget();
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="next"></param>         移動先のマスオブジェクト
    /// <param name="sweetsscript"></param> 移動させるお菓子スクリプト
    /// <param name="beyond"></param>       移動先のマスにあるお菓子スクリプト
    /// 
    /// <param name="pairsweets"></param>   移動させるお菓子のペアスクリプト
    /// <param name="pairbeyond"></param>   移動させるお菓子の移動先のマスにあるお菓子スクリプト
    private async UniTask MoveMass(GameObject next, Sweets sweetsscript, Sweets beyond, Sweets pairsweets, Sweets pairbeyond)
    {
        //自身の子オブジェクトを取得
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Debug.Log(child.name);
            children.Add(child.gameObject);
        }

        //移動させるお菓子がある && ペアのお菓子が存在する
        if (sweetsscript != null && pairsweets != null)
        {
            //移動先のマスにお菓子が存在する && ペアのお菓子の移動先にお菓子がある
            //= 2マスのお菓子の場合は移動することはできない
            if (beyond !=null && pairbeyond != null)
            {
                //親オブジェクトをリセット
                ResetParent(children);

                inProcess = false;
                return;
            }
            //ペアのお菓子の移動先にお菓子がない
            //= お菓子が移動できる
            else if (pairbeyond == null)
            {
                //移動先のマスにお菓子がある && 移動させるお菓子と移動先のお菓子で作れない
                //= 移動処理なし
                if (beyond != null && !sweetsscript.TryMake(beyond))
                {
                    Debug.Log("can not make");

                    //親子関係をリセット
                    ResetParent(children);

                    inProcess = false;
                    return;
                }

                //移動のためにペアのお菓子の親オブジェクトを設定
                pairsweets.gameObject.transform.SetParent(this.gameObject.transform);
            }
        }
        //移動させるお菓子がある && 移動先のマスにお菓子が存在する && ペアのお菓子が存在しない
        else if (sweetsscript != null && beyond != null && pairsweets == null)
        {
            if (!sweetsscript.TryMake(beyond))
            {
                Debug.Log("can not make");

                //親子関係をリセット
                ResetParent(children);

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

        //元のマスのひびチェック
        ReturnNowTileScript().ChangeSprite();

        //現在地を更新
        nowmass = tm.GetNowMass(this.gameObject);

        //マス情報を更新
        tm.GetAllMass();

        //自身の子オブジェクトが0以外 = 移動するお菓子がある
        if (this.gameObject.transform.childCount != 0)
        {
            //お菓子を作れるとき
            //-> 移動先にお菓子が存在していてペアのお菓子が存在していないとき
            if (sweetsscript != null && beyond != null && pairsweets == null)
            {
                sweetsscript.MakeSweets(beyond.gameObject);
            }
            //-> 移動先にお菓子が存在していないがペアのお菓子の移動先にお菓子が存在しているとき
            else if (sweetsscript != null && beyond == null && pairsweets != null && pairbeyond != null)
            {
                pairsweets.MakeSweets(pairbeyond.gameObject);
            }

            //親子関係をリセット
            ResetParent(children);

            /*残り工程数をひとつ減らす*/
            //Debug.Log("decrease remaining num");
            remaining.ReduceLife();
        }

        //お菓子の位置を更新
        sm.SearchSweets();

        //もし生クリームを踏んだ時の処理   
        Collider2D[] col = Physics2D.OverlapPointAll(nowmass.transform.position);
        foreach (Collider2D col2 in col)
        {
            if (col2.gameObject.GetComponent<Trap>() && col2.gameObject.GetComponent<Trap>().type == Trap.Type.FrischeSahne)
            {
                remaining.ReduceLife();
            }
        }

        //クリアチェック
        //残り移動数が0以下だったら = これ以上移動できない状態なら
        if (remaining.currentLife <= 0)
        {
            //もしゴールできないなら、GameOverの設定
            if (!cg.CanMassThrough(ReturnNowTileScript())) goc.ShowGameOver();
        }

        //処理フラグ更新
        inProcess = false;
    }

    /// <summary>
    /// 親子関係をリセットする関数
    /// </summary>
    /// <param name="child"></param> 
    private void ResetParent(List<GameObject> child)
    {
        foreach (GameObject ch in child)
        {
            ch.transform.SetParent(sm.gameObject.transform);
            Debug.Log(ch.name);
        }
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
            sm.CallDecreaseFoodGauge();
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
        float xvalue = actions.Player.SweetsMove.ReadValue<float>();    //GamePad.X or KeyCode.Shift
        float avalue = actions.Player.Eat.ReadValue<float>();           //GamePad.A or KeyCode.C
        float escape = actions.Player.Pause.ReadValue<float>();         //GamePad.Start or KeyCode.Escape

        //移動
        if (!inProcess && vec2 != Vector2.zero)
        {
            CheckDirection(vec2, xvalue);
        }
        //食べる
        else if (!inProcess && avalue > 0.5f)
        {
            TryEat(direction);
        }
        //ポーズ
        else if (!inProcess && escape > 0.5f)
        {
            if (pause == null) Debug.Log("pause is null");
            pause.SetPause();
        }
    }
}
