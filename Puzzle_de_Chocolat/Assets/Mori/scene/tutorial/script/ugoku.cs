using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ugoku : MonoBehaviour
{
    [Header("�m�[�h�̐ݒ�")]
    public Transform[] nodes;  // �m�[�h�i�}�X�j�̔z��

    [System.Serializable]
    public class NodeConnectionData
    {
        public int nodeIndex;  // ���݂̃m�[�h
        public List<int> connectedNodes;  // �ڑ�����Ă���m�[�h�̃��X�g
    }

    [Header("�m�[�h�ڑ��ݒ�")]
    public NodeConnectionData[] nodeConnections;  // �m�[�h�ڑ����

    private int currentNodeIndex = 0;  // ���݂̃m�[�h�C���f�b�N�X
    private int targetNodeIndex = -1;  // �ڕW�m�[�h�C���f�b�N�X

    [Header("�ړ��ݒ�")]
    public float speed = 5f;  // �ړ����x�imoveSpeed����ύX�j
    private bool isMoving = false;  // �ړ������ǂ���

    private Vector3 targetPosition;  // �ڕW�ʒu

    // �A�j���[�V�����p�Ɍ��J
    public bool IsMoving => isMoving;

    public Vector3 MoveDirection
    {
        get
        {
            if (!isMoving) return Vector3.zero;
            Vector3 dir = targetPosition - transform.position;
            dir.y = 0;
            return dir.normalized;
        }
    }

    void Start()
    {
        if (nodeConnections.Length == 0)
        {
            Debug.LogError("�m�[�h�ڑ����ݒ肳��Ă��܂���I�C���X�y�N�^�[�Ńm�[�h�ڑ���ݒ肵�Ă��������B");
        }
        transform.position = new Vector3(6.75f, 2.73f, -1f);  // �����ʒu�ݒ�
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();  // ���͏���
        }
        else
        {
            MoveToTarget();  // �ړ�����
        }
    }

    // ���͏���
    void HandleInput()
    {
        Gamepad pad = Gamepad.current;
        bool padLeft = false, padRight = false, padUp = false, padDown = false;

#if ENABLE_INPUT_SYSTEM
        if (pad != null)
        {
            Vector2 stick = pad.leftStick.ReadValue();
            padLeft = stick.x < -0.5f || pad.dpad.left.wasPressedThisFrame;
            padRight = stick.x > 0.5f || pad.dpad.right.wasPressedThisFrame;
            padUp = stick.y > 0.5f || pad.dpad.up.wasPressedThisFrame;
            padDown = stick.y < -0.5f || pad.dpad.down.wasPressedThisFrame;
        }
#endif

        // �m�[�h�ڑ��Ɋ�Â��ړ�����
        foreach (var connection in nodeConnections)
        {
            if (connection.nodeIndex == currentNodeIndex)
            {
                // �e�����Ɉړ��\���`�F�b�N
                if (TryMoveIfValid(currentNodeIndex + 1, padRight, KeyCode.RightArrow))  // �E
                {
                    TryMove(1);
                }
                else if (TryMoveIfValid(currentNodeIndex - 1, padLeft, KeyCode.LeftArrow))  // ��
                {
                    TryMove(-1);
                }
                else if (TryMoveIfValid(currentNodeIndex + 3, padUp, KeyCode.UpArrow))  // ��
                {
                    TryMove(3);
                }
                else if (TryMoveIfValid(currentNodeIndex - 3, padDown, KeyCode.DownArrow))  // ��
                {
                    TryMove(-3);
                }
                break;
            }
        }
    }

    // �ړ��\���ǂ������`�F�b�N
    private bool TryMoveIfValid(int targetIndex, bool padInput, KeyCode keyInput)
    {
        return (padInput || Input.GetKeyDown(keyInput)) && IsValidMove(targetIndex);
    }

    // �ړ����L�����`�F�b�N
    private bool IsValidMove(int targetIndex)
    {
        foreach (var connection in nodeConnections)
        {
            if (connection.nodeIndex == currentNodeIndex)
            {
                return connection.connectedNodes.Contains(targetIndex);
            }
        }
        return false;
    }

    // �ړ������s
    private void TryMove(int direction)
    {
        targetNodeIndex = currentNodeIndex + direction;

        // �m�[�h�͈͓̔��ł���Έړ�
        if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
        {
            Vector3 basePos = nodes[targetNodeIndex].position;
            targetPosition = new Vector3(basePos.x, basePos.y, basePos.z);  // �ڕW�ʒu�ݒ�

            isMoving = true;  // �ړ��t���O�𗧂Ă�
        }
    }

    // �ڕW�ʒu�Ɍ������Ĉړ�
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // �ڕW�ʒu�ɓ��B�����ꍇ
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            currentNodeIndex = targetNodeIndex;
            targetNodeIndex = -1;
            isMoving = false;
        }
    }
}
