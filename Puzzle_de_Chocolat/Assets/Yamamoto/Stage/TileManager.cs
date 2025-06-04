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
    /// <param name="center"></param>    ��ƂȂ�I�u�W�F�N�g
    /// <param name="direction"></param> �ʒu�֌W�̕ϐ�
    /// <returns></returns>
    public GameObject GetForwardMass(GameObject center, Vector2 direction)
    {
        GameObject sweets = null;
        Tile tilescript = center.GetComponent<Tile>();

        //�}�X�̈ʒu�֌WDictionary����אڃ}�X���擾
        foreach (KeyValuePair<GameObject, Vector2> pair in tilescript.neighbor)
        {
            //�ʒu�֌W����v���� && �܂����َq�������Ă��Ȃ�
            if (pair.Value == direction && sweets == null)
            {
                Vector2 distance = pair.Key.GetComponent<SpriteRenderer>().bounds.size;
                Collider2D[] hits = Physics2D.OverlapPointAll((Vector2)pair.Key.transform.position + distance * pair.Value);
                foreach (Collider2D hitobj in hits)
                {
                    if (hitobj.gameObject.GetComponent<Sweets>())
                    {
                        sweets = hitobj.gameObject;
                        break;
                    }
                }
            }
        }
        Debug.Log(sweets.name);
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
