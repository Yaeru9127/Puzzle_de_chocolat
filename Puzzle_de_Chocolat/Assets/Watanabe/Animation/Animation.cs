using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer; // �ǉ�

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // �ǉ�
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

        // ���� ���������]�����i�E�����̂Ƃ��� flipX = true�j
        if (moveX != 0)
        {
            spriteRenderer.flipX = (moveX > 0);  // �� �̂Ƃ� true�i���]�j
        }
    }
}
