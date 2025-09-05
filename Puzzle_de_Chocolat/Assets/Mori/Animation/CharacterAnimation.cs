using UnityEngine;

// キャラクターアニメーション制御スクリプト
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
            // 速度の大きさを渡す（右向き共通アニメ）
            animator.SetFloat("MoveX", Mathf.Abs(dir.x));
            spriteRenderer.flipX = dir.x < 0; // 左向きなら反転
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
        }
    }

}
