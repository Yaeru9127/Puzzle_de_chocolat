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
        //������
        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();

        actions = inputmanager.GetActions();
        inputmanager.PlayerOff();
        inputmanager.UIOn();

        isHolding = false;
    }

    private void GetMouseHold()
    {
        //�z�[���h�����m
        if (actions.UI.Click.phase == InputActionPhase.Performed)
        {
            if (!isHolding)
            {
                isHolding = true;
                //�}�E�X�̈ʒu�����[���h���W�ɗ��Ƃ�����
                Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
                Vector2 world = Camera.main.ScreenToWorldPoint(mouse);

                //���̈ʒu�ɓ����蔻���ݒu
                Collider2D collider = Physics2D.OverlapPoint(world);

                //���������I�u�W�F�N�g�����َq���ǂ���
                //(��������͉�)
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
        //�z�[���h�𗣂����Ƃ� or �N���b�N���Ă��Ȃ��Ƃ�
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
