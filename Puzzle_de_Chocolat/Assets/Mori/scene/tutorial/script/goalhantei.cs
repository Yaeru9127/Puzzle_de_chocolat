using UnityEngine;
using UnityEngine.SceneManagement;

public class goalhantei : MonoBehaviour
{
    public GameObject target;              // �v���C���[�I�u�W�F�N�g���C���X�y�N�^�[�Ŏw��
    public string nextSceneName = ""; // �؂�ւ������V�[�������w��

    private bool goalReached = false;      // �S�[�����B�t���O�i1�񂾂����������邽�߁j

    void Update()
    {
        if (goalReached) return; // ���łɃS�[�����Ă�����X�L�b�v

        if (target != null)
        {
            Vector2 playerPos = target.transform.position;
            Vector2 goalPos = transform.position;

            // �����_�̌덷�΍�F�����}�X�Ɋۂ߂Ĕ�r
            Vector2Int playerGridPos = Vector2Int.RoundToInt(playerPos);
            Vector2Int goalGridPos = Vector2Int.RoundToInt(goalPos);

            if (playerGridPos == goalGridPos)
            {
                Debug.Log("�S�[���I");
                goalReached = true; // ��d������h�~

                // �V�[���؂�ւ�
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
