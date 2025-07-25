using UnityEngine;
using UnityEngine.EventSystems;

public class UISelector : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null); // ��U���Z�b�g
        EventSystem.current.SetSelectedGameObject(firstSelected); // �ŏ���UI��I��
    }
}
