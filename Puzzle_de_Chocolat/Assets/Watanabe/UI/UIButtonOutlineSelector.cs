using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIButtonOutlineSelector : MonoBehaviour
{
    // このオブジェクトの子供にあるButtonコンポーネントを全て格納するリスト
    private List<Button> buttons = new List<Button>();

    void Start()    
    {
        // 子オブジェクトからButtonコンポーネントを全て取得
        buttons.AddRange(GetComponentsInChildren<Button>());

       
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

    // オプションメニューのボタンを追加登録する関数
    public void AddOptionButtons(GameObject optionMenuRoot)
    {
        Button[] optionButtons = optionMenuRoot.GetComponentsInChildren<Button>(true); 
        foreach (var btn in optionButtons)
        {
            if (!buttons.Contains(btn)) 
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
