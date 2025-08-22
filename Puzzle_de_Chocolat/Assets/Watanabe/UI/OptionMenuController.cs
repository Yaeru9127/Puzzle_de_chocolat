using UnityEngine;

public class OptionMenuController : MonoBehaviour
{
    [SerializeField] private GameObject optionMenuUI; // �I�v�V�������j���[�̃��[�g
    [SerializeField] private UIButtonOutlineSelector selector; // �Q�Ƃ��Ă���

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ��: Esc�ŃI�v�V�����J��
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
            Time.timeScale = 0f; // �|�[�Y����Ȃ炱����
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
