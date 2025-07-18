using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public static GameClear clear { get; private set; }

    private CursorController cc;

    [Header("UI")]
    public Image clearImage;

    public Sprite noRetryFastClearSprite;               //��3   Sprite
    public Sprite retryOrNoRetryClearSprite;            //��2   Sprite
    public Sprite retryClearSprite;                     //��1   Sprite

    [Header("�N���A�]���ݒ�")]
    public int minStepsToClear;                    // �ŒZ�X�e�b�v��
    public int marginSteps;                         //
    public int stepsTaken = 0;                          // ���݂̃X�e�b�v��

    public bool wasEat;
    public bool wasMaked;

    [Header("�Q�[�W�]���ݒ�")]
    [Tooltip("noRetryFastClear �����e����Q�[�W�����񐔁i��: 3�j")]
    public int allowedGaugeCount = 3;

    private void Awake()
    {
        if (clear == null) clear = this;
        else if (clear != null) Destroy(this.gameObject);
    }

    private void Start()
    {
        cc = CursorController.cc;
        wasEat = false;
        wasMaked = false;
    }

    // �X�e�b�v�������Z����
    public void AddStep()
    {
        stepsTaken++;
    }

    // �N���A���o�����s
    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);
        AudioManager.Instance.PlaySE("Game clear");
        cc.ChangeCursorEnable(true);

        // �Q�[���N���A�ς݃t���O�𗧂Ă� �� GameOver��h��
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        if (stepsTaken <= 4)
        {
            if (wasEat) clearImage.sprite = retryOrNoRetryClearSprite;
            else clearImage.sprite = noRetryFastClearSprite;
        }
        else if (stepsTaken >= 5)
        {
            if (wasEat) clearImage.sprite = retryClearSprite;
            else clearImage.sprite = retryOrNoRetryClearSprite;
        }
        else
        {
            clearImage.sprite = retryClearSprite;
        }

        //if (stepsTaken <= minStepsToClear && !wasEat)
        //{
        //    clearImage.sprite = noRetryFastClearSprite;
        //}
        //else if (stepsTaken <= minStepsToClear + marginSteps)
        //{
        //    clearImage.sprite = retryOrNoRetryClearSprite;
        //}
        //else if (stepsTaken >= minStepsToClear + marginSteps && wasEat)
        //{
        //    clearImage.sprite = retryClearSprite;
        //}
        ///// ���e�񐔈ȓ��Ȃ�ŒZ�]��������
        //bool allowFastClear = GaugeController.gaugeIncreaseCount <= allowedGaugeCount;

        //if (stepsTaken <= minStepsToClear && allowFastClear)
        //{
        //    clearImage.sprite = noRetryFastClearSprite;
        //}
        //else if (stepsTaken <= minStepsToClear + marginSteps)
        //{
        //    clearImage.sprite = retryOrNoRetryClearSprite;
        //}
        //else
        //{
        //    clearImage.sprite = retryClearSprite;
        //}
    }

    // ���U���g�V�[���Ɉړ�
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
