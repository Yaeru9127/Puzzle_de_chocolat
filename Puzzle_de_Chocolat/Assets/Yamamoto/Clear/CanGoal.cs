using System.Collections.Generic;
using UnityEngine;

public class CanGoal : MonoBehaviour
{
    private TileManager tm;
    private SweetsManager sm;

    [SerializeField] private TestPlayer playerscript;
    [SerializeField] private GameObject goal;
    [SerializeField] private List<GameObject> searched = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        tm = TileManager.tm;
        sm = SweetsManager.sm;
        searched.Clear();
    }

    /// <summary>
    /// �S�[���ł��邩�𔻒肷��֐�
    /// </summary>
    /// <param name="now"></param> ��ƂȂ�}�X�̃X�N���v�g
    /// ���َq���ړ������Ďc��ړ�����0�ɂȂ����Ƃ��ɂ��̊֐������s����
    private bool CanMassThrough(Tile now)
    {
        //�����ς݃��X�g�ɏd������������return
        if (searched.Contains(now.gameObject)) return false;

        //���X�g�ɒǉ�
        searched.Add(now.gameObject);

        //��}�X�̗אڃ}�X������
        foreach (KeyValuePair<GameObject, Vector2> pair in now.neighbor)
        {
            //�אڃ}�X�ɂ��َq���������玟�̃}�X��
            Sweets sweets = sm.GetSweets(pair.Key.transform.position);
            if (sweets != null) continue;

            //���َq�̂Ȃ��אڃ}�X���S�[���}�X�Ȃ�
            if (pair.Key == goal)
            {
                //�f�o�b�O
                //Debug.Log($"serachmass : {now.gameObject.name}  goal : {pair.Key.name}");

                return true;
            }

            //�אڃ}�X�̗אڃ}�X������
            if (CanMassThrough(pair.Key.GetComponent<Tile>())) return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //�e�X�g�p
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log($"serach is end : {CanMassThrough(playerscript.ReturnNowTileScript())}");
        }
    }
}
