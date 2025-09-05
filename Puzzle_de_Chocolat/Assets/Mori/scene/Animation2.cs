using UnityEngine;

public class Animation2 : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float lastMoveX = 0f;
    private float lastMoveY = 0f;

    private float stopDelayTimer = 0f;
    private float stopDelayDuration = 0.9f; // 停止までの遅延時間（秒）

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 入力がない（キー離した）とき
        if (moveX == 0 && moveY == 0)
        {
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);

            // 停止ディレイタイマー起動
            if (stopDelayTimer > 0f)
            {
                stopDelayTimer -= Time.deltaTime;
                if (stopDelayTimer <= 0f)
                {
                    animator.speed = 0f; // 完全停止
                }
            }
            else
            {
                stopDelayTimer = stopDelayDuration;
            }
        }
        else
        {
            // 入力がある間はアニメーション再生
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);

            lastMoveX = moveX;
            lastMoveY = moveY;

            animator.speed = 1f;
            stopDelayTimer = 0f; // タイマーリセット
        }

        // スプライトの向き（X方向のみ）
        if (moveX != 0)
        {
            spriteRenderer.flipX = (moveX > 0);
        }
    }
}
