using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
        // SpriteRenderer�R���|�[�l���g���擾
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ���͂��擾
        float moveX = Input.GetAxisRaw("Horizontal"); // ���E
        float moveY = Input.GetAxisRaw("Vertical");   // �㉺

        // �A�j���[�V�����̑J�ڂƊG�̔��]
        if (moveX > 0)
        {
            // �E�ړ��A�j���[�V����
            animator.SetTrigger("MoveRight");
            // �E�Ɉړ�����Ƃ��͊G�𔽓]������
            spriteRenderer.flipX = true;
        }
        else if (moveX < 0)
        {
            // ���ړ��A�j���[�V����
            animator.SetTrigger("MoveLeft");
            // ���Ɉړ�����Ƃ��͊G�𔽓]�����Ȃ�
            spriteRenderer.flipX = false;
        }
        else if (moveY > 0)
        {
            // ��ړ��A�j���[�V����
            animator.SetTrigger("MoveUp");
        }
        else if (moveY < 0)
        {
            // ���ړ��A�j���[�V����
            animator.SetTrigger("MoveDown");
        }
        else
        {
            // Idle�A�j���[�V����
            animator.SetTrigger("Idle");
        }
    }
}
