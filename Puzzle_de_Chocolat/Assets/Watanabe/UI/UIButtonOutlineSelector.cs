using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIButtonOutlineSelector : MonoBehaviour
{
    // このオブジェクトの子供にあるButtonコンポーネントを全て格納する配列
    private UnityEngine.UI.Button[] buttons;

    void Start()
    {
        // 子オブジェクトからButtonコンポーネントを全て取得
        buttons = GetComponentsInChildren<UnityEngine.UI.Button>();

        // ボタンが1つ以上あれば、最初のボタンを選択状態にしてアウトラインを有効にする
        if (buttons.Length > 0)
        {
            // EventSystemに最初のボタンを選択状態として登録
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);

            // 最初のボタンにアウトラインを付ける
            SetOutline(buttons[0], true);
        }
    }

    void Update()
    {
        // 現在選択されているUIオブジェクトを取得
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        // 全ボタンに対して処理を行う
        foreach (var btn in buttons)
        {
            // 現在選択されているボタンかどうかを判定
            bool isSelected = (btn.gameObject == selectedObj);

            // 選択されているボタンにはアウトラインを有効にし、それ以外は無効にする
            SetOutline(btn, isSelected);
        }
    }

    // 指定したボタンにアウトラインを付けたり外したりする関数
    void SetOutline(UnityEngine.UI.Button btn, bool enable)
    {
        // ボタンに既にOutlineコンポーネントがあるか調べる
        Outline outline = btn.GetComponent<Outline>();

        if (outline == null)
        {
            // Outlineが無ければ追加する
            outline = btn.gameObject.AddComponent<Outline>();

            // アウトラインの色を赤に設定
            outline.effectColor = Color.red;

            // アウトラインの幅を調整
            outline.effectDistance = new Vector2(3, 3);
        }

        // アウトラインの表示・非表示を切り替え
        outline.enabled = enable;
    }
}
