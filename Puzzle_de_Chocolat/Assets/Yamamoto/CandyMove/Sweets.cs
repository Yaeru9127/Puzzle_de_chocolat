using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Sweets : MonoBehaviour
{
    /*����
     *���َq�͐e�I�u�W�F�N�g��SweetsParent�ɐݒ肵�Đ�������*/

    private SweetsManager sm;
    private TileManager tm;

    //���َq�ޗ�enum
    public enum Material
    {
        Butter,
        Sugar,
        Egg,
        Milk
    }
    public Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sm = SweetsManager.sm;
        tm = TileManager.tm;
    }

    /// <summary>
    /// ���َq�����邩�`�F�b�N����֐�
    /// </summary>
    public GameObject TryMake()
    {
        Debug.Log("in TryMake");
        Sweets sweets = null;   //�ړ���̂��َq�̃X�N���v�g
        GameObject movedsweets = null;

        GameObject now = tm.GetNowMass(this.gameObject);    //������}�X��T��

        //�ړ���̃}�X�ɕʂ̂��َq�����邩���ׂ�
        foreach (KeyValuePair<Vector2, Sweets> pair in sm.sweets)
        {
            //�A�^�b�`����Ă���X�N���v�g���Ⴄ && ���W������
            if ((this.gameObject.GetComponent<Sweets>() != pair.Value) && ((Vector2)this.transform.position == pair.Key))
            {
                sweets = pair.Value;
                movedsweets = sweets.gameObject;
            }
        }

        //�ړ���̃}�X�ɂ��َq���Ȃ��ꍇ
        if (sweets == null)
        {
            return movedsweets;
        }
        //�ړ���̃}�X�ɂ��َq������ꍇ
        else
        {
            //�ޗ����r���č��邩�ǂ��������߂�
            /*��r�����͍���̃��V�s�̑����ɂ���ĕς��*/
            switch (material)
            {
                case Material.Butter:
                    if (sweets.material == Material.Butter)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
                case Material.Sugar:
                    if (sweets.material == Material.Sugar)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
                case Material.Egg:
                    if (sweets.material == Material.Egg)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
                case Material.Milk:
                    if (sweets.material == Material.Milk)
                    {
                        movedsweets = null;
                        return movedsweets;
                    }
                    break;
            }
        }

        return movedsweets;
    }

    /// <summary>
    /// ���َq�����֐�
    /// </summary>
    /// <param name="comparison"></param> �ړ���̂��َq�̃I�u�W�F�N�g
    public void MakeSweets(GameObject comparison)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
