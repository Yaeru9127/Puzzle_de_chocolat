using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public static GameClear clear { get; private set; }

    private CursorController cc;

    [Header("UI")]
    public Image clearImage;

    public Sprite star3;            //��3   Sprite
    public Sprite star2;            //��2   Sprite
    public Sprite star1;            //��1   Sprite
    public Sprite star0;            //��0   Sprite

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

        int star = 3;
        if (wasEat)
        {
            star--;
            //Debug.Log("eat");
        }
        if (stepsTaken > 4)
        {
            star--;
            //Debug.Log("step is over 4");
        }
        if (stepsTaken > 6)
        {
            star--;
            //Debug.Log("step is over 6");
        }
        switch (star)
        {
            case 0:
                clearImage.sprite = star0;
                break;
            case 1:
                clearImage.sprite = star1;
                break;
            case 2:
                clearImage.sprite = star2;
                break;
            case 3:
                clearImage.sprite = star3;
                break;
            default:
                clearImage.sprite = null;
                break;
        }

        ////�萔 <= 4    => 4��ȓ�
        //if (stepsTaken <= 4)
        //{
        //    if (wasEat) clearImage.sprite = star2;      //4��ȓ��ŐH�ׂ���
        //    else clearImage.sprite = star3;             //4��ȓ��ŐH�ׂĂȂ��Ȃ�
        //}
        ////6 <= �萔 >= 5
        //else if (stepsTaken >= 5 && stepsTaken <= 6)
        //{
        //    if (wasEat) clearImage.sprite = star1;      //5,6��ŐH�ׂ���
        //    else clearImage.sprite = star2;             //5,6��ŐH�ׂĂȂ��Ȃ�
        //}
        //else
        //{
        //    clearImage.sprite = star1;
        //}

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
