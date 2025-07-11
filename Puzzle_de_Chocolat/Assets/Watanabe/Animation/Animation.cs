using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float lastMoveX = 0f;
    private float lastMoveY = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ���L�[�i�L�[�{�[�h�j�ƃR���g���[���[���͂�����
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // �ړ����[���̂Ƃ��͒�~�����i�Ō�̕����Ńt���[����~�j
        if (moveX == 0 && moveY == 0)
        {
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);

            animator.speed = 0f;  // �A�j���[�V������~
        }
        else
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);

            lastMoveX = moveX;
            lastMoveY = moveY;

            animator.speed = 1f;  // �A�j���[�V�����ĊJ
        }

        // X�������͂�����ꍇ�������]����
        if (moveX != 0)
        {
            spriteRenderer.flipX = (moveX > 0);
        }
    }
}
