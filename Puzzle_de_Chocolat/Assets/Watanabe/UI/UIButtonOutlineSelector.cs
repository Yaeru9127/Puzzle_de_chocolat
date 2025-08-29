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
        public Slider slider;   // BGM / SE のスライダー
        public Text label;      // スライダーの横にあるラベルText
    }

    [SerializeField]
    private List<SliderLabelPair> sliderLabelPairs = new List<SliderLabelPair>();

    [SerializeField]
    private GameObject firstSelectObject; // 最初に選択させたいオブジェクト（インスペクタで指定）

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

        // firstSelectObject が指定されていない場合はボタン[0]を使う
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
