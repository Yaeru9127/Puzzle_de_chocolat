using UnityEngine;

public class PlayerReferenceHolder : MonoBehaviour
{
    private GameObject currentPlayer;

    private void OnEnable()
    {
        TestPlayer.OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        TestPlayer.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        currentPlayer = player;
    }

    /// <summary>
    /// 現在有効なプレイヤーを返す。無ければnull。
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentPlayer()
    {
        if (currentPlayer == null) return null;

        // Unity の MissingReferenceException はオブジェクト破棄後も参照が null でないことがあるため
        // bool 演算子のオーバーロードを利用してチェック
        if (!currentPlayer)
        {
            currentPlayer = null;
            return null;
        }

        return currentPlayer;
    }
}
