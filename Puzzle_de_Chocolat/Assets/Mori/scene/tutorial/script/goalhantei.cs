using UnityEngine;
using UnityEngine.SceneManagement;

public class goalhantei : MonoBehaviour
{
    // �ړ��������V�[�����i���ڂ����Ŏw��j
    private string sceneToLoad = "tutorial2";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
