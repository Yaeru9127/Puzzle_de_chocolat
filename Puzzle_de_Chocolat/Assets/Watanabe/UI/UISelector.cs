using UnityEngine;
using UnityEngine.EventSystems;

public class UISelector : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null); // 一旦リセット
        EventSystem.current.SetSelectedGameObject(firstSelected); // 最初のUIを選択
    }
}
