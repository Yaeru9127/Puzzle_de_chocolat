using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Shake : MonoBehaviour
{
    //揺らしたいRectTransform
    public RectTransform targetRcttransform;
    //アニメーションの時間
    public float duration = 0.2f;

    private Vector3 LocalPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (targetRcttransform == null) targetRcttransform = GetComponent<RectTransform>();
        LocalPosition = targetRcttransform.localPosition;
    }

    //揺れのアニメーションを再生させる
    public　void TriggerShake()
    {
        //アニメーションを終了させて元の位置から新しくスタートさせる
        targetRcttransform.DOKill(true);
        targetRcttransform.localPosition = LocalPosition;

        targetRcttransform.DOPunchPosition(new Vector3(30, 0, 0), duration).OnComplete(() =>
        {
            targetRcttransform.localPosition = LocalPosition;
        });

    }
}