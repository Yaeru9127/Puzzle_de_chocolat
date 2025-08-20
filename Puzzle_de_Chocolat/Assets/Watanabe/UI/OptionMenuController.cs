using UnityEngine;

public class OptionMenuController : MonoBehaviour
{
    [SerializeField] private GameObject optionMenuUI; // オプションメニューのルート
    [SerializeField] private UIButtonOutlineSelector selector; // 参照しておく

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // 例: Escでオプション開閉
        {
            ToggleOptionMenu();
        }
    }

    public void ToggleOptionMenu()
    {
        isOpen = !isOpen;
        optionMenuUI.SetActive(isOpen);

        if (isOpen)
        {
            selector.AddOptionButtons(optionMenuUI);
            Time.timeScale = 0f; // ポーズするならここで
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
