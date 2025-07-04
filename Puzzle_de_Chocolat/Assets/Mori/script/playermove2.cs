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

        // �m�[�h�ڑ���`�F�o����
        nodeConnections[0] = new List<int> { 1 };         // ���W1 �� ���W2
        nodeConnections[1] = new List<int> { 0, 2 };      // ���W2 �� ���W1,3
        nodeConnections[2] = new List<int> { 1, 3 };      // ���W3 �� ���W2,4
        nodeConnections[3] = new List<int> { 2 };         // ���W4 �� ���W3

        // �����ʒu�Ɉړ�
        transform.position = nodes[currentNodeIndex].position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) //Esc��B�{�^��(��)�Ń^�C�g���֖߂�
        {
            Debug.Log("�^�C�g����");
            fadeController.FadeOutAndLoadScene("stag");  // �t�F�[�h�A�E�g���ă^�C�g����
        }
        if (!isMoving)
        {
            // �L�[�{�[�h��R���g���[���[���͏���
            Vector2 input = Vector2.zero;
#if ENABLE_INPUT_SYSTEM
            Gamepad pad = Gamepad.current;
            if (pad != null)
            {
                input = pad.leftStick.ReadValue();
            }
#endif
            input.x += Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(input.x) > 0.5f)
            {
                TryMove(input.x > 0 ? 1 : -1);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))//�X�y�[�X�L�[��A�{�^��(��)�ŃX�e�[�W�I��
            {
                SceneChange();
            }           
        }else
        {
            MoveToTarget();
        }
    }

    void SceneChange()
    {
        if (currentNodeIndex == 0)
        {
            Debug.Log("�X�e�[�W1");
            fadeController.FadeOutAndLoadScene("stag");
        }
        else if (currentNodeIndex == 1)
        {
            Debug.Log("�X�e�[�W2");
            // fadeController.FadeOutAndLoadScene("");
        }
        else if (currentNodeIndex == 2)
        {
            Debug.Log("�X�e�[�W3");
            // fadeController.FadeOutAndLoadScene("");
        }
        else if (currentNodeIndex == 3)
        {
            Debug.Log("�X�e�[�W4");
            // fadeController.FadeOutAndLoadScene("");
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
                targetPosition = nodes[targetNodeIndex].position;
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