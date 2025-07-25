using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionHighlighter : MonoBehaviour
{
    private GameObject borderImage;

    void Start()
    {
        // 子オブジェクトにある「SelectionBorder」画像を取得して非表示に
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
