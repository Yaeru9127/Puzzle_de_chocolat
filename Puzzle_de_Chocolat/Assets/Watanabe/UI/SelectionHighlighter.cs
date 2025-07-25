using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionHighlighter : MonoBehaviour
{
    private GameObject borderImage;

    void Start()
    {
        // �q�I�u�W�F�N�g�ɂ���uSelectionBorder�v�摜���擾���Ĕ�\����
        Transform border = transform.Find("SelectionBorder");
        if (border != null)
        {
            borderImage = border.gameObject;
            borderImage.SetActive(false);
        }
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (borderImage != null) borderImage.SetActive(true);
        }
        else
        {
            if (borderImage != null) borderImage.SetActive(false);
        }
    }
}
