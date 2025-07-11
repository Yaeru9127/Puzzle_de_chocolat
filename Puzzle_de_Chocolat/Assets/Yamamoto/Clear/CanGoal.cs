using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanGoal : MonoBehaviour
{
    public static CanGoal cg {  get; private set; }

    private TileManager tm;
    private SweetsManager sm;

    [SerializeField] private TestPlayer playerscript;   //�v���C���[�ϐ�
    [SerializeField] private GameObject goal;           //�S�[���}�X�I�u�W�F�N�g

    //�S�[���ł��邩�̔��莞�Ɏg�������ς݊i�[�֐�
    private List<GameObject> searched = new List<GameObject>();

    private void Awake()
    {
        if (cg == null) cg = this;
        else if (cg != null) Destroy(cg);
    }

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
    /// �c��ړ�����0�ɂȂ����Ƃ��ɂ��̊֐������s����
    /// true = �S�[���ɓ��B�ł���  false = �S�[���ɓ��B�ł��Ȃ�
    public bool CanMassThrough(Tile now)
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

    private void OnDestroy()
    {
        //�V�[�����ׂ��Ƃ��Ƀ������������
        if (cg == this) cg = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
