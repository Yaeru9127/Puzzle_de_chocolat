using UnityEngine;

public class Trap : MonoBehaviour
{
    //�g���b�v�̎��
    //����A��������\��������̂�enum�Őݒ�
    public enum Type
    {
        FrischeSahne,       //���N���[��
        Test
    }
    public Type type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// �ȉ��A�g���b�v�𓥂񂾎��̏���
    /// </summary>
    
    //���N���[��
    public void CaseFrischeSahne()
    {
        Debug.Log("in CaseFrischeSahne()");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
