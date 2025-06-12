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

    //�v���C���[�������Ă������
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    [SerializeField] private Sprite[] directionSprites = new Sprite[4];
    /*directionSprites => 0:�� , 1:�� , 2:�� , 3:��*/
    private GameObject nowmass;          //������}�X
    private float speed;                //�}�X�Ԃ̈ړ����x
    private GameObject sweets;          //�ꎞ�I�Ȃ��َq�ϐ�
    private bool inProcess;             //�������t���O

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
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
    /// ���͒l����������Z�o����֐�
    /// </summary>
    /// <param name="dir"></param>         ���͒l
    /// <param name="button"></param>      (X or Shift) or (A or C)�{�^���������Ă��邩
    private void CheckDirection(Vector2 dir, float button)
    {
        if (!inProcess) inProcess = true;
        Vector2 directo = Vector2.zero;
        //���E�̓��͕���������(���Ԃ�Ȃ��Ǝv������)
        if (Mathf.Abs(dir.x) == Mathf.Abs(dir.y))
        {
            //y�̓��͒l�Ŕ��f
            if ((dir.x > 0 && dir.y > 0) || (dir.x < 0 && dir.y > 0)) directo = Vector2.up;
            else directo = Vector2.down;
        }
        //x�̕���y���傫��
        else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            //x�̒l���傫�� => �E
            if (dir.x > 0) directo = Vector2.right;

            //x�̒l�������� => ��
            else directo = Vector2.left;
        }
        //y�̕���x���傫��
        else if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            //y�̒l���傫�� => ��
            if (dir.y > 0) directo = Vector2.up;

            //y�̒l�������� => ��
            else directo = Vector2.down;
        }

        //���͒l��0��������return
        if (directo == Vector2.zero)
        {
            inProcess = false;
            return;
        }

        //�v���C���[�̌�����ݒ�
        SetPlayerDirection(directo);

        //���͕����ɂ��鎟�̃}�X���擾
        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttileobj = nowtile.ReturnNextMass(directo);

        /*���̃}�X�����݂���ꍇ*/
        //�ړ�
        if (nexttileobj != null)
        {
            //�f�o�b�O
            //Debug.Log($"next mass is {nexttileobj}");

            //�ړ��`�F�b�N
            TryMove(nexttileobj, button, directo);
        }
        //�H�ׂ�
        else if (nexttileobj != null)
        {
            //�H�ׂ邨�َq�̃X�N���v�g���擾
            Sweets eatnext = sm.GetSweets(nexttileobj.transform.position);

            //null����Ȃ�������H�ׂ鏈����
            if (eatnext != null)
            {
                //���َq��H�ׂ�
                eatnext.EatSweets();

                //�H���Q�[�W�̑���
                Debug.Log("food gauge is increase");
            }
            else Debug.Log("script of to eat is null");

            inProcess = false;
            return;
        }
        //���̃}�X�����݂��Ȃ��ꍇ
        else
        {
            Debug.Log($"next mass is null");
            inProcess = false;
            return;
        }
    }

    /// <summary>
    /// ���͒l����v���C���[�̌�����ݒ肷��֐�
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private void SetPlayerDirection(Vector2 dir)
    {
        //���͒l���画�f
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
    /// �ړ��ł��邩�`�F�b�N����֐�
    /// </summary>
    /// <param name="next"></param> �ړ���̃}�X
    /// <param name="X"></param>    X or Shift�̃{�^���̉����Ă���l
    /// <param name="dire"></param> ���͂��ꂽ�ړ�����
    private void TryMove(GameObject next, float X, Vector2 dire)
    {
        //�ڂ̑O�̃}�X�ɂ��邨�َq���擾
        Sweets sweetsscript;
        sweetsscript = sm.GetSweets(next.transform.position);

        //�ڂ̑O�ɂ��َq���Ȃ��ꍇ�A���̃}�X����������
        GameObject backmass = null;
        if (sweetsscript == null)
        {
            //���̈ʒu�֌WVector2���擾
            Vector2 reverse = Vector2.zero;
            if (dire == Vector2.up) reverse = Vector2.down;
            else if (dire == Vector2.down) reverse = Vector2.up;
            else if (dire == Vector2.left) reverse = Vector2.right;
            else if (dire == Vector2.right) reverse = Vector2.left;
            backmass = nowmass.GetComponent<Tile>().ReturnNextMass(reverse);

            //���̃}�X������
            if (backmass != null)
            {
                //�����Ă�������Ƌt�����̃}�X�ɂ��َq�����邩��T��
                if (reverse != Vector2.zero)
                {
                    //Debug.Log("search in reverse direction mass");
                    sweetsscript = sm.GetSweets(backmass.transform.position);
                }
                else //�������㉺���E�ł͂Ȃ��ꍇ�i���Ԃ�Ȃ����ǁj
                {
                    inProcess = false;
                    return;
                }
            }
            //else Debug.Log("back mass is null");
        }

        /*//�f�o�b�O
        if (sweetsscript != null) Debug.Log($"{sweetsscript.gameObject.name}");
        else Debug.Log("sweetsscript is null");*/

        GameObject nextnextmass = null;     //�㏑���p�}�X�I�u�W�F�N�g
        Sweets nextnextsweets = null;       //�㏑���p���َq�X�N���v�g

        //�O�����̃}�X�ɂ��َq����������
        if (sweetsscript != null)
        {
            //X or Shift�������Ă���
            if (X > 0.5f)
            {
                //�ړ��ł��Ȃ����َq��������return
                if (!sweetsscript.canMove)
                {
                    inProcess = false;
                    return;
                }

                //----------------------------------------------------
                //�����Ă�������ɂ��َq���ړ�������
                //���̃}�X�ϐ���null => ���̃}�X��T���Ă��Ȃ�
                if (backmass == null)
                {
                    /*�����َq�̐�̃}�X�ɂ��َq�����邩�T��*/
                    //���َq�̐�̃}�X���擾
                    nextnextmass = next.GetComponent<Tile>().ReturnNextMass(dire);

                    //���َq�̐�̃}�X�̂��َq���擾
                    foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
                    {
                        //�ړ������邨�َq�̍��W �� (Vector2)���َq�̂��̐�̃}�X �̍��W���r
                        if (nextnextmass != null && pair.Key == (Vector2)nextnextmass.transform.position)
                        {
                            nextnextsweets = pair.Value;

                            break;
                        }
                    }
                }
                //----------------------------------------------------
                //�����Ă�������Ƃ͋t�����Ɉړ�����
                //���̃}�X�ϐ���null�ȊO => ���̃}�X�ɂ��َq������
                else if (backmass != null)
                {
                    //�����̌��̃}�X���擾
                    nextnextmass = backmass;
                }

                //----------------------------------------------------
                //���َq�̐�̃}�X���Ȃ� or ���Ƀ}�X���Ȃ�
                if (nextnextmass == null)
                {
                    inProcess = false;
                    return;
                }

                nextnextmass = next;

                //���َq�����g�̎q�I�u�W�F�N�g�ɂ���
                sweets = sweetsscript.gameObject;
                sweets.transform.SetParent(this.gameObject.transform);
            }
            //X or Shift�������Ă��Ȃ�
            else
            {
                //�ړ���̃}�X�ɂ��َq�����邩�T��
                foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
                {
                    //���َq����������return
                    if ((Vector2)next.transform.position == pair.Key)
                    {
                        inProcess = false;
                        return;
                    }
                }

                //�ړ���̃}�X�ɂ��َq���Ȃ�
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
    /// �ړ��֐�
    /// </summary>
    /// <param name="next"></param>         �ړ���̃}�X�I�u�W�F�N�g
    /// <param name="sweetsscript"></param> �ړ������邨�َq�X�N���v�g
    /// <param name="beyond"></param>       �ړ���̃}�X�ɂ��邨�َq�X�N���v�g
    /// canmake = null ; �ړ������邨�َq�̐�̃}�X�ɂ��َq���Ȃ�
    /// canmake != null; �ړ������邨�َq�̐�̃}�X�ɂ��َq������
    private async UniTask MoveMass(GameObject next, Sweets sweetsscript, Sweets beyond)
    {
        //�ړ���̃}�X�ɂ��َq�I�u�W�F�N�g������
        if (beyond != null)
        {
            //"�ړ����邨�َq"��"�ړ���̂��َq"�ō��邩
            //���� = true  ���Ȃ� = false
            if (!sweetsscript.TryMake(beyond))
            {
                inProcess = false;
                return;
            }
        }

        //�ړ���̏ꏊ�̐ݒ�
        Vector3 pos = next.transform.position;
        pos.z = -5;

        //�ړ����I���܂ŏ�����҂�
        await this.gameObject.transform.DOMove(pos, speed)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();

        //���ݒn���X�V
        nowmass = tm.GetNowMass(this.gameObject);

        //���َq�ϐ���null����Ȃ� && ���g�̎q�I�u�W�F�N�g��0�ȏ�
        // = �ړ����邨�َq������
        if (sweets != null && this.gameObject.transform.childCount != 0)
        {
            //���َq������Ƃ�
            if (sweetsscript != null && beyond != null) sweetsscript.MakeSweets(beyond.gameObject);
            //���Ȃ��Ƃ�
            else
            {
                //���َq�I�u�W�F�N�g�̐e��������
                sweets.transform.SetParent(sm.gameObject.transform);
                sweets = null;
            }

            /*�H�������ЂƂ��炷*/
            Debug.Log("decrease remaining num");
        }

        //���َq�̈ʒu���X�V
        sm.SearchSweets();

        //�N���A�`�F�b�N
        cc.ClearCheck((Vector2)this.transform.position);

        //�����t���O�X�V
        inProcess = false;
    }

    /// <summary>
    /// ���َq���H�ׂ�邩�`�F�b�N����֐�
    /// </summary>
    /// <param name="dire"></param> Direction = �����Ă������
    private void TryEat(Direction dire)
    {
        if (!inProcess) inProcess = true;

        //�����Ă����������ʒu�֌WVector2���擾
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

        //����null�`�F�b�N
        if (original == Vector2.zero)
        {
            inProcess = false;
            return;
        }

        //�����Ă�������̎��̃}�X���擾
        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttile = nowtile.ReturnNextMass(original);

        //�}�X��null�`�F�b�N
        if (nexttile == null)
        {
            inProcess = false;
            return;
        }

        //���̃}�X�ɂ��邨�َq�̃X�N���v�g���擾
        Sweets eatnext = sm.GetSweets(nexttile.transform.position);

        //���َq�X�N���v�g��null or �H�ׂ�Ȃ����َq �Ȃ�
        if (eatnext == null)
        {
            inProcess = false;
            return;
        }

        if (eatnext.canEat)
        {
            //���َq��H�ׂ�
            eatnext.EatSweets();

            //�H���Q�[�W�̑���
            Debug.Log("food gauge is increase");
        }
        else Debug.Log("this food can not eat");


        //�����t���O�̍X�V
        inProcess = false;
    }

    // Update is called once per frame
    void Update()
    {
        //���[�U�[���͂��󂯎��
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();        //�ړ����͒l
        float xvalue = actions.Player.SweetsMove.ReadValue<float>();    //X or KeyCode.Shift
        float avalue = actions.Player.Eat.ReadValue<float>();           //A or KeyCode.C

        //�ړ�
        if (vec2 != Vector2.zero && !inProcess)
        {
            CheckDirection(vec2, xvalue);
        }
        //�H�ׂ�
        else if (!inProcess && avalue > 0.5f)
        {
            TryEat(direction);
        }
        

    }
}
