using UnityEngine;
using UnityEngine.SceneManagement;

public class playermove : MonoBehaviour
{
    public float moveDistance = 7f; // ��x�̓��͂Ői�ދ���
    public float moveSpeed = 5f; // �ړ����x
    private bool isMoving = false; // �ړ������ǂ���
    private Vector3 targetPosition; // �ړ���̈ʒu

    public GameObject[] stage = new GameObject[4];  // Inspector��4�̃I�u�W�F�N�g�����蓖�Ă�

    // �e���W�̐�����ݒ�
    private Vector3 restrictedPosition1 = new Vector3(-7.3f, -2.71f, 0); // �E��̂�
    private Vector3 restrictedPosition2 = new Vector3(-2.360252f, 2.299747f, 0); // �����ƉE���̂�
    private Vector3 restrictedPosition3 = new Vector3(2.589495f, -2.65f, 0); // ����ƉE��̂�
    //private Vector3 restrictedPosition4 = new Vector3(7.539243f, 2.299747f, 0); // �����̂�

    // ���e�͈́i�덷���l�����āj
    public float tolerance = 0.1f;

    private FadeController fadeController;  // �t�F�[�h�R���g���[���[�̎Q��

    void Start()
    {
        // �t�F�[�h�R���g���[���[���擾
        fadeController = Object.FindFirstObjectByType<FadeController>();

        for (int i = 0; i < stage.Length; i++)
        {
            Vector3 pos = stage[i].transform.position;
            Debug.Log($"�X�e�[�W�I�u�W�F�N�g{i + 1}�̍��W: {pos}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) // Esc��B�{�^���ŃX�e�[�W�I��
        {
            Debug.Log("�^�C�g����");
            fadeController.FadeOutAndLoadScene("stag");  // �t�F�[�h�A�E�g���ăX�e�[�W1��
        }

        if (isMoving)
        {
            // �ړ����͎w�肵���ʒu�Ɍ������Đi��
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �ړI�n�ɓ��B������ړ���~
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
        else
        {
            // ���͂ňړ��J�n
            HandleInput();
        }
    }

    void HandleInput()
    {
        // ���݂̈ʒu���������ꂽ���W�߂��ɂ��邩�m�F
        if (IsNearRestrictedPosition(transform.position, restrictedPosition1))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // �X�y�[�X��A�{�^��(��)�ŃX�e�[�W�I��
            {
                Debug.Log("�X�e�[�W1");
                fadeController.FadeOutAndLoadScene("stag");  // �t�F�[�h�A�E�g���ăX�e�[�W1��
            }
            // �E��̂�
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) &&
                (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0))
            {
                StartMove(Vector3.up + Vector3.right); // �E��
            }
        }
        else if (IsNearRestrictedPosition(transform.position, restrictedPosition2))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // �X�y�[�X��A�{�^��(��)�ŃX�e�[�W�I��
            {
                Debug.Log("�X�e�[�W2");
                //fadeController.FadeOutAndLoadScene("");  // �t�F�[�h�A�E�g���ăX�e�[�W2��
            }
            // �����ƉE���̂�
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) &&
                (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0))
            {
                StartMove(Vector3.down + Vector3.left); // ����
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) &&
                     (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0))
            {
                StartMove(Vector3.down + Vector3.right); // �E��
            }
        }
        else if (IsNearRestrictedPosition(transform.position, restrictedPosition3))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // �X�y�[�X��A�{�^��(��)�ŃX�e�[�W�I��
            {
                Debug.Log("�X�e�[�W3");
                //fadeController.FadeOutAndLoadScene("");  // �t�F�[�h�A�E�g���ăX�e�[�W3��
            }
            // ����ƉE��̂�
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) &&
                (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0))
            {
                StartMove(Vector3.up + Vector3.left); // ����
            }
            /*else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) &&
                     (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0))
            {
                StartMove(Vector3.up + Vector3.right); // �E��
            }*/
        }
        /*else if (IsNearRestrictedPosition(transform.position, restrictedPosition4))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // �X�y�[�X��A�{�^��(��)�ŃX�e�[�W�I��
            {
                Debug.Log("�X�e�[�W4");
                //fadeController.FadeOutAndLoadScene("");  // �t�F�[�h�A�E�g���ăX�e�[�W4��
            }
            // �����̂�
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) &&
                (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0))
            {
                StartMove(Vector3.down + Vector3.left); // ����
            }
        }*/
    }

    void StartMove(Vector3 direction)
    {
        // ���݈ʒu����w�肳�ꂽ���������i�ނ��߂̃^�[�Q�b�g�ʒu��ݒ�
        targetPosition = transform.position + direction.normalized * moveDistance;
        isMoving = true;
    }

    // �w�肵���ʒu�������ʒu�ɋ߂����ǂ������`�F�b�N
    bool IsNearRestrictedPosition(Vector3 currentPosition, Vector3 restrictedPos)
    {
        return Mathf.Abs(currentPosition.x - restrictedPos.x) < tolerance &&
               Mathf.Abs(currentPosition.y - restrictedPos.y) < tolerance;
    }
}