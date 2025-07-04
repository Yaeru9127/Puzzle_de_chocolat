using UnityEngine;
using UnityEngine.EventSystems; // EventSystem�֘A�̋@�\���g�p���邽�߂ɕK�v

public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator myAnimator; // �{�^���ɃA�^�b�`���ꂽAnimator�R���|�[�l���g

    void Awake()
    {
        // Animator�R���|�[�l���g���擾
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            Debug.LogError("ButtonAnimationController: Animator�R���|�[�l���g��������܂���B���̃X�N���v�g��Animator�R���|�[�l���g���A�^�b�`���ꂽGameObject�ɃA�^�b�`���Ă��������B", this);
        }
    }

    // �}�E�X�J�[�\�����{�^���ɓ������Ƃ��ɌĂяo�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myAnimator != null)
        {
            // Animator��"Highlighted" Trigger��ݒ�B
            // "Highlighted"�X�e�[�g�̃A�j���[�V���������[�v�ݒ肳��Ă���΁A
            // �J�[�\���������Ă���ԁA���̃A�j���[�V�������Đ����ꑱ���܂��B
            myAnimator.SetTrigger("Highlighted");
            Debug.Log("�}�E�X���{�^���ɓ���܂���: " + gameObject.name + " -> Highlighted�A�j���[�V�����Đ� (���[�v)");
        }
    }

    // �}�E�X�J�[�\�����{�^������o���Ƃ��ɌĂяo�����
    public void OnPointerExit(PointerEventData eventData)
    {
        if (myAnimator != null)
        {
            // Animator��"Normal" Trigger��ݒ肵�āA�ʏ�̃A�j���[�V�������Đ��B
            // ����ɂ��A"Highlighted"�X�e�[�g����"Normal"�X�e�[�g�֑J�ڂ��܂��B
            myAnimator.SetTrigger("Normal");
            Debug.Log("�}�E�X���{�^������o�܂���: " + gameObject.name + " -> Normal�A�j���[�V�����Đ�");
        }
    }
}
