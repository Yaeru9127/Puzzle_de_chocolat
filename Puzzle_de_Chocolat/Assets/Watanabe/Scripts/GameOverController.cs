using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks; 

public class GameOverController : MonoBehaviour
{
    public static GameOverController over { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    private CursorController cc;
    private StageManager stage;

    public Image gameOverImage;          // GameOver�\���pUI
    public float fadeDuration = 1.5f;    // �t�F�[�h�C������
    public float waitBeforeRetry = 3.0f; // ���g���C�O�̑ҋ@����

    private bool isGameOver = false;

    private void Awake()
    {
        if (over == null) over = this;
        else if (over != null) Destroy(this.gameObject);
    }

    void Start()
    {
        if (gameOverImage != null)
        {
            // �����ɂ��Ĕ�\��
            gameOverImage.color = new Color(1, 1, 1, 0);
            gameOverImage.gameObject.SetActive(false);
        }

        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();
        cc = CursorController.cc;
        stage = StageManager.stage;
    }

    // GameOver���o���J�n
    public void ShowGameOver()
    {
        if (isGameOver) return; // ���łɎ��s���Ȃ疳��

        manager.PlayerOff(); // �v���C���[���얳��
        isGameOver = true;

        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(true);
            stage.phase = StageManager.Phase.Result;

            // �J�[�\���\��
            if (cc.instance != null)
                cc.instance.SetActive(true);

            FadeInAndRetry().Forget(); // �񓯊��ŊJ�n
        }
    }

    // �t�F�[�h�C�����Ĉ�莞�Ԍ�Ƀ��g���C�V�[����
    private async UniTaskVoid FadeInAndRetry()
    {
        float elapsed = 0f;
        Color color = gameOverImage.color;
        color.a = 0f;

        // �t�F�[�h�C������
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = alpha;
            gameOverImage.color = color;
            await UniTask.Yield(); // �t���[����҂�
        }

        // �t���\��
        color.a = 1f;
        gameOverImage.color = color;

        // ��莞�ԑҋ@
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitBeforeRetry));

        // ���g���C�V�[����
        SceneManager.LoadScene("RetryScene");
    }

    private void OnDestroy()
    {
        // �V���O���g�������i���̃V�[���ōĐ����\�ɂ���j
        if (over == this) over = null;
    }
}
