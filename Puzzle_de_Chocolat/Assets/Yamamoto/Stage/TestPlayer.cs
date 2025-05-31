using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;

public class TestPlayer : MonoBehaviour
{
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

    public GameObject nowmass;


    private bool isMove;

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

        isMove = false;
    }

    //���ݒn���擾����֐�
    private void GetNowMass()
    {
        //�����蔻��Ŏ擾
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(0.1f, 0.1f), 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject)
            {
                nowmass = collider.gameObject;
            }
        }
    }

    //���͒l����������Z�o����֐�
    private void CheckDirection(Vector2 dir)
    {
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

        //�f�o�b�O
        Debug.Log(direction);
    }

    //���̃}�X���擾����֐�
    private async UniTask GetNextMass()
    {
        Tile tilescript = nowmass.GetComponent<Tile>();
        await MoveMass(tilescript.ReturnNextMass(direction.ToString()));
    }

    private async UniTask MoveMass(GameObject next)
    {
        Vector3 pos = next.transform.position;
        pos.z = -5;

        //�ړ�����������܂ő҂�
        this.gameObject.transform.DOMove(pos, 1.5f);
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
        Vector2 vec2 = actions.Player.Move.ReadValue<Vector2>();
        if (vec2 != Vector2.zero)
        {
            CheckDirection(vec2);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMove = true;
            GetNextMass();
        }
    }
}
