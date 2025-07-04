using UnityEngine;
using UnityEngine.InputSystem;

public class idou : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(3, 3);  // 3×3 のマス
    public float cellSize = 3f;                         // マス1つのサイズ
    public float moveTime = 0.1f;                       // 移動時間

    private Vector2Int playerPos;  // マス座標
    private bool isMoving = false;

    void Start()
    {
        playerPos = new Vector2Int(1, 1);  // 中央からスタート (1,1)
        transform.position = GridToWorld(playerPos);
    }

    void Update()
    {
        if (isMoving) return;

        Vector2 input = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        if (Gamepad.current != null)
            input = Gamepad.current.leftStick.ReadValue();
#endif
        input.x += Input.GetAxisRaw("Horizontal");
        input.y += Input.GetAxisRaw("Vertical");

        Vector2Int dir = Vector2Int.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y) && Mathf.Abs(input.x) > 0.5f)
            dir = new Vector2Int((int)Mathf.Sign(input.x), 0);
        else if (Mathf.Abs(input.y) > 0.5f)
            dir = new Vector2Int(0, (int)Mathf.Sign(input.y));

        if (dir != Vector2Int.zero)
        {
            Vector2Int nextPos = playerPos + dir;
            if (IsInsideGrid(nextPos))
            {
                StartCoroutine(MoveTo(nextPos));
            }
        }
    }

    System.Collections.IEnumerator MoveTo(Vector2Int newPos)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = GridToWorld(newPos);
        float t = 0f;

        while (t < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        playerPos = newPos;
        isMoving = false;
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0f);
    }

    bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
    }
}
