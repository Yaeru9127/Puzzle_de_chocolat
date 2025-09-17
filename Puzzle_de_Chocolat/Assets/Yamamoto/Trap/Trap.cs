using UnityEngine;

public class Trap : MonoBehaviour
{
    //�g���b�v�̎��
    //����A��������\��������̂�enum�Őݒ�
    public enum Type
    {
        FrischeSahne,       //���N���[��
        Test
    }
    public Type type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPosition();
    }

    /// <summary>
    /// �����ʒu��ݒ肷��֐�
    /// </summary>
    private void SetPosition()
    {
        //Ray�Ŏ��g������}�X��T��
        Vector3 origin = this.gameObject.transform.position;
        origin.z -= 1;
        Collider2D[] col = Physics2D.OverlapPointAll(origin);

        //�q�b�g����������}�X��T��
        foreach (Collider2D hit in col)
        {
            Tile tile = hit.gameObject.GetComponent<Tile>();
            if (tile == null) continue; //�}�X�X�N���v�g�������ĂȂ������玟��

            //�}�X����������
            if (tile != null)
            {
                //�ʒu�̒���
                Vector3 pos = tile.transform.position;
                pos.z = -3;
                this.gameObject.transform.position = pos;

                //�f�o�b�O
                //Debug.Log($"mass : {tile.gameObject.transform.position}");
                //Debug.Log($"trap : {this.gameObject.transform.position}");
                break;
            }
        }
    }

    /// <summary>
    /// �ȉ��A�g���b�v�𓥂񂾎��̏���
    /// </summary>

    //���N���[��
    public void CaseFrischeSahne()
    {
        Debug.Log("in CaseFrischeSahne()");
    }
}
