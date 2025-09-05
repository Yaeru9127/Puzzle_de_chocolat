using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playermove2 : MonoBehaviour
{
    private FadeController fadeController;
    private CharacterAnimation characterAnimation;

    public Transform[] nodes; // �m�[�h���W
    public Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    public int currentNodeIndex = 0;
    private int targetNodeIndex = -1;

    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Vector3 targetPosition;

    [Header("�V�[���ɒu�������I�u�W�F�N�g")]
    public GameObject arrowLeft;   // �����i�V�[���ɔz�u�ς݁j
    public GameObject arrowRight;  // �E���i�V�[���ɔz�u�ς݁j

    void Start()
    {
        fadeController = Object.FindFirstObjectByType<FadeController>();
        characterAnimation = GetComponent<CharacterAnimation>();

        // �m�[�h�ڑ��i�����ړ��j
        nodeConnections[0] = new List<int> { 1 };
        nodeConnections[1] = new List<int> { 0, 2 };
        nodeConnections[2] = new List<int> { 1, 3 };
        nodeConnections[3] = new List<int> { 2 };

        // �����ʒu
        transform.position = nodes[currentNodeIndex].position;

        UpdateArrows();
    }

    void Update()
    {
        // Esc / B�{�^���Ń^�C�g���֖߂�
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            fadeController.FadeOutAndLoadScene("stag");
        }

        Vector2 input = Vector2.zero;

        if (!isMoving)
        {
#if ENABLE_INPUT_SYSTEM
            Gamepad pad = Gamepad.current;
            if (pad != null) input = pad.leftStick.ReadValue();
#endif
            if (input == Vector2.zero)
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");
            }

            if (Mathf.Abs(input.x) > 0.5f)
            {
                TryMove(input.x > 0 ? -1 : 1); // �E�L�[�͍��ɐi��
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            {
                SceneChange();
            }

            characterAnimation.SetMove(input.normalized, false);
        }
        else
        {
            MoveToTarget();

            float dirX = targetNodeIndex - currentNodeIndex;
            if ((currentNodeIndex == 1 && targetNodeIndex == 2) ||
                (currentNodeIndex == 2 && targetNodeIndex == 1))
            {
                dirX *= -1f;
            }

            Vector2 dir = new Vector2(Mathf.Sign(dirX), 0f);
            characterAnimation.SetMove(dir, true);
        }

        UpdateArrows();
    }

    void SceneChange()
    {
        switch (currentNodeIndex)
        {
            case 0: fadeController.FadeOutAndLoadScene("stag"); break;
            case 1: Debug.Log("�X�e�[�W2"); break;
            case 2: Debug.Log("�X�e�[�W3"); break;
            case 3: Debug.Log("�X�e�[�W4"); break;
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

    void UpdateArrows()
    {
        if (arrowLeft == null || arrowRight == null) return;

        if (currentNodeIndex == 0)
        {
            // �ŏ��̃m�[�h �� �E�����\��
            arrowLeft.SetActive(true);
            arrowRight.SetActive(false);
        }
        else if (currentNodeIndex == nodes.Length - 1)
        {
            // �Ō�̃m�[�h �� �������\��
            arrowLeft.SetActive(false);
            arrowRight.SetActive(true);
        }
        else
        {
            // ���ԃm�[�h �� �����\��
            arrowLeft.SetActive(true);
            arrowRight.SetActive(true);
        }
    }
}
