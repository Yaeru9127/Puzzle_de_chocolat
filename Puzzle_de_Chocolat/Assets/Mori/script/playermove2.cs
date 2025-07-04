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

        // ノード接続定義：双方向
        nodeConnections[0] = new List<int> { 1 };         // 座標1 → 座標2
        nodeConnections[1] = new List<int> { 0, 2 };      // 座標2 → 座標1,3
        nodeConnections[2] = new List<int> { 1, 3 };      // 座標3 → 座標2,4
        nodeConnections[3] = new List<int> { 2 };         // 座標4 → 座標3

        // 初期位置に移動
        transform.position = nodes[currentNodeIndex].position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) //EscとBボタン(仮)でタイトルへ戻る
        {
            Debug.Log("タイトルへ");
            fadeController.FadeOutAndLoadScene("stag");  // フェードアウトしてタイトルへ
        }
        if (!isMoving)
        {
            // キーボードやコントローラー入力処理
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

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))//スペースキーとAボタン(仮)でステージ選択
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
            Debug.Log("ステージ1");
            fadeController.FadeOutAndLoadScene("stag");
        }
        else if (currentNodeIndex == 1)
        {
            Debug.Log("ステージ2");
            // fadeController.FadeOutAndLoadScene("");
        }
        else if (currentNodeIndex == 2)
        {
            Debug.Log("ステージ3");
            // fadeController.FadeOutAndLoadScene("");
        }
        else if (currentNodeIndex == 3)
        {
            Debug.Log("ステージ4");
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