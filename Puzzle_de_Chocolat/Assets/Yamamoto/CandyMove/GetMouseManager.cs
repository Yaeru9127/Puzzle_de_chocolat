using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GetMouseManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;

    private float holidStartTime;
    private bool isHolding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();

        actions = inputmanager.GetActions();
        inputmanager.UIOn();

        isHolding = false;
    }

    private void GetMouseHold()
    {
        //�z�[���h�����͂��߂����m
        if (actions.UI.Click.WasPressedThisFrame())
        {
            isHolding = true;
            holidStartTime = Time.time;
        }
        //�z�[���h��b�����u��
        else if (actions.UI.Click.WasReleasedThisFrame())
        {
            isHolding = false;

            //�������ʒu�����[���h���W�ɕϊ����Ă��̈ʒu�ɃI�u�W�F�N�g�����邩�m�F����
            Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
            Vector2 world = Camera.main.ScreenToWorldPoint(mouse);
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log($"{hit.collider.name}");
            }
            else
            {
                Debug.Log("not hit");
            }
        }
    }

    private void HoldTimeCheck()
    {
        if (isHolding && Time.time - holidStartTime >= 0.2f)
        {
            Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
            Vector2 world = Camera.main.ScreenToWorldPoint(mouse);
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("hold is hit");
            }

            isHolding = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseHold();
        HoldTimeCheck();
    }
}
