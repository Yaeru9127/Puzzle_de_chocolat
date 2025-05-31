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

    private void GetMass()
    {

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
