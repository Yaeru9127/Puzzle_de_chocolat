using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIButtonOutlineSelector : MonoBehaviour
{
    // ���̃I�u�W�F�N�g�̎q���ɂ���Button�R���|�[�l���g��S�Ċi�[���郊�X�g
    private List<Button> buttons = new List<Button>();

    void Start()    
    {
        // �q�I�u�W�F�N�g����Button�R���|�[�l���g��S�Ď擾
        buttons.AddRange(GetComponentsInChildren<Button>());

        // �{�^����1�ȏ゠��΁A�ŏ��̃{�^����I����Ԃɂ��ăA�E�g���C����L���ɂ���
        if (buttons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            SetOutline(buttons[0], true);
        }
    }

    void Update()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        foreach (var btn in buttons)
        {
            bool isSelected = (btn.gameObject == selectedObj);
            SetOutline(btn, isSelected);
        }
    }

    // �I�v�V�������j���[�̃{�^����ǉ��o�^����֐�
    public void AddOptionButtons(GameObject optionMenuRoot)
    {
        Button[] optionButtons = optionMenuRoot.GetComponentsInChildren<Button>(true); // ��A�N�e�B�u�ł��擾
        foreach (var btn in optionButtons)
        {
            if (!buttons.Contains(btn)) // �d���o�^��h��
            {
                buttons.Add(btn);
            }
        }
    }

    void SetOutline(Button btn, bool enable)
    {
        Outline outline = btn.GetComponent<Outline>();

        if (outline == null)
        {
            outline = btn.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;
            outline.effectDistance = new Vector2(3, 3);
        }

        outline.enabled = enable;
    }
}
