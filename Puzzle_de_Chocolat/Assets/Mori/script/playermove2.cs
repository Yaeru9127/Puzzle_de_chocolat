using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playermove2 : MonoBehaviour
{
    public Transform[] nodes;
    private Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    private int currentNodeIndex = 0;
    private int targetNodeIndex = -1;

    public float moveSpeed = 5f;
    private bool isMoving = false;

    private Vector3 targetPosition;

    // 👇 アニメーション用に公開
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
        // ノード接続設定
        nodeConnections[0] = new List<int> { 1 };
        nodeConnections[1] = new List<int> { 0, 2 };
        nodeConnections[2] = new List<int> { 1, 3 };
        nodeConnections[3] = new List<int> { 2 };

        transform.position = new Vector3(6.75f, 2.73f, -1f);
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }
        else
        {
            MoveToTarget();
        }
    }

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

        // ノード移動方向
        switch (currentNodeIndex)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft) TryMove(1);
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.DownArrow) || padDown) TryMove(1);
                else if (Input.GetKeyDown(KeyCode.RightArrow) || padRight) TryMove(-1);
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft) TryMove(1);
                else if (Input.GetKeyDown(KeyCode.UpArrow) || padUp) TryMove(-1);
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.RightArrow) || padRight) TryMove(-1);
                break;
        }

        // 決定ボタンでステージを選択
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1") ||
            (pad != null && pad.buttonSouth.wasPressedThisFrame))
        {
            SceneChange();
        }
    }

    void SceneChange()
    {
        switch (currentNodeIndex)
        {
            case 0:
                StageManager.stage.SetStageNum(0);
                SceneManager.LoadScene("tutoriaru1"); 
                break;
            case 1:
                StageManager.stage.SetStageNum(1);
                //SceneManager.LoadScene("");
                Debug.Log("ステージ2");
                break;
            case 2:
                StageManager.stage.SetStageNum(2);
                //SceneManager.LoadScene("");
                Debug.Log("ステージ3");
                break;
            case 3:
                StageManager.stage.SetStageNum(3);
                //SceneManager.LoadScene("");
                Debug.Log("ステージ4");
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

                // オフセットを調整してノード位置に移動
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
