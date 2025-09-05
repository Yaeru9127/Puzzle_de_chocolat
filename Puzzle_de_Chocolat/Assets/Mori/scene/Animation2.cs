using UnityEngine;

public class Animation2 : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float lastMoveX = 0f;
    private float lastMoveY = 0f;

    private float stopDelayTimer = 0f;
    private float stopDelayDuration = 0.9f; // ��~�܂ł̒x�����ԁi�b�j

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // ���͂��Ȃ��i�L�[�������j�Ƃ�
        if (moveX == 0 && moveY == 0)
        {
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);

            // ��~�f�B���C�^�C�}�[�N��
            if (stopDelayTimer > 0f)
            {
                stopDelayTimer -= Time.deltaTime;
                if (stopDelayTimer <= 0f)
                {
                    animator.speed = 0f; // ���S��~
                }
            }
            else
            {
                stopDelayTimer = stopDelayDuration;
            }
        }
        else
        {
            // ���͂�����Ԃ̓A�j���[�V�����Đ�
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);

            lastMoveX = moveX;
            lastMoveY = moveY;

            animator.speed = 1f;
            stopDelayTimer = 0f; // �^�C�}�[���Z�b�g
        }

        // �X�v���C�g�̌����iX�����̂݁j
        if (moveX != 0)
        {
            spriteRenderer.flipX = (moveX > 0);
        }
    }
}
