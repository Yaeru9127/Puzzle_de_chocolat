using UnityEngine;
using UnityEngine.SceneManagement;

public class goalhantei : MonoBehaviour
{
    public GameObject target;           // プレイヤーオブジェクト（タグで探す）
    public string nextSceneName = "";  // 次のシーン名

    private bool goalReached = false;  // ゴール判定済みかどうか

    void Update()
    {
        if (goalReached) return; // 既にゴール済みなら処理しない

        // targetがnullまたは破棄されている場合はタグで探す
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player"); // "player"タグで取得
            if (target == null)
            {
                // プレイヤーがまだ存在しなければ処理しない
                return;
            }
        }

        Vector2 playerPos = target.transform.position;
        Vector2 goalPos = transform.position;

        // 小数点誤差対策に丸めて比較
        //Vector2Int playerGridPos = Vector2Int.RoundToInt(playerPos);
        //Vector2Int goalGridPos = Vector2Int.RoundToInt(goalPos);

        if (playerPos == goalPos)
        {
            Debug.Log("ゴール！");
            goalReached = true; // 一度だけ処理

            SceneManager.LoadScene(nextSceneName);
        }
    }
}
