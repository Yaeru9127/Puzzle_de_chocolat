using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class UIButtonOutlineSelector : MonoBehaviour
{
    private List<Button> buttons = new List<Button>();

    [System.Serializable]
    public class SliderLabelPair
    {
        public Slider slider;
        public Text label;
    }

    [SerializeField]
    private List<SliderLabelPair> sliderLabelPairs = new List<SliderLabelPair>();

    [SerializeField]
    private GameObject firstSelectObject;

    private GameObject lastSelected; // ← 追加

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
        //bool deviceCheck = Gamepad.all.Count > 0;
        //if (deviceCheck) return;

        if (EventSystem.current == null) return;

        var selectedObj = EventSystem.current.currentSelectedGameObject;

        // 選択が外れてしまった時 → 最後の選択に戻す
        if (selectedObj == null && lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
            selectedObj = lastSelected;
        }
        else
        {
            lastSelected = selectedObj;
        }

        // ① ボタン
        foreach (var btn in buttons)
        {
            bool isSelected = (btn.gameObject == selectedObj);
            SetOutline(btn, isSelected);
        }

        // ② スライダー
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
        if (target == null && buttons.Count > 0)
        {
            target = buttons[0].gameObject;
        }
        if (target != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(target);
            lastSelected = target;
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
