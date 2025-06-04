using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager tm {  get; private set; }
    public Dictionary<GameObject, Vector2> tiles = new Dictionary<GameObject, Vector2>();


    private void Awake()
    {
        if (tm == null)
        {
            tm = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        GetAllMass();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    /// <summary>
    /// �ڂ̑O�̃}�X�ɂ��邨�َq�̗L�����擾����֐�
    /// </summary>
    /// <param name="center"></param>    �T���}�X�̃I�u�W�F�N�g
    /// <returns></returns>
    public GameObject SearchSweets(GameObject center)
    {
        GameObject sweets = null;

        //�}�X�̍��W�ɓ����蔻���ݒu
        Collider2D[] hits = Physics2D.OverlapPointAll((Vector2)center.transform.position);
        foreach (Collider2D hitobj in hits)
        {
            //���َq�I�u�W�F�N�g����������
            if (hitobj.gameObject.GetComponent<Sweets>())
            {
                sweets = hitobj.gameObject;
                break;
            }
        }

        /*//�f�o�b�O
        if (sweets == null) Debug.Log("sweets is null");
        else Debug.Log("sweets is not null");*/

        return sweets;
    }

    //���ׂẴ}�X���擾����֐�
    public void GetAllMass()
    {
        //������
        tiles.Clear();

        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            Tile tile = this.gameObject.transform.GetChild(i).GetComponent<Tile>(); ;
            tiles.Add(this.gameObject.transform.GetChild(i).gameObject, tile.GetTilePos());
        }

        /*//�f�o�b�O�p
        foreach (KeyValuePair<GameObject, Vector2> dictionary in tiles)
        {
            Debug.Log($"{dictionary.Key.name}, {dictionary.Value}");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
