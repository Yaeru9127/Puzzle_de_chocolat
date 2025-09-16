using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager tm {  get; private set; }

    //�}�X�I�u�W�F�N�g�i�[��Dictionary(�I�u�W�F�N�g, "�ʒu�֌W")
    public Dictionary<GameObject, Vector2> tiles = new Dictionary<GameObject, Vector2>();

    private void Awake()
    {
        if (tm == null) tm = this;
        else if (tm != null) Destroy(this.gameObject);
        GetAllMass();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// ���ׂẴ}�X���擾����֐�
    /// </summary>
    public void GetAllMass()
    {
        //������
        tiles.Clear();

        //���g�̎q�I�u�W�F�N�g�̒�����Tile�X�N���v�g�����I�u�W�F�N�g��T��
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<Tile>())
            {
                //�}�X���X�g�ɒǉ�
                Tile tile = this.gameObject.transform.GetChild(i).GetComponent<Tile>(); ;
                tiles.Add(this.gameObject.transform.GetChild(i).gameObject, tile.gameObject.transform.position);

                //�אڃ}�X���擾
                tile.GetNeighborTiles();
            }
        }

        ////�f�o�b�O
        //foreach (KeyValuePair<GameObject, Vector2> dictionary in tiles)
        //{
        //    Debug.Log($"{dictionary.Key.name}, {dictionary.Value}");
        //}
    }

    /// <summary>
    /// ������}�X���擾����֐�
    /// </summary>
    /// <param name="obj"></param> ��r����I�u�W�F�N�g
    public GameObject GetNowMass(GameObject obj)
    {
        //Debug.Log("GetNowMass() called");
        //Debug.Log(obj.name +" : " + obj.transform.position);
        GameObject nowmass = null;

        //�I�u�W�F�N�g�̍��W����v���Ă��邩
        foreach (KeyValuePair<GameObject, Vector2> pair in tiles)
        {
            //Debug.Log(pair.Key.transform.position);
            if((Vector2)obj.transform.position == (Vector2)pair.Key.transform.position)
            {
                nowmass = pair.Key;
            }
        }

        return nowmass;
    }

    private void OnDestroy()
    {
        //�V�[�����ׂ��Ƃ��Ƀ������������
        if (tm == this) tm = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
