using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private playermove2 moveScript; // �ړ��X�N���v�g�̎Q��
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (moveScript == null)
        {
            //Debug.LogError("playermove2�X�N���v�g�̎Q�Ƃ��Z�b�g����Ă��܂���");
        }
    }

    void Update()
    {
        if (moveScript == null) return;

        // �ړ������ǂ�����Animator�ɃZ�b�g
        animator.SetBool("isWalking", moveScript.IsMoving);

        if (moveScript.IsMoving)
        {
            Vector3 dir = moveScript.MoveDirection;

            int direction = GetDirectionFromVector(dir);
            animator.SetInteger("direction", direction);
        }
    }

    // direction: 0=�O, 1=�E, 2=���, 3=��
    private int GetDirectionFromVector(Vector3 dir)
    {
        float angle = Vector3.SignedAngle(Vector3.forward, dir, Vector3.up);

        if (angle >= -45f && angle < 45f) return 0;      // �O
        else if (angle >= 45f && angle < 135f) return 1; // �E
        else if (angle >= -135f && angle < -45f) return 3; // ��
        else return 2;                                    // ���
    }
}
