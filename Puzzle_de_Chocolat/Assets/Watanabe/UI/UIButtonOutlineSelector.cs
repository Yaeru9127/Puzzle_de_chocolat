using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIButtonOutlineSelector : MonoBehaviour
{
    // 子オブジェクト内のButtonをまとめて管理するリスト
    private List<Button> buttons = new List<Button>();

    // SliderとTextをペアで管理するクラス
    [System.Serializable]
    public class SliderLabelPair
    {
        public Slider slider;   // BGM / SE のスライダー
        public Text label;      // スライダーの横にあるラベルText
    }

    // Inspector上で設定できるようにSerializeField化
    [SerializeField] private List<SliderLabelPair> sliderLabelPairs = new List<SliderLabelPair>();

    void Start()
    {
        // 子オブジェクトにある全てのButtonを登録
        buttons.AddRange(GetComponentsInChildren<Button>());

        // 最初にボタンが存在すれば、そのボタンを選択状態にする
        if (buttons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            SetOutline(buttons[0], true);  // 最初のボタンに赤枠を付ける
        }
    }

    void Update()
    {
        // 現在EventSystemで選択されているUIオブジェクトを取得
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
                SetOutline(pair.label, isSelected);
            }
        }
    }

    // Buttonに赤枠を設定する
    void SetOutline(Button btn, bool enable)
    {
        Outline outline = btn.GetComponent<Outline>();
        if (outline == null)
        {
            outline = btn.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;          // 赤色の枠
            outline.effectDistance = new Vector2(3, 3); // 枠線の太さ
        }
        outline.enabled = enable;
    }

    // Textに赤枠を設定する
    void SetOutline(Text txt, bool enable)
    {
        Outline outline = txt.GetComponent<Outline>();
        if (outline == null)
        {
            outline = txt.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;          // 赤色の枠
            outline.effectDistance = new Vector2(1, 1); // 枠線の太さ
        }
        outline.enabled = enable;
    }
}
