using UnityEngine;

public class CursorController1 : MonoBehaviour
{
    //public static CursorController cc;

    [SerializeField] private GameObject cursorObject; // �J�[�\���\���𐧌䂷��I�u�W�F�N�g�i�C�Ӂj

    private void Awake()
    {
        //cc = this;
    }

    /// <summary>
    /// �J�[�\����L��/�����ɂ���
    /// </summary>
    public void ChangeCursorEnable(bool enable)
    {
        if (cursorObject != null)
        {
            cursorObject.SetActive(enable);
        }
        else
        {
            Debug.LogWarning("cursorObject �����ݒ�ł��B�C���X�y�N�^�[�Őݒ肵�Ă��������B");
        }
    }
}
