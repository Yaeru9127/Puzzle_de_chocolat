using UnityEngine;
using UnityEngine.SceneManagement;

public class goalhantei : MonoBehaviour
{
    public GameObject target;           // �v���C���[�I�u�W�F�N�g�i�^�O�ŒT���j
    public string nextSceneName = "";  // ���̃V�[����

    private bool goalReached = false;  // �S�[������ς݂��ǂ���

    void Update()
    {
        if (goalReached) return; // ���ɃS�[���ς݂Ȃ珈�����Ȃ�

        // target��null�܂��͔j������Ă���ꍇ�̓^�O�ŒT��
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player"); // "player"�^�O�Ŏ擾
            if (target == null)
            {
                // �v���C���[���܂����݂��Ȃ���Ώ������Ȃ�
                return;
            }
        }

        Vector2 playerPos = target.transform.position;
        Vector2 goalPos = transform.position;

        // �����_�덷�΍�Ɋۂ߂Ĕ�r
        //Vector2Int playerGridPos = Vector2Int.RoundToInt(playerPos);
        //Vector2Int goalGridPos = Vector2Int.RoundToInt(goalPos);

        if (playerPos == goalPos)
        {
            Debug.Log("�S�[���I");
            goalReached = true; // ��x��������

            SceneManager.LoadScene(nextSceneName);
        }
    }
}
