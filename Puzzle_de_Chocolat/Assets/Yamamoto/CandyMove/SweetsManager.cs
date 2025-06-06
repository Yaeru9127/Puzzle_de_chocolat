using System.Collections.Generic;
using UnityEngine;

public class SweetsManager : MonoBehaviour
{
    public static SweetsManager sm { get; private set; }

    //���َq�I�u�W�F�N�g�i�[��Dictionary<���W, �X�N���v�g>
    public Dictionary<Vector2, Sweets> sweets = new Dictionary<Vector2, Sweets>();

    private void Awake()
    {
        if (sm == null) sm = this;
        else Destroy(sm);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
