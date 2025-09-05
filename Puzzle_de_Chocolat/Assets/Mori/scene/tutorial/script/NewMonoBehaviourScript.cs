using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    // �m�[�h�i�}�X�j�̔z����C���X�y�N�^�[����ݒ�ł���悤��
    public Transform[] nodes;

    // �e�m�[�h�̐ڑ���m�[�h�̃C���f�b�N�X�i�C���X�y�N�^�[�Őݒ�j
    [System.Serializable]
    public class NodeConnection
    {
        public int nodeIndex;  // ���݂̃m�[�h
        public List<int> connectedNodes;  // �ڑ�����Ă���m�[�h�̃��X�g
    }

    public NodeConnection[] nodeConnections;

    private int currentNodeIndex = 0;  // ���݂̃m�[�h�̃C���f�b�N�X
    private int targetNodeIndex = -1;  // �ڕW�m�[�h�̃C���f�b�N�X

    public float moveSpeed = 5f;  // �ړ����x
    private bool isMoving = false;  // �ړ������ǂ���

    private Vector3 targetPosition;  // �ڕW�ʒu

    // �A�j���[�V�����p�Ɍ��J
    public bool IsMoving => isMoving;

    // �ړ��������v�Z����v���p�e�B
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
        // �����ڑ�����Ă��Ȃ��m�[�h�ڑ�����ɂ���
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

        // ���݂̃m�[�h����ړ��\�ȕ���������
        foreach (var connection in nodeConnections)
        {
            if (connection.nodeIndex == currentNodeIndex)
            {
                // ���͂ɉ����Ĉړ����������I��
                if (connection.connectedNodes.Contains(currentNodeIndex + 1) && (Input.GetKeyDown(KeyCode.RightArrow) || padRight))
                {
                    TryMove(1);
                }
                else if (connection.connectedNodes.Contains(currentNodeIndex - 1) && (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft))
                {
                    TryMove(-1);
                }
                else if (connection.connectedNodes.Contains(currentNodeIndex + 3) && (Input.GetKeyDown(KeyCode.UpArrow) || padUp))
                {
                    TryMoveUp(1);
                }
                else if (connection.connectedNodes.Contains(currentNodeIndex - 3) && (Input.GetKeyDown(KeyCode.DownArrow) || padDown))
                {
                    TryMoveDown(-1);
                }
                break;
            }
        }
    }

    private void TryMove(int v)
    {
        throw new NotImplementedException();
    }

    // ������ւ̈ړ�����
    void TryMoveUp(int direction)
    {
        targetNodeIndex = currentNodeIndex + direction;

        if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
        {
            Vector3 basePos = nodes[targetNodeIndex].position;
            targetPosition = new Vector3(basePos.x, basePos.y, basePos.z); // �ڕW�ʒu�ݒ�

            isMoving = true;
        }
    }

    // �������ւ̈ړ�����
    void TryMoveDown(int direction)
    {
        targetNodeIndex = currentNodeIndex + direction;

        if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
        {
            Vector3 basePos = nodes[targetNodeIndex].position;
            targetPosition = new Vector3(basePos.x, basePos.y, basePos.z); // �ڕW�ʒu�ݒ�

            isMoving = true;
        }
    }

    // �ڕW�ʒu�Ɍ������Ĉړ�
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

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
