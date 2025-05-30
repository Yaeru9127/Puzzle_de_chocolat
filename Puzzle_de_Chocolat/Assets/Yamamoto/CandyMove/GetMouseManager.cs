using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GetMouseManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private bool isHolding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();

        actions = inputmanager.GetActions();
        inputmanager.PlayerOff();
        inputmanager.UIOn();

        isHolding = false;
    }

    private void GetMouseHold()
    {
        //ホールドを検知
        if (actions.UI.Click.phase == InputActionPhase.Performed)
        {
            if (!isHolding)
            {
                isHolding = true;
                //マウスの位置をワールド座標に落とし込む
                Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
                Vector2 world = Camera.main.ScreenToWorldPoint(mouse);

                //その位置に当たり判定を設置
                Collider2D collider = Physics2D.OverlapPoint(world);

                //当たったオブジェクトがお菓子かどうか
                //(判定条件は仮)
                if (collider != null && collider.gameObject.GetComponent<Sweets>())
                {
                    Debug.Log(collider.name);
                }
                else
                {
                    Debug.Log("none");
                }
            }
        }
        //ホールドを離したとき or クリックしていないとき
        else if (actions.UI.Click.phase == InputActionPhase.Canceled || actions.UI.Click.phase == InputActionPhase.Waiting)
        {
            isHolding = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseHold();
    }
}
