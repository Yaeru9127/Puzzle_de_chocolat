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

    public bool canmake;        //���َq�̍���t���O

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //������
        sm = SweetsManager.sm;
        tm = TileManager.tm;
        canmake = false;
    }

    /// <summary>
    /// ���َq�����邩�`�F�b�N����֐�
    /// </summary>
    /// <param name="comparison"></param> ��r����X�N���v�g
    public bool TryMake(Sweets comparison)
    {
        Debug.Log("in TryMake");

        //�ޗ����r���č��邩�ǂ��������߂�
        //�i�ޗ���������������false�̂܂�return�j

        /*��r�����͍���̃��V�s�̑����ɂ���ĕς��*/
        //�o�^�[
        if (material == Material.Butter && comparison.material == Material.Butter) return false;
        //����
        else if (material == Material.Sugar && comparison.material == Material.Sugar) return false;
        //��
        else if (material == Material.Egg && comparison.material == Material.Egg) return false;
        //����
        else if (material == Material.Milk && comparison.material == Material.Milk) return false;


        //�����܂ł���Ƃ������Ƃ͍ޗ����Ⴄ���̂ł���Ƃ�������
        return true;
    }

    /// <summary>
    /// ���َq�����֐�
    /// </summary>
    /// <param name="comparison"></param> �ړ���̂��َq�̃I�u�W�F�N�g
    public void MakeSweets(GameObject comparison)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canmake) return;

        /*���َq�̍��̏���*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
