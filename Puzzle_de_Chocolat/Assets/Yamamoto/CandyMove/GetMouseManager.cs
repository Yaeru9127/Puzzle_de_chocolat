using UnityEngine;
using UnityEngine.InputSystem;

public class GetMouseManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private bool isHolding;                     //�z�[���h�t���O
    private bool isSweetsMove;                  //���َq�ړ��t���O
    private GameObject movingSweets;            //�ړ����邨�َq�ϐ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();

        actions = inputmanager.GetActions();
        inputmanager.PlayerOff();
        inputmanager.UIPointOn();
        inputmanager.UIClickOn();

        isHolding = false;
        isSweetsMove = false;
        movingSweets = null;
    }

    //�z�[���h�擾�֐�
    private void GetMouseHold()
    {
        //�z�[���h�����m
        if (actions.UI.Click.phase == InputActionPhase.Performed)
        {
            //���Ƀz�[���h���Ă��邩
            if (!isHolding)
            {
                isHolding = true;

                //�}�E�X�̈ʒu�����[���h���W�ɗ��Ƃ�����
                Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
                Vector3 vec3 = Camera.main.ScreenToWorldPoint(mouse);
                Vector2 world = new Vector2(vec3.x, vec3.y);

                //���̈ʒu�ɓ����蔻���ݒu
                Collider2D[] colliders = Physics2D.OverlapPointAll(world);

                //���������I�u�W�F�N�g�����َq���ǂ���
                //(��������͉�)
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.GetComponent<Sweets>())
                    {
                        //���َq�̈ړ�
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

    //�z�[���h�𗣂����Ƃ��̊֐�
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

    //���َq��z�u����֐�
    private void SetSweets()
    {
        //�}�E�X�̈ʒu�����[���h���W�ɗ��Ƃ�����
        Vector2 mouse = actions.UI.Point.ReadValue<Vector2>();
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(mouse);
        Vector2 world = new Vector2(vec3.x, vec3.y);
        
        //���̈ʒu�ɓ����蔻���ݒu
        Collider2D[] colliders = Physics2D.OverlapPointAll(world);

        //���������I�u�W�F�N�g���^�C�����ǂ���
        //(��������͉�)
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Tile>())
            {
                //�^�C���̏ꏊ�ɂ��َq��z�u
                Tile tilescript = collider.GetComponent<Tile>();
                isSweetsMove = false;
                movingSweets.transform.position = tilescript.GetTilePos();
                movingSweets = null;
            }
        }
    }

    //���َq�̃z�[���h�ړ��֐�
    private void SweetsMove(GameObject sweets)
    {
        //�}�E�X�̈ʒu�����[���h���W�ɗ��Ƃ�����
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
