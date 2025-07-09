using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer; // 追加

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 追加
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        if (Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) moveY = -1f;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        // ←★ ここが反転処理（右向きのときに flipX = true）
        if (moveX != 0)
        {
            spriteRenderer.flipX = (moveX > 0);  // → のとき true（反転）
        }
    }
}
