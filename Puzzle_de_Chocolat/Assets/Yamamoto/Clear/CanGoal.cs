using System.Collections.Generic;
using UnityEngine;

public class CanGoal : MonoBehaviour
{
    private TileManager tm;
    private SweetsManager sm;

    [SerializeField] private TestPlayer playerscript;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject check;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tm = TileManager.tm;
        sm = SweetsManager.sm;
    }

    /// <summary>
    /// �S�[���ł��邩�𔻒肷��֐�
    /// </summary>
    /// <param name="now"></param> ��ƂȂ�}�X�̃X�N���v�g
    private void CanMassThrough(Tile now, GameObject test)
    {
        foreach (KeyValuePair<GameObject, Vector2> pair in tm.tiles)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        //�e�X�g�p
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject ob = Instantiate(check);
            CanMassThrough(playerscript.ReturnNowTileScript(), ob);
        }
    }
}
