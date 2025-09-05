using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private playermove2 moveScript; // 移動スクリプトの参照
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (moveScript == null)
        {
            //Debug.LogError("playermove2スクリプトの参照がセットされていません");
        }
    }

    void Update()
    {
        if (moveScript == null) return;

        // 移動中かどうかをAnimatorにセット
        animator.SetBool("isWalking", moveScript.IsMoving);

        if (moveScript.IsMoving)
        {
            Vector3 dir = moveScript.MoveDirection;

            int direction = GetDirectionFromVector(dir);
            animator.SetInteger("direction", direction);
        }
    }

    // direction: 0=前, 1=右, 2=後ろ, 3=左
    private int GetDirectionFromVector(Vector3 dir)
    {
        float angle = Vector3.SignedAngle(Vector3.forward, dir, Vector3.up);

        if (angle >= -45f && angle < 45f) return 0;      // 前
        else if (angle >= 45f && angle < 135f) return 1; // 右
        else if (angle >= -135f && angle < -45f) return 3; // 左
        else return 2;                                    // 後ろ
    }
}
