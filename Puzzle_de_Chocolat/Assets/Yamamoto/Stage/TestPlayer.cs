using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private TileManager tm;
    private SweetsManager sm;

    //�v���C���[�������Ă������
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;

    public GameObject nowmass;          //������}�X
    private float speed;                //�}�X�Ԃ̈ړ����x
    private bool onMove;
    private GameObject sweets;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tm = TileManager.tm;
        sm = SweetsManager.sm;
        nowmass = tm.GetNowMass(this.gameObject);

        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();
        actions = inputmanager.GetActions();
        inputmanager.PlayerOn();
        speed = 0.5f;
        onMove = false;
    }

    /// <summary>
    /// ���͒l����������Z�o����֐�
    /// </summary>
    /// <param name="dir"></param>     ���͒l
    /// <param name="xbutton"></param> X�{�^���������Ă��邩
    private void CheckDirection(Vector2 dir, float xbutton)
    {
        if (!onMove) onMove = true;
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
            onMove = false;
            return;
        }

        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttileobj = nowtile.ReturnNextMass(directo);

        //���̃}�X�����݂���ꍇ
        if (nexttileobj != null)
        {
            //�f�o�b�O
            //Debug.Log($"next mass is {nexttileobj}");
            TryMove(nexttileobj, xbutton, directo);
        }
        //���̃}�X�����݂��Ȃ��ꍇ
        else
        {
            Debug.Log($"next mass is null");
            onMove = false;
            return;
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
        GameObject movedsweets = null;

        //�ړ���̃}�X�ɂ��َq����������
        foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
        {
            if ((Vector2)next.transform.position == pair.Key)
            {
                onMove = false;
                return;
            }
        }

        //���̃}�X�ɂ��邨�َq���擾
        Sweets sweetsscript;
        sweetsscript = sm.GetSweets(next.transform.position);

        //�i�s�����ɂ��َq���Ȃ��ꍇ�A���̃}�X����������
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

            if (backmass != null)
            {
                //���̃}�X���擾��A���̃}�X�ɂ��َq�����邩��T��
                if (reverse != Vector2.zero)
                {
                    GameObject nextback = nowmass.GetComponent<Tile>().ReturnNextMass(reverse);
                    //���̃}�X�����݂���ꍇ
                    if (nextback != null)
                    {
                        //���̃}�X�ɂ��َq�����邩��T��
                        sweetsscript = sm.GetSweets(nextback.transform.position);
                    }
                }
                else //�������㉺���E�ł͂Ȃ��ꍇ�i���Ԃ�Ȃ����ǁj
                {
                    onMove = false;
                    return;
                }
            }
            
        }

        /*//�f�o�b�O
        if (sweetsscript != null) Debug.Log($"{sweetsscript.gameObject.name}");
        else Debug.Log("sweetsscript is null");*/

        Tile nextnexttile;
        GameObject nextnextmass = null;

        //�O�����̃}�X�ɂ��َq����������
        if (sweetsscript != null)
        {
            //X or Shift�������Ă�����
            if (X > 0.5f)
            {
                //���̃}�X�ϐ���null => ���̃}�X��T���Ă��Ȃ�
                if (backmass == null)
                {
                    //���َq�̐�̃}�X���擾
                    nextnexttile = next.GetComponent<Tile>();
                    nextnextmass = nextnexttile.ReturnNextMass(dire);

                    //���َq�̐�̃}�X�̂��َq���擾
                    movedsweets = sweets.GetComponent<Sweets>().TryMake();
                }
                //���̃}�X�ϐ���null�ȊO => ���̃}�X�ɂ��َq������
                else if (backmass != null)
                {
                    //�����̌��̃}�X���擾
                    nextnexttile = backmass.GetComponent<Tile>();
                    nextnextmass = backmass;
                }

                //���َq�̐�̃}�X���Ȃ� or ���Ƀ}�X���Ȃ�
                if (nextnextmass == null)
                {
                    onMove = false;
                    return;
                }

                nextnextmass = next;

                //���َq�����g�̎q�I�u�W�F�N�g�ɂ���
                sweets = sweetsscript.gameObject;
                sweets.transform.SetParent(this.gameObject.transform);
            }
            //X or Shift�������Ă��Ȃ� && ���َq�I�u�W�F�N�g���Ȃ�
            else if (X > 0.5f && sweetsscript == null)
            {
                Debug.Log("sweets is not null");
                onMove = false;
                return;
            }
            else
            {
                nextnextmass = next;
            }
        }
        else
        {
            nextnextmass = next;
        }

        MoveMass(nextnextmass, movedsweets).Forget();
    }

    /// <summary>
    /// �ړ��֐�
    /// </summary>
    /// <param name="next"></param>   �ړ���̃}�X�I�u�W�F�N�g
    private async UniTask MoveMass(GameObject next, GameObject canmake)
    {
        //"�ړ���̃}�X�̂��َq"��"�ړ������邨�َq"�ł��َq�����Ȃ��Ȃ�
        if (canmake == null)
        {
            onMove = false;
            return;
        }

        Vector3 pos = next.transform.position;
        pos.z = -5;

        await this.gameObject.transform.DOMove(pos, speed)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();

        //���ݒn���X�V
        nowmass = tm.GetNowMass(this.gameObject);

        if (sweets != null && this.gameObject.transform.childCount != 0)
        {
            //���َq������Ȃ���
            if (canmake != null)
            {
                sweets.GetComponent<Sweets>().MakeSweets(sweets);
            }
            //���َq�I�u�W�F�N�g�̐e��������
            sweets.transform.SetParent(sm.gameObject.transform);
            sweets = null;
        }

        //���َq�̈ʒu���X�V
        sm.SearchSweets();

        //�ړ��t���O�X�V
        onMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();
        float xvalue = actions.Player.CandyMove.ReadValue<float>();
        if (vec2 != Vector2.zero && !onMove)
        {
            CheckDirection(vec2, xvalue);
        }
    }
}
