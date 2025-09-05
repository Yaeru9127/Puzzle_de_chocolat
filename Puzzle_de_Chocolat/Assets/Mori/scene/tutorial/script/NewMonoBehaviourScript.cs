using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    // ノード（マス）の配列をインスペクターから設定できるように
    public Transform[] nodes;

    // 各ノードの接続先ノードのインデックス（インスペクターで設定）
    [System.Serializable]
    public class NodeConnection
    {
        public int nodeIndex;  // 現在のノード
        public List<int> connectedNodes;  // 接続されているノードのリスト
    }

    public NodeConnection[] nodeConnections;

    private int currentNodeIndex = 0;  // 現在のノードのインデックス
    private int targetNodeIndex = -1;  // 目標ノードのインデックス

    public float moveSpeed = 5f;  // 移動速度
    private bool isMoving = false;  // 移動中かどうか

    private Vector3 targetPosition;  // 目標位置

    // アニメーション用に公開
    public bool IsMoving => isMoving;

    // 移動方向を計算するプロパティ
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
        // 何も接続されていないノード接続を空にする
        if (nodeConnections.Length == 0)
        {
            Debug.LogError("ノード接続が設定されていません！インスペクターでノード接続を設定してください。");
        }
        transform.position = new Vector3(6.75f, 2.73f, -1f);  // 初期位置設定
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();  // 入力処理
        }
        else
        {
            MoveToTarget();  // 移動処理
        }
    }

    // 入力処理
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

        // 現在のノードから移動可能な方向を決定
        foreach (var connection in nodeConnections)
        {
            if (connection.nodeIndex == currentNodeIndex)
            {
                // 入力に応じて移動する方向を選択
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

    // 上方向への移動処理
    void TryMoveUp(int direction)
    {
        targetNodeIndex = currentNodeIndex + direction;

        if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
        {
            Vector3 basePos = nodes[targetNodeIndex].position;
            targetPosition = new Vector3(basePos.x, basePos.y, basePos.z); // 目標位置設定

            isMoving = true;
        }
    }

    // 下方向への移動処理
    void TryMoveDown(int direction)
    {
        targetNodeIndex = currentNodeIndex + direction;

        if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
        {
            Vector3 basePos = nodes[targetNodeIndex].position;
            targetPosition = new Vector3(basePos.x, basePos.y, basePos.z); // 目標位置設定

            isMoving = true;
        }
    }

    // 目標位置に向かって移動
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 目標位置に到達した場合
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            currentNodeIndex = targetNodeIndex;
            targetNodeIndex = -1;
            isMoving = false;
        }
    }
}
