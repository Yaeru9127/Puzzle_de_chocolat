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

    void Start()
    {
        // 子オブジェクトにある全てのButtonを登録
        buttons.AddRange(GetComponentsInChildren<Button>());

        // 各ボタンのナビゲーションを設定（デフォルトの設定を有効にする）
        foreach (var btn in buttons)
        {
            Button btnComponent = btn.GetComponent<Button>();
            btnComponent.navigation = Navigation.defaultNavigation;  // デフォルトのナビゲーション設定
        }

        // 最初の選択を設定
        SelectFirst();
    }

    void OnEnable()
    {
        // シーン切替や再表示時にも必ず最初を選択する
        SelectFirst();
    }

    void Update()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        // ① ボタンの選択判定
        foreach (var btn in buttons)
        {
            bool isSelected = (btn.gameObject == selectedObj);
            SetOutline(btn, isSelected);
        }

        // ② スライダーの選択判定
        foreach (var pair in sliderLabelPairs)
        {
            bool isSelected = (pair.slider != null && pair.slider.gameObject == selectedObj);
            if (pair.label != null)
            {
                // ラベル自体は選択されないけど、見た目で枠を出す
                SetOutline(pair.label, isSelected);
            }
        }
    }

    // 最初のボタンを選択状態にする
    private void SelectFirst()
    {
        if (buttons.Count > 0 && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // いったんリセット
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
