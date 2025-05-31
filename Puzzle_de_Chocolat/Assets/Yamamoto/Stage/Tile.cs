using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //�ׂ̃}�X�Ƃ���"�ʒu�֌W"��Dictionary
    private static readonly Dictionary<GameObject, Vector2> neighbor = new Dictionary<GameObject, Vector2>();
    private Vector2 tilepos;        //�}�X�̍��W
                                    //�ʒu�֌W�̂��߂̔z��
    Vector2[] direction = new Vector2[]
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    private void Awake()
    {
        tilepos = this.gameObject.transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNeighborTiles();
    }

    //�אڂ���}�X�̎擾�֐�
    private void GetNeighborTiles()
    {
        foreach (var dir in direction)
        {
            //�����蔻��Ŏ擾
            Vector2 check = (Vector2)this.gameObject.transform.position;
            Collider2D[] hits = Physics2D.OverlapBoxAll(check,
                this.gameObject.GetComponent<SpriteRenderer>().bounds.size, 0);


            foreach (var hit in hits)
            {
                if (hit != null && hit.gameObject != this.gameObject && !neighbor.ContainsKey(hit.gameObject))
                {
                    if ((Mathf.Abs(this.gameObject.transform.position.x) == Mathf.Abs(hit.gameObject.transform.position.x)) || (Mathf.Abs(this.gameObject.transform.position.y) == Mathf.Abs(hit.gameObject.transform.position.y)))
                    {
                        Debug.Log($"{this.gameObject.name}, {hit.gameObject.name}");
                        neighbor.Add(hit.gameObject, dir);
                    }
                }
            }
        }

        //�f�o�b�O
        //foreach (var nei in neighbor.Values)
        //{
        //    Debug.Log($"{this.gameObject.name}, {nei}");
        //}
        foreach (var nei in neighbor.Keys)
        {
            Debug.Log($"{this.gameObject.name}, {nei.name}");
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 size = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.gameObject.transform.position, size);
    }

    //�}�X��Position��Ԃ��֐�
    public Vector2 GetTilePos()
    {
        return tilepos;
    }

    //���̃}�X��Ԃ��֐�
    public GameObject ReturnNextMass(string direction)
    {
        Vector2 target = Vector2.zero;
        GameObject mass = null;

        //�����ɕϊ�
        switch (direction)
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
            return mass;
        }

        //�����ƈʒu�֌W����I�u�W�F�N�g��T��
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            if (pair.Value == target)
            {
                mass = pair.Key;
                break;
            }
        }

        //�f�o�b�O
        if (mass == null) Debug.Log("mass is null");
        else Debug.Log(mass.name);
        return mass;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
