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
        // 矢印キー（キーボード）とコントローラー入力を合成
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 移動がゼロのときは停止処理（最後の方向でフレーム停止）
        if (moveX == 0 && moveY == 0)
        {
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);

            animator.speed = 0f;  // アニメーション停止
        }
        else
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);

            lastMoveX = moveX;
            lastMoveY = moveY;

            animator.speed = 1f;  // アニメーション再開
        }

        // X方向入力がある場合だけ反転処理
        if (moveX != 0)
        {
            spriteRenderer.flipX = (moveX > 0);
        }
    }
}
