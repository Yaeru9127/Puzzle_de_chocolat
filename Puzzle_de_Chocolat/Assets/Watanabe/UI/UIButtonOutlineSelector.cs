using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIButtonOutlineSelector : MonoBehaviour
{
    // �q�I�u�W�F�N�g����Button���܂Ƃ߂ĊǗ����郊�X�g
    private List<Button> buttons = new List<Button>();

    // Slider��Text���y�A�ŊǗ�����N���X
    [System.Serializable]
    public class SliderLabelPair
    {
        public Slider slider;   // BGM / SE �̃X���C�_�[
        public Text label;      // �X���C�_�[�̉��ɂ��郉�x��Text
    }

    // Inspector��Őݒ�ł���悤��SerializeField��
    [SerializeField] private List<SliderLabelPair> sliderLabelPairs = new List<SliderLabelPair>();

    void Start()
    {
        // �q�I�u�W�F�N�g�ɂ���S�Ă�Button��o�^
        buttons.AddRange(GetComponentsInChildren<Button>());

        // �ŏ��Ƀ{�^�������݂���΁A���̃{�^����I����Ԃɂ���
        if (buttons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            SetOutline(buttons[0], true);  // �ŏ��̃{�^���ɐԘg��t����
        }
    }

    void Update()
    {
        // ����EventSystem�őI������Ă���UI�I�u�W�F�N�g���擾
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        // �@ �{�^���̑I�𔻒�
        foreach (var btn in buttons)
        {
            bool isSelected = (btn.gameObject == selectedObj);
            SetOutline(btn, isSelected);   
        }

        // �A �X���C�_�[�̑I�𔻒�
        foreach (var pair in sliderLabelPairs)
        {
            bool isSelected = (pair.slider != null && pair.slider.gameObject == selectedObj);
            if (pair.label != null)
            {
                SetOutline(pair.label, isSelected);
            }
        }
    }

    // Button�ɐԘg��ݒ肷��
    void SetOutline(Button btn, bool enable)
    {
        Outline outline = btn.GetComponent<Outline>();
        if (outline == null)
        {
            outline = btn.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;          // �ԐF�̘g
            outline.effectDistance = new Vector2(3, 3); // �g���̑���
        }
        outline.enabled = enable;
    }

    // Text�ɐԘg��ݒ肷��
    void SetOutline(Text txt, bool enable)
    {
        Outline outline = txt.GetComponent<Outline>();
        if (outline == null)
        {
            outline = txt.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;          // �ԐF�̘g
            outline.effectDistance = new Vector2(1, 1); // �g���̑���
        }
        outline.enabled = enable;
    }
}
