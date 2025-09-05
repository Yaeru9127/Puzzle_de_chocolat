using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Shake : MonoBehaviour
{
    //�h�炵����RectTransform
    public RectTransform targetRcttransform;
    //�A�j���[�V�����̎���
    public float duration = 0.2f;

    private Vector3 LocalPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (targetRcttransform == null) targetRcttransform = GetComponent<RectTransform>();
        LocalPosition = targetRcttransform.localPosition;
    }

    //�h��̃A�j���[�V�������Đ�������
    public�@void TriggerShake()
    {
        //�A�j���[�V�������I�������Č��̈ʒu����V�����X�^�[�g������
        targetRcttransform.DOKill(true);
        targetRcttransform.localPosition = LocalPosition;

        targetRcttransform.DOPunchPosition(new Vector3(30, 0, 0), duration).OnComplete(() =>
        {
            targetRcttransform.localPosition = LocalPosition;
        });

    }
}