using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class moveplayer : MonoBehaviour
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
    //private CursorController cc;
    private Remainingaircraft remain;
    private GameOverController goc;
    private ReloadCountManager rcm;
    private StageManager stage;
    private GameClear clear;

    //プレイヤーが向いている向き
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    private Animator animator;
    [SerializeField] private Sprite[] sprites = new Sprite[4];
    private GameObject nowmass;         //今いるマス
    private float speed;                //マス間の移動速度
    private bool inProcess;             //処理中フラグ
    private float lastX;
    private float lastY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        manager = InputSystem_Manager.manager;
        tm = TileManager.tm;
        sm = SweetsManager.sm;
        pause = PauseController.pause;
        cg = CanGoal.cg;
        //cc = CursorController.cc;
        remain = Remainingaircraft.remain;
        goc = GameOverController.over;
        rcm = ReloadCountManager.Instance;
        stage = StageManager.stage;
        clear = GameClear.clear;
        animator = this.gameObject.GetComponent<Animator>();

        actions = manager.GetActions();
        nowmass = tm.GetNowMass(this.gameObject);
        //manager.PlayerOn();
        //manager.GamePadOff();
        //cc.ChangeCursorEnable(false);
        stage.phase = StageManager.Phase.Game;
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
    /// <param name="button"></param>      X or Shift ボタンを押しているか
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

        lastX = directo.x;
        lastY = directo.y;

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
            //Debug.Log($"next mass is null");
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
        if (dir == Vector2.up) direction = Direction.Up;
        else if (dir == Vector2.down) direction = Direction.Down;
        else if (dir == Vector2.left) direction = Direction.Left;
        else if (dir == Vector2.right) direction = Direction.Right;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
        animator.speed = 1f;        // アニメーション再開

        // X方向入力がある場合だけ反転処理
        if (dir.x != 0)
        {
            renderer.flipX = (dir.x > 0);
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
            //Debug.Log(child.name);
            children.Add(child.gameObject);
        }

        //移動させるお菓子がある && ペアのお菓子が存在する
        if (sweetsscript != null && pairsweets != null)
        {
            //移動先のマスにお菓子が存在する && ペアのお菓子の移動先にお菓子がある
            //= 2マスのお菓子の場合は移動することはできない
            if (beyond != null && pairbeyond != null)
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
                //Debug.Log("can not make");

                //親子関係をリセット
                ResetParent(children);

                inProcess = false;
                return;
            }
        }

        //移動先の場所の設定
        Vector3 pos = next.transform.position;
        pos.z = -5;

        //自身の子オブジェクトが0以外 = 移動するお菓子がある
        if (this.gameObject.transform.childCount != 0 && AudioManager.Instance != null)
        {
            //お菓子を移動させるときのSEを流す
            AudioManager.Instance.PlaySE("move");
        }
        //移動するお菓子がない
        else if (AudioManager.Instance != null)
        {
            //移動SEを流す
            AudioManager.Instance.PlaySE("RUN");
        }

        //移動が終わるまで処理を待つ
        await this.gameObject.transform.DOMove(pos, speed)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();

        //一時的に入力を受け付けなくする
        manager.PlayerOff();

        //移動SEを止める
        AudioManager.Instance.seAudioSource.Stop();

        //元のマスのひびチェック
        //ReturnNowTileScript().ChangeSprite();

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
                clear.wasMaked = true;
            }
            //-> 移動先にお菓子が存在していないがペアのお菓子の移動先にお菓子が存在しているとき
            else if (sweetsscript != null && beyond == null && pairsweets != null && pairbeyond != null)
            {
                pairsweets.MakeSweets(pairbeyond.gameObject);
                clear.wasMaked = true;
            }

            //親子関係をリセット
            ResetParent(children);

            /*残り工程数をひとつ減らす*/
            //Debug.Log("decrease remaining num");
            remain.ReduceLife();
        }

        //お菓子の位置を更新
        sm.SearchSweets();
        sm.SetEffect();

        //もし生クリームを踏んだ時の処理   
        Collider2D[] col = Physics2D.OverlapPointAll(nowmass.transform.position);
        foreach (Collider2D col2 in col)
        {
            if (col2.gameObject.GetComponent<Trap>() && col2.gameObject.GetComponent<Trap>().type == Trap.Type.FrischeSahne)
            {
                remain.ReduceLife();
            }
        }

        /*//デバッグ
        cg.searched.Clear();
        Debug.Log(cg.CanMassThrough(ReturnNowTileScript()));*/

        //クリアチェック
        //現在の残り工程数が0 && 現在のマスがゴールでないなら
        if (remain.currentLife == 0 && nowmass != cg.goal)
        {
            //ゴール判定リストの初期化
            cg.searched.Clear();

            //もしゴールできないなら、GameOverの設定
            //if (!cg.CanMassThrough(ReturnNowTileScript()))
            //{
            //    Debug.Log("in can not goal");
            //    goc.ShowGameOver();
            //    stage.phase = StageManager.Phase.Result;
            //}
        }

        //ゴールマスについたら
        if (nowmass == cg.goal)
        {
            //Debug.Log("reach goal");
            //manager.PlayerOff();
            //cc.ChangeCursorEnable(true);
            clear.ShowClearResult(rcm.ReloadCount);

            return;
        }

        //アニメーション設定
        animator.speed = 0f;
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", 0);

        //入力を受け付ける
        manager.PlayerOn();

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
            //Debug.Log(ch.name);
        }
    }

    /// <summary>
    /// お菓子が食べれるかチェックする関数
    /// </summary>
    /// <param name="dire"></param> Direction = 向いている方向
    private async void TryEat(Direction dire)
    {
        if (!inProcess) inProcess = true;

        //一時的に入力を受け付けなくする
        manager.PlayerOff();

        //向いている方向から位置関係Vector2を取得
        Vector2 original = direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };

        //方向nullチェック
        if (original == Vector2.zero)
        {
            //入力を受け付けるようにする
            manager.PlayerOn();

            inProcess = false;
            return;
        }

        //向いている方向の次のマスを取得
        GameObject nexttile = nowmass.GetComponent<Tile>().ReturnNextMass(original);

        //マスのnullチェック
        if (nexttile == null)
        {
            //入力を受け付けるようにする
            manager.PlayerOn();

            inProcess = false;
            return;
        }

        //次のマスにあるお菓子のスクリプトを取得
        Sweets eatnext = sm.GetSweets(nexttile.transform.position);

        //お菓子スクリプトがnull or 食べれないお菓子 なら
        if (eatnext == null || !eatnext.canEat)
        {
            //入力を受け付けるようにする
            manager.PlayerOn();

            inProcess = false;
            return;
        }

        await eatnext.EatSweets();

        //工程数をひとつ減らす
        //remain.ReduceLife();

        //食料ゲージの増加
        sm.CallDecreaseFoodGauge();

        //お菓子の位置の更新
        sm.SearchSweets();
        sm.SetEffect();

        await UniTask.Delay(800);

        if (remain.currentLife > 0)
        {
            //入力を受け付けるようにする
            manager.PlayerOn();

            //処理フラグの更新
            inProcess = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ユーザー入力を受け取る
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();        //移動入力値
        float xvalue = actions.Player.SweetsMove.ReadValue<float>();    //GamePad.X or KeyCode.Shift
        float escape = actions.Player.Pause.ReadValue<float>();         //GamePad.Start or KeyCode.Escape
        float r = actions.Player.Retry.ReadValue<float>();              //GamePad.Y or KeyCode.R

        //移動
        if (!inProcess && vec2 != Vector2.zero)
        {
            if ((Mathf.Abs(vec2.x) < 0.3f && Mathf.Abs(vec2.y) < 0.3f)) return;
            CheckDirection(vec2, xvalue);
        }
        else if (!inProcess && vec2 == Vector2.zero)
        {
            // 移動がゼロのときは停止処理（最後の方向でフレーム停止）
            /*animator.speed = 0f;        // アニメーション停止
            animator.SetFloat("MoveX", lastX);
            animator.SetFloat("MoveY", lastY);*/


            SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
            if (lastX != 0)
            {
                renderer.flipX = (lastX > 0);
            }
        }

        //食べる
        if (!inProcess && actions.Player.Eat.WasPressedThisFrame())
        {
            TryEat(direction);
        }

        //ポーズ
        if (!inProcess && escape > 0.5f && stage.phase == StageManager.Phase.Game &&
            (!goc.gameOverImage.gameObject.activeSelf || !clear.clearImage.gameObject.activeSelf))
        {
            if (pause == null) Debug.Log("pause is null");
            pause.SetPause();
        }

        //リトライ
        if (!inProcess && r > 0.5f)
        {
            rcm.IncrementReloadCount();     //リロードカウントを増やす
            manager.Retry();
        }

        //GameOver or GameClear
        /*if (goc.gameOverImage.gameObject.activeSelf || clear.clearImage.gameObject.activeSelf)
        {
            //入力を受け付けない
            manager.PlayerOff();
        }*/
    }
}
