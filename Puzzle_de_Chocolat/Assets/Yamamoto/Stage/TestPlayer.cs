using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    /// <summary>
    /// Other Scripts
    /// </summary>
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;

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
        RandomMassSet();
        GetNowMass();

        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();
        actions = inputmanager.GetActions();
        inputmanager.PlayerOn();
        speed = 1f;
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
    private void CheckDirection(Vector2 dir, bool xbutton)
    {
        onMove = true;

        //���E�̓��͕���������(���Ԃ�Ȃ��Ǝv������)
        if (Mathf.Abs(dir.x) == Mathf.Abs(dir.y))
        {
            //y�̓��͒l�Ŕ��f
            if ((dir.x > 0 && dir.y > 0) || (dir.x < 0 && dir.y > 0)) direction = Direction.Up;
            else direction = Direction.Down;
        }
        //x�̕���y���傫��
        else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            //x�̒l���傫�� => �E
            if (dir.x > 0) direction = Direction.Right;

            //x�̒l�������� => ��
            else direction = Direction.Left;
        }
        //y�̕���x���傫��
        else
        {
            //y�̒l���傫�� => ��
            if (dir.y > 0) direction = Direction.Up;

            //y�̒l�������� => ��
            else direction = Direction.Down;
        }

        Vector2 di = SetDirection(direction.ToString());
        Tile tile = nowmass.GetComponent<Tile>();

        //�ڂ̑O�̃}�X���擾
        GameObject next = tile.ReturnNextMass(di);
        /*//�f�o�b�O
        if (next == null) Debug.Log("next is null");
        else Debug.Log("next is not null");*/

        GameObject movemass;
        sweets = TileManager.tm.SearchSweets(next);

        //���َq���ڂ̑O�ɂȂ�
        if (sweets == null)
        {
            movemass = tile.ReturnNextMass(di);
        }
        //���َq���ڂ̑O�ɂ���
        else
        {
            //���َq�����g�̎q�I�u�W�F�N�g�ɐݒ�
            sweets.transform.SetParent(this.gameObject.transform);
            /*�����Ă���������t�̕����ɋ󂫃}�X�����邩*/
        }
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
            GetNowMass();
            onMove = false;
        });
    }

    //�e�X�g
    private void RandomMassSet()
    {
        int num = Random.Range(0, TileManager.tm.tiles.Count);
        KeyValuePair<GameObject, Vector2> pair = TileManager.tm.tiles.ElementAt(num);
        this.gameObject.transform.position = new Vector3(pair.Value.x, pair.Value.y, -5);
        nowmass = pair.Key;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();
        float xvalue = actions.Player.CandyMove.ReadValue<float>();
        if (vec2 != Vector2.zero && !onMove)
        {
            CheckDirection(vec2, xvalue > 0.7f);
        }
    }
}
