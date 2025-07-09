using UnityEngine;
using UnityEngine.SceneManagement;

public class goalhantei : MonoBehaviour
{
    public GameObject target;              // プレイヤーオブジェクトをインスペクターで指定
    public string nextSceneName = ""; // 切り替えたいシーン名を指定

    private bool goalReached = false;      // ゴール到達フラグ（1回だけ処理させるため）

    void Update()
    {
        if (goalReached) return; // すでにゴールしていたらスキップ

        if (target != null)
        {
            Vector2 playerPos = target.transform.position;
            Vector2 goalPos = transform.position;

            // 小数点の誤差対策：整数マスに丸めて比較
            Vector2Int playerGridPos = Vector2Int.RoundToInt(playerPos);
            Vector2Int goalGridPos = Vector2Int.RoundToInt(goalPos);

            if (playerGridPos == goalGridPos)
            {
                Debug.Log("ゴール！");
                goalReached = true; // 二重処理を防止

                // シーン切り替え
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
