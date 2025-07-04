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

    //�v���C���[�������Ă������
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    /*directionSprites => 0:�� , 1:�� , 2:�� , 3:��*/
    [SerializeField] private Sprite[] directionSprites = new Sprite[4];

    [SerializeField] private GameObject controllers;
    private GameObject nowmass;         //������}�X
    private float speed;                //�}�X�Ԃ̈ړ����x
    private bool inProcess;             //�������t���O

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
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
    /// ���݂̃}�X�̃X�N���v�g��Ԃ��֐�
    /// </summary>
    /// <returns></returns>
    public Tile ReturnNowTileScript()
    {
        return nowmass.GetComponent<Tile>();
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
        Tile nowtile = ReturnNowTileScript();
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
        //���̃}�X�����݂��Ȃ��ꍇ
        else if (nexttileobj == null)
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

        GameObject newnextmass = null;     //�}�X�I�u�W�F�N�g
        Sweets nextnextsweets = null;      //���َq�X�N���v�g
        Sweets pairnextsweets = null;      //�y�A�̂��َq�X�N���v�g

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
                    /*�����َq�̐�̃}�X��T��*/
                    //���َq�̐�̃}�X���擾
                    newnextmass = next.GetComponent<Tile>().ReturnNextMass(dire);
                    
                    //2�}�X�̂��َq�̏ꍇ�̓y�A�̂��َq�̐�̃}�X���T��
                    if (sweetsscript.pair != null)
                    {
                        //�y�A�̂��َq�̃}�X���擾
                        GameObject pairmass = tm.GetNowMass(sweetsscript.pair);

                        //�y�A�̂��َq�̐�̃}�X���擾
                        GameObject pairnextmass = pairmass.GetComponent<Tile>().ReturnNextMass(dire);

                        //�y�A�̂��َq�̐�̃}�X����������
                        if (pairnextmass != null)
                        {
                            //�y�A�̂��َq�̐�̃}�X�ɂ��邨�َq��T��
                            foreach (KeyValuePair<Vector2, Sweets> sweetspair in sm.sweets)
                            {
                                //���W�Ō���
                                //�y�A�̂��َq�̐�̃}�X�ɂ��邨�َq�̍��W == �y�A�̂��َq�̐�̃}�X�̍��W
                                if (sweetspair.Key == (Vector2)pairmass.transform.position)
                                {
                                    //�y�A�̂��َq�ϐ��ɐݒ�
                                    pairnextsweets = sweetspair.Value;
                                }
                            }
                        }
                        //�y�A�̂��َq�̐�̃}�X���Ȃ�������
                        else
                        {
                            inProcess = false;
                            return;
                        }
                    }

                    //�ړ������邨�َq�̐�̃}�X��T��
                    foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
                    {
                        //���W�Ō���
                        //�ړ������邨�َq�̐�̃}�X������ &&
                        //�ړ������邨�َq�̐�̃}�X�ɂ��邨�َq�̍��W == �ړ������邨�َq�̐�̃}�X�̍��W
                        if (newnextmass != null && pair.Key == (Vector2)newnextmass.transform.position)
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
                    newnextmass = backmass;
                }

                //----------------------------------------------------
                //���َq�̐�̃}�X���Ȃ� or ���Ƀ}�X���Ȃ�
                if (newnextmass == null)
                {
                    inProcess = false;
                    return;
                }

                newnextmass = next;

                //���َq�����g�̎q�I�u�W�F�N�g�ɂ���
                sweetsscript.gameObject.transform.SetParent(this.gameObject.transform);
                if (sweetsscript.pair != null)
                {
                    //�ړ��p�ɐe�I�u�W�F�N�g��ݒ�
                    sweetsscript.pair.transform.SetParent(this.gameObject.transform);
                    Debug.Log(sweetsscript.pair.name);
                }
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
                newnextmass = next;
            }
        }
        else
        {
            newnextmass = next;
        }

        /*//���̃}�X�̃g���b�v���擾
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
            //�ړ��O�̒i�K�Ŏ��̃}�X�̃g���b�v����p����ꍇ�͂��̃g���b�v������
        }*/

        //�y�A�̂��َq�̃X�N���v�g���擾
        Sweets pairsweetsscript = null;
        if (sweetsscript != null && sweetsscript.pair != null && sweetsscript.pair.GetComponent<Sweets>())
        {
            pairsweetsscript = sweetsscript.pair.GetComponent<Sweets>();
        }

        MoveMass(newnextmass, sweetsscript, nextnextsweets, pairsweetsscript, pairnextsweets).Forget();
    }

    /// <summary>
    /// �ړ��֐�
    /// </summary>
    /// <param name="next"></param>         �ړ���̃}�X�I�u�W�F�N�g
    /// <param name="sweetsscript"></param> �ړ������邨�َq�X�N���v�g
    /// <param name="beyond"></param>       �ړ���̃}�X�ɂ��邨�َq�X�N���v�g
    /// 
    /// <param name="pairsweets"></param>   �ړ������邨�َq�̃y�A�X�N���v�g
    /// <param name="pairbeyond"></param>   �ړ������邨�َq�̈ړ���̃}�X�ɂ��邨�َq�X�N���v�g
    private async UniTask MoveMass(GameObject next, Sweets sweetsscript, Sweets beyond, Sweets pairsweets, Sweets pairbeyond)
    {
        //���g�̎q�I�u�W�F�N�g���擾
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Debug.Log(child.name);
            children.Add(child.gameObject);
        }

        //�ړ������邨�َq������ && �y�A�̂��َq�����݂���
        if (sweetsscript != null && pairsweets != null)
        {
            //�ړ���̃}�X�ɂ��َq�����݂��� && �y�A�̂��َq�̈ړ���ɂ��َq������
            //= 2�}�X�̂��َq�̏ꍇ�͈ړ����邱�Ƃ͂ł��Ȃ�
            if (beyond !=null && pairbeyond != null)
            {
                //�e�I�u�W�F�N�g�����Z�b�g
                ResetParent(children);

                inProcess = false;
                return;
            }
            //�y�A�̂��َq�̈ړ���ɂ��َq���Ȃ�
            //= ���َq���ړ��ł���
            else if (pairbeyond == null)
            {
                //�ړ���̃}�X�ɂ��َq������ && �ړ������邨�َq�ƈړ���̂��َq�ō��Ȃ�
                //= �ړ������Ȃ�
                if (beyond != null && !sweetsscript.TryMake(beyond))
                {
                    Debug.Log("can not make");

                    //�e�q�֌W�����Z�b�g
                    ResetParent(children);

                    inProcess = false;
                    return;
                }

                //�ړ��̂��߂Ƀy�A�̂��َq�̐e�I�u�W�F�N�g��ݒ�
                pairsweets.gameObject.transform.SetParent(this.gameObject.transform);
            }
        }
        //�ړ������邨�َq������ && �ړ���̃}�X�ɂ��َq�����݂��� && �y�A�̂��َq�����݂��Ȃ�
        else if (sweetsscript != null && beyond != null && pairsweets == null)
        {
            if (!sweetsscript.TryMake(beyond))
            {
                Debug.Log("can not make");

                //�e�q�֌W�����Z�b�g
                ResetParent(children);

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

        //���̃}�X�̂Ђу`�F�b�N
        ReturnNowTileScript().ChangeSprite();

        //���ݒn���X�V
        nowmass = tm.GetNowMass(this.gameObject);

        //�}�X�����X�V
        tm.GetAllMass();

        //���g�̎q�I�u�W�F�N�g��0�ȊO = �ړ����邨�َq������
        if (this.gameObject.transform.childCount != 0)
        {
            //���َq������Ƃ�
            //-> �ړ���ɂ��َq�����݂��Ă��ăy�A�̂��َq�����݂��Ă��Ȃ��Ƃ�
            if (sweetsscript != null && beyond != null && pairsweets == null)
            {
                sweetsscript.MakeSweets(beyond.gameObject);
            }
            //-> �ړ���ɂ��َq�����݂��Ă��Ȃ����y�A�̂��َq�̈ړ���ɂ��َq�����݂��Ă���Ƃ�
            else if (sweetsscript != null && beyond == null && pairsweets != null && pairbeyond != null)
            {
                pairsweets.MakeSweets(pairbeyond.gameObject);
            }

            //�e�q�֌W�����Z�b�g
            ResetParent(children);

            /*�c��H�������ЂƂ��炷*/
            //Debug.Log("decrease remaining num");
            remaining.ReduceLife();
        }

        //���َq�̈ʒu���X�V
        sm.SearchSweets();

        //�������N���[���𓥂񂾎��̏���   
        Collider2D[] col = Physics2D.OverlapPointAll(nowmass.transform.position);
        foreach (Collider2D col2 in col)
        {
            if (col2.gameObject.GetComponent<Trap>() && col2.gameObject.GetComponent<Trap>().type == Trap.Type.FrischeSahne)
            {
                remaining.ReduceLife();
            }
        }

        //�N���A�`�F�b�N
        //�c��ړ�����0�ȉ��������� = ����ȏ�ړ��ł��Ȃ���ԂȂ�
        if (remaining.currentLife <= 0)
        {
            //�����S�[���ł��Ȃ��Ȃ�AGameOver�̐ݒ�
            if (!cg.CanMassThrough(ReturnNowTileScript())) goc.ShowGameOver();
        }

        //�����t���O�X�V
        inProcess = false;
    }

    /// <summary>
    /// �e�q�֌W�����Z�b�g����֐�
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
            sm.CallDecreaseFoodGauge();
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
        float xvalue = actions.Player.SweetsMove.ReadValue<float>();    //GamePad.X or KeyCode.Shift
        float avalue = actions.Player.Eat.ReadValue<float>();           //GamePad.A or KeyCode.C
        float escape = actions.Player.Pause.ReadValue<float>();         //GamePad.Start or KeyCode.Escape

        //�ړ�
        if (!inProcess && vec2 != Vector2.zero)
        {
            CheckDirection(vec2, xvalue);
        }
        //�H�ׂ�
        else if (!inProcess && avalue > 0.5f)
        {
            TryEat(direction);
        }
        //�|�[�Y
        else if (!inProcess && escape > 0.5f)
        {
            if (pause == null) Debug.Log("pause is null");
            pause.SetPause();
        }
    }
}
