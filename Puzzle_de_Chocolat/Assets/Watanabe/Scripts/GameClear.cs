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

    [Header("�Q�[�W�]���ݒ�")]
    [Tooltip("noRetryFastClear �����e����Q�[�W�����񐔁i��: 3�j")]
    public int allowedGaugeCount = 3;

    //public bool wasEat;
    public int wasEat =0;
    public bool wasMaked = false;


    //���َq�����ꂽ�L�^�����ǉ�
    public bool hasPannacotta = false;
    public bool hasThiramisu = false;

    public int star = 3;
    private void Awake()
    {
        if (clear == null) clear = this;
        else if (clear != null) Destroy(this.gameObject);
    }

    private void Start()
    {
        cc = CursorController.cc;
        //v wasEat = false;
        wasMaked = false;
        //���₵�܂���
        hasPannacotta = false;
        hasThiramisu = false;
    }
    //���َq�����ꂽ�Ƃ��ɌĂяo��w��ǉ�
    public void MadeSweets(string sweetsName)
    {
       if(sweetsName =="pannacotta")
        {
            hasPannacotta = true;
        }
       else if(sweetsName =="thiramisu")
        {
            hasThiramisu = true;
        }
            
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

        ////��������
        //int star = 3;
        //if (wasEat >= 1)
        //{
        //    star--;
        //    //Debug.Log("eat");
        //}
        //if (stepsTaken > 4)
        //{
        //    star--;
        //    //Debug.Log("step is over 4");
        //}
        //if (stepsTaken > 6)
        //{
        //    star--;
        //    //Debug.Log("step is over 6");
        //}
        //switch (star)
        //{
        //    case 0:
        //        clearImage.sprite = star0;
        //        break;
        //    case 1:
        //        clearImage.sprite = star1;
        //        break;
        //    case 2:
        //        clearImage.sprite = star2;
        //        break;
        //    case 3:
        //        clearImage.sprite = star3;
        //        break;
        //    default:
        //        clearImage.sprite = null;
        //        break;
        //}

        if (!hasPannacotta)
        {
            star--;
            //Debug.Log("eat");
        }
        if (!hasThiramisu)
        {
            star--;
            //Debug.Log("step is over 4");
        }
        if (wasEat >= 3)
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
    }

    // ���U���g�V�[���Ɉړ�
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }
}
