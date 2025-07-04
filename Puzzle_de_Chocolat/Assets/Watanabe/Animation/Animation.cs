using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 入力を取得
        float moveX = Input.GetAxisRaw("Horizontal"); // 左右
        float moveY = Input.GetAxisRaw("Vertical");   // 上下

        // アニメーションの遷移と絵の反転
        if (moveX > 0)
        {
            // 右移動アニメーション
            animator.SetTrigger("MoveRight");
            // 右に移動するときは絵を反転させる
            spriteRenderer.flipX = true;
        }
        else if (moveX < 0)
        {
            // 左移動アニメーション
            animator.SetTrigger("MoveLeft");
            // 左に移動するときは絵を反転させない
            spriteRenderer.flipX = false;
        }
        else if (moveY > 0)
        {
            // 上移動アニメーション
            animator.SetTrigger("MoveUp");
        }
        else if (moveY < 0)
        {
            // 下移動アニメーション
            animator.SetTrigger("MoveDown");
        }
        else
        {
            // Idleアニメーション
            animator.SetTrigger("Idle");
        }
    }
}
