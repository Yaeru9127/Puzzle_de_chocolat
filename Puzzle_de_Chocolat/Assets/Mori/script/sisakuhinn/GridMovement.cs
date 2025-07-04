using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector2Int currentGridPos;

    // routeMapはそのままにしておいて、進む方向を補正する
    private bool[,,] routeMap = new bool[4, 2, 4]
    {
        // x = 0 列
        {
            { false, false, false, false }, // y = 0
            { false, true, false, false } // y = 1
        },
        // x = 1 列
        {
            { false, true, false, true }, // y = 0
            { true, false, true, false } // y = 1
        },
        // x = 2 列
        {
            { true, true, false, true }, // y = 0
            { false, true, true, false } // y = 1
        },
        // x = 3 列
        {
            { false, false, false, true }, // y = 0
            { true, false, true, false } // y = 1
        }
    };

    void Start()
    {
        currentGridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            0  // y の初期位置を 0 に設定
        );

        // 範囲外にならないように制限をかける
        currentGridPos.x = Mathf.Clamp(currentGridPos.x, 0, 3);
        currentGridPos.y = Mathf.Clamp(currentGridPos.y, 0, 1);

        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // 移動中
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
        else
        {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            // 斜め移動でも許可（両方が0じゃない場合）
            if (horizontal != 0 || vertical != 0)
            {
                Vector2Int moveDir = new Vector2Int(horizontal, vertical);
                int dirIndex = -1;

                // 進行方向に対応するインデックスを設定
                if (horizontal == 1 && vertical == 1) dirIndex = 1; // 右上
                else if (horizontal == 1 && vertical == -1) dirIndex = 3; // 右下
                else if (horizontal == -1 && vertical == 1) dirIndex = 0; // 左上
                else if (horizontal == -1 && vertical == -1) dirIndex = 2; // 左下

                Vector2Int nextPos = currentGridPos + moveDir;

                // 範囲チェック（xは0～3, yは0～1）
                if (nextPos.x >= 0 && nextPos.x < 4 && nextPos.y >= 0 && nextPos.y < 2)
                {
                    // routeMap[currentGridPos.x, currentGridPos.y, dirIndex] が通行可能か確認
                    if (routeMap[currentGridPos.x, currentGridPos.y, dirIndex])
                    {
                        // 実際の移動：斜めに進む
                        currentGridPos = nextPos;

                        // 表示位置は斜め方向に進んでいるように見せる
                        targetPosition = new Vector3((currentGridPos.x + 0.5f) * gridSize, (currentGridPos.y + 0.5f) * gridSize, transform.position.z);
                        isMoving = true;

                        // デバッグログ
                        Debug.Log($"Moving to {currentGridPos.x}, {currentGridPos.y} - Direction: {dirIndex}");
                    }
                    else
                    {
                        // 通行不可な場合
                        Debug.Log("Cannot move in that direction.");
                    }
                }
            }
        }
    }
}
