using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIButtonOutlineSelector : MonoBehaviour
{
    // ���̃I�u�W�F�N�g�̎q���ɂ���Button�R���|�[�l���g��S�Ċi�[����z��
    private UnityEngine.UI.Button[] buttons;

    void Start()
    {
        // �q�I�u�W�F�N�g����Button�R���|�[�l���g��S�Ď擾
        buttons = GetComponentsInChildren<UnityEngine.UI.Button>();

        // �{�^����1�ȏ゠��΁A�ŏ��̃{�^����I����Ԃɂ��ăA�E�g���C����L���ɂ���
        if (buttons.Length > 0)
        {
            // EventSystem�ɍŏ��̃{�^����I����ԂƂ��ēo�^
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);

            // �ŏ��̃{�^���ɃA�E�g���C����t����
            SetOutline(buttons[0], true);
        }
    }

    void Update()
    {
        // ���ݑI������Ă���UI�I�u�W�F�N�g���擾
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        // �S�{�^���ɑ΂��ď������s��
        foreach (var btn in buttons)
        {
            // ���ݑI������Ă���{�^�����ǂ����𔻒�
            bool isSelected = (btn.gameObject == selectedObj);

            // �I������Ă���{�^���ɂ̓A�E�g���C����L���ɂ��A����ȊO�͖����ɂ���
            SetOutline(btn, isSelected);
        }
    }

    // �w�肵���{�^���ɃA�E�g���C����t������O�����肷��֐�
    void SetOutline(UnityEngine.UI.Button btn, bool enable)
    {
        // �{�^���Ɋ���Outline�R���|�[�l���g�����邩���ׂ�
        Outline outline = btn.GetComponent<Outline>();

        if (outline == null)
        {
            // Outline��������Βǉ�����
            outline = btn.gameObject.AddComponent<Outline>();

            // �A�E�g���C���̐F��Ԃɐݒ�
            outline.effectColor = Color.red;

            // �A�E�g���C���̕��𒲐�
            outline.effectDistance = new Vector2(3, 3);
        }

        // �A�E�g���C���̕\���E��\����؂�ւ�
        outline.enabled = enable;
    }
}
