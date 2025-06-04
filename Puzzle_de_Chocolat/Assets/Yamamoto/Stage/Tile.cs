using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //�ׂ̃}�X�Ƃ���"�ʒu�֌W"��Dictionary
    public Dictionary<GameObject, Vector2> neighbor = new Dictionary<GameObject, Vector2>();
    private Vector2 tilepos;        //�}�X�̍��W
                                    //�ʒu�֌W�̂��߂̔z��
    private Vector2[] direction = new Vector2[]
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
            Vector2 distance = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;

            //���g�̏ꏊ + SpriteRenderer�̃T�C�Y * Vector2�̏㉺���E���� = �����蔻��|�C���g
            Vector2 center = (Vector2)this.gameObject.transform.position + distance * dir;
            Collider2D hitobj = Physics2D.OverlapPoint(center);

            //�����蔻��̃|�C���g�ɂ���I�u�W�F�N�g���i�[
            if (hitobj != null && hitobj.gameObject != this.gameObject)
            {
                //�f�o�b�O
                //Debug.Log($"{this.gameObject.name}, {hitobj.gameObject.name}");

                //�d�����Ă��Ȃ�������ǉ�
                if (!neighbor.ContainsKey(hitobj.gameObject)) neighbor.Add(hitobj.gameObject, dir);

            }
        }

        /*//�f�o�b�O
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            Debug.Log($"{this.gameObject.name}: object => {pair.Key.name}, pos => {pair.Value}");
        }*/
    }

    /*//�f�o�b�O�i�����蔻��`��֐��j
    private void OnDrawGizmos()
    {
        Vector2 size = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.gameObject.transform.position, size);
    }*/

    //�}�X��Position��Ԃ��֐�
    public Vector2 GetTilePos()
    {
        return tilepos;
    }

    /// <summary>
    /// ���̃}�X��Ԃ��֐�
    /// </summary>
    /// <param name="pos"></param> �ʒu�֌W�̕ϐ�
    /// <returns></returns>
    public GameObject ReturnNextMass(Vector2 pos)
    {
        GameObject mass = null;

        //�����ƈʒu�֌W����I�u�W�F�N�g��T��
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            if (pair.Value == pos)
            {
                //�f�o�b�O
                //Debug.Log($"{pair.Key.name} : {pair.Value}");
                mass = pair.Key;
                break;
            }
        }

        /*//�f�o�b�O
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            Debug.Log($"{this.gameObject.name} => {pair.Key.name} : {pair.Value}");
        }
        if (mass == null) Debug.Log("mass is null");
        else Debug.Log(mass.name);*/
        return mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
