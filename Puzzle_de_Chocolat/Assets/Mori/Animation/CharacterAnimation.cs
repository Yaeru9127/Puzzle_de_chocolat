using UnityEngine;

// �L�����N�^�[�A�j���[�V��������X�N���v�g
public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetMove(Vector3 dir, bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // ���x�̑傫����n���i�E�������ʃA�j���j
            animator.SetFloat("MoveX", Mathf.Abs(dir.x));
            spriteRenderer.flipX = dir.x < 0; // �������Ȃ甽�]
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
        }
    }

}
