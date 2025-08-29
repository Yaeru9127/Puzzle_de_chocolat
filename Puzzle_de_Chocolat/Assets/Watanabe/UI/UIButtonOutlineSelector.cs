using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIButtonOutlineSelector : MonoBehaviour
{
    private List<Button> buttons = new List<Button>();

    [System.Serializable]
    public class SliderLabelPair
    {
        public Slider slider;   // BGM / SE �̃X���C�_�[
        public Text label;      // �X���C�_�[�̉��ɂ��郉�x��Text
    }

    [SerializeField]
    private List<SliderLabelPair> sliderLabelPairs = new List<SliderLabelPair>();

    void Start()
    {
        // �q�I�u�W�F�N�g�ɂ���S�Ă�Button��o�^
        buttons.AddRange(GetComponentsInChildren<Button>());

        // �e�{�^���̃i�r�Q�[�V������ݒ�i�f�t�H���g�̐ݒ��L���ɂ���j
        foreach (var btn in buttons)
        {
            Button btnComponent = btn.GetComponent<Button>();
            btnComponent.navigation = Navigation.defaultNavigation;  // �f�t�H���g�̃i�r�Q�[�V�����ݒ�
        }

        // �ŏ��̑I����ݒ�
        SelectFirst();
    }

    void OnEnable()
    {
        // �V�[���ؑւ�ĕ\�����ɂ��K���ŏ���I������
        SelectFirst();
    }

    void Update()
    {
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
                // ���x�����̂͑I������Ȃ����ǁA�����ڂŘg���o��
                SetOutline(pair.label, isSelected);
            }
        }
    }

    // �ŏ��̃{�^����I����Ԃɂ���
    private void SelectFirst()
    {
        if (buttons.Count > 0 && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // �������񃊃Z�b�g
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            SetOutline(buttons[0], true);
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

    void SetOutline(Text txt, bool enable)
    {
        Outline outline = txt.GetComponent<Outline>();
        if (outline == null)
        {
            outline = txt.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;
            outline.effectDistance = new Vector2(1, 1);
        }
        outline.enabled = enable;
    }
}
