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
    /// ���ݗL���ȃv���C���[��Ԃ��B�������null�B
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentPlayer()
    {
        if (currentPlayer == null) return null;

        // Unity �� MissingReferenceException �̓I�u�W�F�N�g�j������Q�Ƃ� null �łȂ����Ƃ����邽��
        // bool ���Z�q�̃I�[�o�[���[�h�𗘗p���ă`�F�b�N
        if (!currentPlayer)
        {
            currentPlayer = null;
            return null;
        }

        return currentPlayer;
    }
}
