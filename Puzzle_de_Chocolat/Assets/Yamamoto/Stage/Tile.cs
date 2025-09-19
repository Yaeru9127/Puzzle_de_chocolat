using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Tile : MonoBehaviour
{
    //�ׂ̃}�X�Ƃ���"�ʒu�֌W"��Dictionary
    public Dictionary<GameObject, Vector2> neighbor = new Dictionary<GameObject, Vector2>();
    //�ʒu�֌W�̂��߂̔z��
    private Vector2[] direction = new Vector2[]
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    [SerializeField] private Sprite hibi;   //�Ђу}�XSprite
    public bool canBreak;                   //����}�X�ݒ�ϐ�

    /// <summary>
    /// �אڂ���}�X�̎擾�֐�
    /// </summary>
    public void GetNeighborTiles()
    {
        //������
        neighbor.Clear();

        foreach (var dir in direction)
        {
            //�����蔻��Ŏ擾
            Vector2 distance = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;

            //���g�̏ꏊ + SpriteRenderer�̃T�C�Y * Vector2�̏㉺���E���� = �����蔻��|�C���g
            Vector2 center = (Vector2)this.gameObject.transform.position + distance * dir;
            Collider2D[] hitobj = Physics2D.OverlapPointAll(center);

            foreach (var hitobj2 in hitobj)
            {
                //�����蔻��̃|�C���g�ɂ���I�u�W�F�N�g���i�[
                if (hitobj2 != null && hitobj2.gameObject != this.gameObject && hitobj2.GetComponent<Tile>())
                {
                    //�f�o�b�O
                    //Debug.Log($"{this.gameObject.name}, {hitobj.gameObject.name}");

                    //�d�����Ă��Ȃ�������ǉ�
                    if (!neighbor.ContainsKey(hitobj2.gameObject)) neighbor.Add(hitobj2.gameObject, dir);
                }
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

    /// <summary>
    /// ���̃}�X��Ԃ��֐�
    /// </summary>
    /// <param name="pos"></param> �ʒu�֌W�̕ϐ�
    /// <returns></returns>
    public GameObject ReturnNextMass(Vector2 pos)
    {
        GameObject mass = null;
        
        //�ʒu�֌W����I�u�W�F�N�g��T��
        foreach (KeyValuePair<GameObject, Vector2> pair in neighbor)
        {
            if (pair.Key == null) continue;
            //�ʒu�֌W����v�����Ƃ��̃I�u�W�F�N�g
            if (pair.Key.GetComponent<Tile>() && pair.Value == pos && pair.Key != null)
            {
                //�f�o�b�O
                //Debug.Log($"{this.gameObject.name} : {pair.Key.name} , {pair.Value}");
                mass = pair.Key;
                break;
            }
        }

        /*//�f�o�b�O
        if (mass == null) Debug.Log("mass is null");
        else Debug.Log(mass.name);*/
        return mass;
    }

    /// <summary>
    /// �}�X�̂Ђѓ���A�}�X�̍폜�֐�
    /// </summary>
    public async UniTask ChangeSprite()
    {
        //���Ȃ��}�X�Ȃ�return
        if (!canBreak) return;

        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();

        //Sprite���Ђу}�X�łȂ��Ȃ�A�Ђу}�X�ɐݒ�
        if (renderer.sprite != hibi && renderer != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = hibi;
            //Debug.Log("log");
        }
        //Sprite���Ђу}�X�Ȃ�
        else if (renderer.sprite == hibi)
        {
            Destroy(this.gameObject);
            await UniTask.NextFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
