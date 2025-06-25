using UnityEngine;
using UnityEngine.SceneManagement;

public class playermove : MonoBehaviour
{
    public float moveDistance = 7f; // 一度の入力で進む距離
    public float moveSpeed = 5f; // 移動速度
    private bool isMoving = false; // 移動中かどうか
    private Vector3 targetPosition; // 移動先の位置

    public GameObject[] stage = new GameObject[4];  // Inspectorで4つのオブジェクトを割り当てる

    // 各座標の制限を設定
    private Vector3 restrictedPosition1 = new Vector3(-7.3f, -2.71f, 0); // 右上のみ
    private Vector3 restrictedPosition2 = new Vector3(-2.360252f, 2.299747f, 0); // 左下と右下のみ
    private Vector3 restrictedPosition3 = new Vector3(2.589495f, -2.65f, 0); // 左上と右上のみ
    //private Vector3 restrictedPosition4 = new Vector3(7.539243f, 2.299747f, 0); // 左下のみ

    // 許容範囲（誤差を考慮して）
    public float tolerance = 0.1f;

    private FadeController fadeController;  // フェードコントローラーの参照

    void Start()
    {
        // フェードコントローラーを取得
        fadeController = Object.FindFirstObjectByType<FadeController>();

        for (int i = 0; i < stage.Length; i++)
        {
            Vector3 pos = stage[i].transform.position;
            Debug.Log($"ステージオブジェクト{i + 1}の座標: {pos}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) // EscとBボタンでステージ選択
        {
            Debug.Log("タイトルへ");
            fadeController.FadeOutAndLoadScene("stag");  // フェードアウトしてステージ1へ
        }

        if (isMoving)
        {
            // 移動中は指定した位置に向かって進む
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 目的地に到達したら移動停止
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
        else
        {
            // 入力で移動開始
            HandleInput();
        }
    }

    void HandleInput()
    {
        // 現在の位置が制限された座標近くにあるか確認
        if (IsNearRestrictedPosition(transform.position, restrictedPosition1))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // スペースとAボタン(仮)でステージ選択
            {
                Debug.Log("ステージ1");
                fadeController.FadeOutAndLoadScene("stag");  // フェードアウトしてステージ1へ
            }
            // 右上のみ
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) &&
                (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0))
            {
                StartMove(Vector3.up + Vector3.right); // 右上
            }
        }
        else if (IsNearRestrictedPosition(transform.position, restrictedPosition2))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // スペースとAボタン(仮)でステージ選択
            {
                Debug.Log("ステージ2");
                //fadeController.FadeOutAndLoadScene("");  // フェードアウトしてステージ2へ
            }
            // 左下と右下のみ
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) &&
                (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0))
            {
                StartMove(Vector3.down + Vector3.left); // 左下
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) &&
                     (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0))
            {
                StartMove(Vector3.down + Vector3.right); // 右下
            }
        }
        else if (IsNearRestrictedPosition(transform.position, restrictedPosition3))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // スペースとAボタン(仮)でステージ選択
            {
                Debug.Log("ステージ3");
                //fadeController.FadeOutAndLoadScene("");  // フェードアウトしてステージ3へ
            }
            // 左上と右上のみ
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) &&
                (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0))
            {
                StartMove(Vector3.up + Vector3.left); // 左上
            }
            /*else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) &&
                     (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0))
            {
                StartMove(Vector3.up + Vector3.right); // 右上
            }*/
        }
        /*else if (IsNearRestrictedPosition(transform.position, restrictedPosition4))
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) // スペースとAボタン(仮)でステージ選択
            {
                Debug.Log("ステージ4");
                //fadeController.FadeOutAndLoadScene("");  // フェードアウトしてステージ4へ
            }
            // 左下のみ
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) &&
                (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0))
            {
                StartMove(Vector3.down + Vector3.left); // 左下
            }
        }*/
    }

    void StartMove(Vector3 direction)
    {
        // 現在位置から指定された距離だけ進むためのターゲット位置を設定
        targetPosition = transform.position + direction.normalized * moveDistance;
        isMoving = true;
    }

    // 指定した位置が制限位置に近いかどうかをチェック
    bool IsNearRestrictedPosition(Vector3 currentPosition, Vector3 restrictedPos)
    {
        return Mathf.Abs(currentPosition.x - restrictedPos.x) < tolerance &&
               Mathf.Abs(currentPosition.y - restrictedPos.y) < tolerance;
    }
}