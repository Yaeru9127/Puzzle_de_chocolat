using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playermove2 : MonoBehaviour
{
    private FadeController fadeController;  // フェードコントローラーの参照

    public Transform[] nodes; // ノード座標を設定
    private Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    private int currentNodeIndex = 0;
    private int targetNodeIndex = -1;

    public float moveSpeed = 5f;
    private bool isMoving = false;

    private Vector3 targetPosition;

    void Start()
    {
        // フェードコントローラーを取得
        fadeController = Object.FindFirstObjectByType<FadeController>();

        // 座標ごとにいける方向を指定
        nodeConnections[0] = new List<int> { 1 };         // 座標1 → 座標2
        nodeConnections[1] = new List<int> { 0, 2 };      // 座標2 → 座標1,3
        nodeConnections[2] = new List<int> { 1, 3 };      // 座標3 → 座標2,4
        nodeConnections[3] = new List<int> { 2 };         // 座標4 → 座標3

        // 初期位置を指定座標に設定
        transform.position = new Vector3(6.75f, 2.73f, -1f);
    }

    void Update()
    {
        // タイトルに戻る（EscキーまたはBボタン）
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("タイトルへ");
            fadeController.FadeOutAndLoadScene("stag");
        }

        if (!isMoving)
        {
            // コントローラー入力の取得
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

            // ノードごとの入力処理
            switch (currentNodeIndex)
            {
                case 0: // 座標1 → 左で進む
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft)
                    {
                        TryMove(1); // 進む
                    }
                    break;

                case 1: // 座標2 → 下で進む / 右で戻る
                    if (Input.GetKeyDown(KeyCode.DownArrow) || padDown)
                    {
                        TryMove(1); // 進む
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) || padRight)
                    {
                        TryMove(-1); // 戻る
                    }
                    break;

                case 2: // 座標3 → 左で進む / 上で戻る
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || padLeft)
                    {
                        TryMove(1); // 進む
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || padUp)
                    {
                        TryMove(-1); // 戻る
                    }
                    break;

                case 3: // 座標4 → 右で戻る
                    if (Input.GetKeyDown(KeyCode.RightArrow) || padRight)
                    {
                        TryMove(-1); // 戻る
                    }
                    break;
            }

            // ステージ選択（Spaceキー、"Fire1"、またはAボタン）
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
                Debug.Log("ステージ1");
                fadeController.FadeOutAndLoadScene("stag");
                break;
            case 1:
                Debug.Log("ステージ2");
                // fadeController.FadeOutAndLoadScene("");
                break;
            case 2:
                Debug.Log("ステージ3");
                // fadeController.FadeOutAndLoadScene("");
                break;
            case 3:
                Debug.Log("ステージ4");
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
