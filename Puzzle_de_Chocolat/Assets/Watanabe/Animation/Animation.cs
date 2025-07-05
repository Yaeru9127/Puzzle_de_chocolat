using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        bool isMoving = (x != 0 || y != 0);

        animator.SetFloat("MoveX", x);
        animator.SetFloat("MoveY", y);
        animator.SetBool("IsMoving", isMoving);
    }
}
