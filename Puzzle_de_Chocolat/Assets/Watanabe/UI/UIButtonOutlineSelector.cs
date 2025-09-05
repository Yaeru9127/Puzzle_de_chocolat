using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.InputSystem.UI;

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

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;


    void Start()
    {
        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();

        buttons.AddRange(GetComponentsInChildren<Button>());
        SelectFirst();
    }

    void OnEnable()
    {
        SelectFirst();
    }

    private void OnDisable()
    {
        
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
            SetOutlineButton(btn, isSelected);
        }

        // ② スライダー
        foreach (var pair in sliderLabelPairs)
        {
            bool isSelected = (pair.slider != null && pair.slider.gameObject == selectedObj);
            if (pair.label != null)
            {
                SetOutlineText(pair.label, isSelected);
            }
        }

        //コントローラー入力でボタンクリック
        if (Gamepad.all.Count > 0)
        {
            if (actions.GamePad.Click.WasPressedThisFrame()) ControllerClick(selectedObj);
        }
        else if (Gamepad.all.Count == 0)
        {
            if (actions.Mouse.Click.WasPressedThisFrame()) ControllerClick(selectedObj);
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

    void SetOutlineButton(Button btn, bool enable)
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

    void SetOutlineText(Text txt, bool enable)
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

    private void ControllerClick(GameObject bt)
    {
        //選択されいるオブジェクトが存在しないときは無視
        if (bt == null) return;

        //Keyboard操作でないときは無視
        if (Gamepad.all.Count == 0) return;

        Button button = bt.GetComponent<Button>();

        //選択されているUIがButtonじゃないときは無視
        if (bt == null) return;

        button.onClick.Invoke();
    }
}
