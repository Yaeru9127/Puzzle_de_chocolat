using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private TileManager tm = TileManager.tm;

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
        GetNowMass();

        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();
        actions = inputmanager.GetActions();
        inputmanager.PlayerOn();
        speed = 0.5f;
        onMove = false;
    }

    //���ݒn���擾����֐�
    private void GetNowMass()
    {
        //�����蔻��Ŏ擾
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(0.1f, 0.1f), 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject && collider.GetComponent<Tile>())
            {
                nowmass = collider.gameObject;
            }
        }
    }

    /// <summary>
    /// �����ݒ�֐�
    /// </summary>
    /// <param name="directionstring"></param> enum��string�ɕς���
    /// <returns></returns>
    private Vector2 SetDirection(string directionstring)
    {
        Vector2 target = Vector2.zero;

        //�����ɕϊ�
        switch (directionstring)
        {
            case "Up": target = Vector2.up; break;
            case "Down": target = Vector2.down; break;
            case "Left": target = Vector2.left; break;
            case "Right": target = Vector2.right; break;
            default: target = Vector2.zero; break;
        }

        //null�`�F�b�N
        if (target == Vector2.zero)
        {
            Debug.Log("direction is missing!!");
        }

        return target;
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
        else
        {
            //y�̒l���傫�� => ��
            if (dir.y > 0) directo = Vector2.up;

            //y�̒l�������� => ��
            else directo = Vector2.down;
        }

        Tile nowtile = nowmass.GetComponent<Tile>();
        GameObject nexttileobj;
        nexttileobj = nowtile.ReturnNextMass(directo);
        TryMove(nexttileobj, xbutton);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param> �ړ���̃}�X�̍��W
    /// <param name="X"></param>    X or Shift�̃{�^���̉����Ă���l
    private void TryMove(GameObject next, float X)
    {
        //���̃}�X�ɂ��邨�َq���擾
        GameObject sweets = tm.GetSweets(next.transform.position).gameObject;

        //���̃}�X�ɂ��َq����������
        if (sweets != null)
        {
            //X or Shift�������Ă�����
            if (X > 0.5f)
            {
                sweets.transform.SetParent(this.gameObject.transform);
            }
            //X or Shift�������Ă��Ȃ�������
            else
            {
                onMove = false;
                return;
            }
        }

        MoveMass(next);
    }

    /// <summary>
    /// �ړ��֐�
    /// </summary>
    /// <param name="next"></param>   �ړ���̃}�X�I�u�W�F�N�g
    private void MoveMass(GameObject next)
    {
        Vector3 pos = next.transform.position;
        pos.z = -5;

        this.gameObject.transform.DOMove(pos, speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            //���ݒn���X�V
            GetNowMass();

            //�ړ��t���O�X�V
            onMove = false;
        });
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
