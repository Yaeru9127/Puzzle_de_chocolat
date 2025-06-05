using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager tm {  get; private set; }

    //�}�X�I�u�W�F�N�g�i�[��Dictionary(�I�u�W�F�N�g, "�ʒu�֌W")
    public Dictionary<GameObject, Vector2> tiles = new Dictionary<GameObject, Vector2>();

    //���َq�I�u�W�F�N�g�i�[��Dictionary
    public Dictionary<Vector2, Sweets> sweets = new Dictionary<Vector2, Sweets>();

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

        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetAllMass();
        SearchSweets();
    }

    /// <summary>
    /// ���َq�̔z��̒�����ړI�̂��َq�������o���֐�
    /// </summary>
    /// <param name="pos"></param> �T���}�X�̍��W
    public Sweets GetSweets(Vector2 pos)
    {
        Sweets returnsweetts = null;
        foreach (Vector2 sweetspos in sweets.Keys)
        {
            if (pos == sweetspos) returnsweetts = sweets[sweetspos];
        }

        return returnsweetts;
    }

    /// <summary>
    /// �}�X��̂��ׂĂ̂��َq���擾����֐�
    /// </summary>
    /// <returns></returns>
    public void SearchSweets()
    {
        sweets.Clear();

        //���g�̎q�I�u�W�F�N�g�̒�����Sweets�X�N���v�g�����I�u�W�F�N�g��T��
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<Sweets>())
            {
                sweets.Add(this.gameObject.transform.GetChild(i).gameObject.transform.position, this.gameObject.transform.GetChild(i).gameObject.GetComponent<Sweets>());
            }
        }

        /*//�f�o�b�O
        foreach (var sw in sweets)
        {
            Debug.Log($"Key : {sw.Key} , Value : {sw.Value.gameObject.name}");
        }*/
    }

    //���ׂẴ}�X���擾����֐�
    public void GetAllMass()
    {
        //������
        tiles.Clear();

        //���g�̎q�I�u�W�F�N�g�̒�����Tile�X�N���v�g�����I�u�W�F�N�g��T��
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<Tile>())
            {
                Tile tile = this.gameObject.transform.GetChild(i).GetComponent<Tile>(); ;
                tiles.Add(this.gameObject.transform.GetChild(i).gameObject, tile.gameObject.transform.position);
            }
        }

        /*//�f�o�b�O
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
