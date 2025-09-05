using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ugoku : MonoBehaviour
{
    [Header("ノードの設定")]
    public Transform[] nodes;  // ノード（マス）の配列

    [System.Serializable]
    public class NodeConnectionData
    {
        public int nodeIndex;  // 現在のノード
        public List<int> connectedNodes;  // 接続されているノードのリスト
    }

    [Header("ノード接続設定")]
    public NodeConnectionData[] nodeConnections;  // ノード接続情報

    private int currentNodeIndex = 0;  // 現在のノードインデックス
    private int targetNodeIndex = -1;  // 目標ノードインデックス

    [Header("移動設定")]
    public float speed = 5f;  // 移動速度（moveSpeedから変更）
    private bool isMoving = false;  // 移動中かどうか

    private Vector3 targetPosition;  // 目標位置

    // アニメーション用に公開
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

        // ノード接続に基づき移動処理
        foreach (var connection in nodeConnections)
        {
            if (connection.nodeIndex == currentNodeIndex)
            {
                // 各方向に移動可能かチェック
                if (TryMoveIfValid(currentNodeIndex + 1, padRight, KeyCode.RightArrow))  // 右
                {
                    TryMove(1);
                }
                else if (TryMoveIfValid(currentNodeIndex - 1, padLeft, KeyCode.LeftArrow))  // 左
                {
                    TryMove(-1);
                }
                else if (TryMoveIfValid(currentNodeIndex + 3, padUp, KeyCode.UpArrow))  // 上
                {
                    TryMove(3);
                }
                else if (TryMoveIfValid(currentNodeIndex - 3, padDown, KeyCode.DownArrow))  // 下
                {
                    TryMove(-3);
                }
                break;
            }
        }
    }

    // 移動可能かどうかをチェック
    private bool TryMoveIfValid(int targetIndex, bool padInput, KeyCode keyInput)
    {
        return (padInput || Input.GetKeyDown(keyInput)) && IsValidMove(targetIndex);
    }

    // 移動が有効かチェック
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

    // 移動を実行
    private void TryMove(int direction)
    {
        targetNodeIndex = currentNodeIndex + direction;

        // ノードの範囲内であれば移動
        if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
        {
            Vector3 basePos = nodes[targetNodeIndex].position;
            targetPosition = new Vector3(basePos.x, basePos.y, basePos.z);  // 目標位置設定

            isMoving = true;  // 移動フラグを立てる
        }
    }

    // 目標位置に向かって移動
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

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
