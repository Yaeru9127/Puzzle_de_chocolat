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

    [SerializeField]
    private GameObject firstSelectObject; // �ŏ��ɑI�����������I�u�W�F�N�g�i�C���X�y�N�^�Ŏw��j

    void Start()
    {
        buttons.AddRange(GetComponentsInChildren<Button>());
        SelectFirst();
    }

    void OnEnable()
    {
        SelectFirst();
    }

    void Update()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        // �@ �{�^��
        foreach (var btn in buttons)
        {
            bool isSelected = (btn.gameObject == selectedObj);
            SetOutline(btn, isSelected);
        }

        // �A �X���C�_�[
        foreach (var pair in sliderLabelPairs)
        {
            bool isSelected = (pair.slider != null && pair.slider.gameObject == selectedObj);
            if (pair.label != null)
            {
                SetOutline(pair.label, isSelected);
            }
        }
    }

    private void SelectFirst()
    {
        if (EventSystem.current == null) return;

        GameObject target = firstSelectObject;

        // firstSelectObject ���w�肳��Ă��Ȃ��ꍇ�̓{�^��[0]���g��
        if (target == null && buttons.Count > 0)
        {
            target = buttons[0].gameObject;
        }

        if (target != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(target);
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
