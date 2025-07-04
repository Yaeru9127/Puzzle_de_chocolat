using UnityEngine;
using UnityEngine.EventSystems; // EventSystem関連の機能を使用するために必要

public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator myAnimator; // ボタンにアタッチされたAnimatorコンポーネント

    void Awake()
    {
        // Animatorコンポーネントを取得
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            Debug.LogError("ButtonAnimationController: Animatorコンポーネントが見つかりません。このスクリプトはAnimatorコンポーネントがアタッチされたGameObjectにアタッチしてください。", this);
        }
    }

    // マウスカーソルがボタンに入ったときに呼び出される
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myAnimator != null)
        {
            // Animatorの"Highlighted" Triggerを設定。
            // "Highlighted"ステートのアニメーションがループ設定されていれば、
            // カーソルが合っている間、このアニメーションが再生され続けます。
            myAnimator.SetTrigger("Highlighted");
            Debug.Log("マウスがボタンに入りました: " + gameObject.name + " -> Highlightedアニメーション再生 (ループ)");
        }
    }

    // マウスカーソルがボタンから出たときに呼び出される
    public void OnPointerExit(PointerEventData eventData)
    {
        if (myAnimator != null)
        {
            // Animatorの"Normal" Triggerを設定して、通常のアニメーションを再生。
            // これにより、"Highlighted"ステートから"Normal"ステートへ遷移します。
            myAnimator.SetTrigger("Normal");
            Debug.Log("マウスがボタンから出ました: " + gameObject.name + " -> Normalアニメーション再生");
        }
    }
}
