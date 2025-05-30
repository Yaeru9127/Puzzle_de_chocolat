using UnityEngine;
using UnityEngine.InputSystem;

public class GetMouseManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private bool isHolding;                     //ホールドフラグ
    private bool isSweetsMove;                  //お菓子移動フラグ
    private GameObject movingSweets;            //移動するお菓子変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();

        actions = inputmanager.GetActions();
        inputmanager.PlayerOff();
        inputmanager.UIPointOn();
        inputmanager.UIClickOn();

        isHolding = false;
        isSweetsMove = false;
        movingSweets = null;
    }

    //ホールド取得関数
    private void GetMouseHold()
    {
        //ホールドを検知
        if (actions.UI.Click.phase == InputActionPhase.Performed)
        {
            //既にホールドしているか
            if (!isHolding)
            {
                isHolding = true;

                //マウスの位置をワールド座標に落とし込む
                Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
                Vector3 vec3 = Camera.main.ScreenToWorldPoint(mouse);
                Vector2 world = new Vector2(vec3.x, vec3.y);

                //その位置に当たり判定を設置
                Collider2D[] colliders = Physics2D.OverlapPointAll(world);

                //当たったオブジェクトがお菓子かどうか
                //(判定条件は仮)
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.GetComponent<Sweets>())
                    {
                        //お菓子の移動
                        if (movingSweets == null)
                        {
                            movingSweets = collider.gameObject;
                            isSweetsMove = true;
                        }
                    }
                }

                if (movingSweets == null)
                {
                    isHolding = false;
                }
            }
        }
    }

    //ホールドを離したときの関数
    private void ReleaseMouseHold()
    {
        actions.UI.Click.canceled += ctx =>
        {
            if (isHolding && isSweetsMove)
            {
                SetSweets();
            }
        };
    }

    //お菓子を配置する関数
    private void SetSweets()
    {
        //マウスの位置をワールド座標に落とし込む
        Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(mouse);
        Vector2 world = new Vector2(vec3.x, vec3.y);
        
        //その位置に当たり判定を設置
        Collider2D[] colliders = Physics2D.OverlapPointAll(world);

        //当たったオブジェクトがタイルかどうか
        //(判定条件は仮)
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Tile>())
            {
                //タイルの場所にお菓子を配置
                Tile tilescript = collider.GetComponent<Tile>();
                isSweetsMove = false;
                movingSweets.transform.position = tilescript.GetTilePos();
                movingSweets = null;
            }
        }
    }

    //お菓子のホールド移動関数
    private void SweetsMove(GameObject sweets)
    {
        //マウスの位置をワールド座標に落とし込む
        Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(mouse);
        Vector2 world = new Vector2(vec3.x, vec3.y);
        sweets.transform.position = (Vector2)world;
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseHold();
        ReleaseMouseHold();

        if (isSweetsMove && movingSweets != null)
        {
            SweetsMove(movingSweets);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isHolding = false;
        }
    }
}
