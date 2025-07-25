using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playermove2 : MonoBehaviour
{
    private FadeController fadeController;  // �t�F�[�h�R���g���[���[�̎Q��

    public Transform[] nodes; // �m�[�h���W��ݒ�
    private Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    private int currentNodeIndex = 0;
    private int targetNodeIndex = -1;

    public float moveSpeed = 5f;
    private bool isMoving = false;

    private Vector3 targetPosition;

    void Start()
    {
        // �t�F�[�h�R���g���[���[���擾
        fadeController = Object.FindFirstObjectByType<FadeController>();

        // ���W���Ƃɂ�����������w��
        nodeConnections[0] = new List<int> { 1 };         // ���W1 �� ���W2
        nodeConnections[1] = new List<int> { 0, 2 };      // ���W2 �� ���W1,3
        nodeConnections[2] = new List<int> { 1, 3 };      // ���W3 �� ���W2,4
        nodeConnections[3] = new List<int> { 2 };         // ���W4 �� ���W3

        // �����ʒu���w����W�ɐݒ�
        transform.position = new Vector3(6.75f, 2.73f, -1f);
    }

    void Update()
    {
        // �^�C�g���ɖ߂�iEsc�L�[�܂���B�{�^���j
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("�^�C�g����");
            fadeController.FadeOutAndLoadScene("stag");
        }

        if (!isMoving)
        {
            // �R���g���[���[���͂̎擾
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

            // �m�[�h���Ƃ̓��͏���
            switch (currentNodeIndex)
            {
                case 0: // ���W1 �� ���Ői��
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft)
                    {
                        TryMove(1); // �i��
                    }
                    break;

                case 1: // ���W2 �� ���Ői�� / �E�Ŗ߂�
                    if (Input.GetKeyDown(KeyCode.DownArrow) || padDown)
                    {
                        TryMove(1); // �i��
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) || padRight)
                    {
                        TryMove(-1); // �߂�
                    }
                    break;

                case 2: // ���W3 �� ���Ői�� / ��Ŗ߂�
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft)
                    {
                        TryMove(1); // �i��
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || padUp)
                    {
                        TryMove(-1); // �߂�
                    }
                    break;

                case 3: // ���W4 �� �E�Ŗ߂�
                    if (Input.GetKeyDown(KeyCode.RightArrow) || padRight)
                    {
                        TryMove(-1); // �߂�
                    }
                    break;
            }

            // �X�e�[�W�I���iSpace�L�[�A"Fire1"�A�܂���A�{�^���j
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1") ||
                (pad != null && pad.buttonSouth.wasPressedThisFrame))
            {
                SceneChange();
            }
        }
        else
        {
            MoveToTarget();
        }
    }

    void SceneChange()
    {
        switch (currentNodeIndex)
        {
            case 0:
                Debug.Log("�X�e�[�W1");
                fadeController.FadeOutAndLoadScene("stag");
                break;
            case 1:
                Debug.Log("�X�e�[�W2");
                // fadeController.FadeOutAndLoadScene("");
                break;
            case 2:
                Debug.Log("�X�e�[�W3");
                // fadeController.FadeOutAndLoadScene("");
                break;
            case 3:
                Debug.Log("�X�e�[�W4");
                // fadeController.FadeOutAndLoadScene("");
                break;
        }
    }

    void TryMove(int direction)
    {
        if (!nodeConnections.ContainsKey(currentNodeIndex)) return;

        foreach (int connectedNode in nodeConnections[currentNodeIndex])
        {
            if (connectedNode == currentNodeIndex + direction)
            {
                targetNodeIndex = connectedNode;

                Vector3 basePos = nodes[targetNodeIndex].position;
                targetPosition = new Vector3(basePos.x - 0.03f, basePos.y + 0.91f, -1f);

                isMoving = true;
                break;
            }
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            currentNodeIndex = targetNodeIndex;
            targetNodeIndex = -1;
            isMoving = false;
        }
    }
}
