using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    [Serializable]
    public class StageCondition
    {
        public List<int> stepPenalties;
        public List<string> requiredSweets;
        public int maxEat = 2;
    }

    public static GameClear clear { get; private set; }

    [Header("UI")]
    public Image clearImage;
    public Sprite star3;
    public Sprite star2;
    public Sprite star1;
    public Sprite star0;

    [Header("�Q�[���]���f�[�^")]
    [Tooltip("���݂̃X�e�[�W�ԍ��B")]
    public int currentStage = 1;
    public int stepsTaken = 0;
    public int wasEat = 0;
    public bool wasMaked = false;

    public List<StageCondition> stageConditions; 

    [HideInInspector]
    public List<string> madeSweets;

    private void Awake()
    {
        if (clear == null) 
        {
            clear = this; 
        }
        else if (clear != this)
        {
            Destroy(this.gameObject); 
        }

    }

    private void Start()
    {
        // ���X�g��������
        madeSweets = new List<string>();
        stepsTaken = 0;
        wasEat = 0;
        wasMaked = false;
    }

    /// <summary>
    /// ���َq�����ꂽ�Ƃ��ɌĂяo�����\�b�h
    /// </summary>
    public void MadeSweets(string sweetsName)
    {
        if (!madeSweets.Contains(sweetsName))
        {
            madeSweets.Add(sweetsName);
        }
    }

    /// <summary>
    /// �X�e�b�v�������Z����
    /// </summary>
    public void AddStep()
    {
        stepsTaken++;
    }

    /// <summary>
    /// �N���A���o�����s���A���̐����v�Z����
    /// </summary>
    public void  ShowClearResult()
    {
        clearImage.gameObject.SetActive(true);
        // AudioManager.Instance.PlaySE("Game clear");

        // Remainingaircraft�X�N���v�g��isGameCleared�t���O�𗧂Ă�
        if (Remainingaircraft.remain != null)
        {
            Remainingaircraft.remain.isGameCleared = true;
        }

        int star = 3;
        int stageIndex = currentStage - 1;

        if (stageIndex >= 0 && stageIndex < stageConditions.Count)
        {
            StageCondition currentCondition = stageConditions[stageIndex];

            // �K�{�̂��َq�̌��_
            foreach (string requiredSweet in currentCondition.requiredSweets)
            {
                if (!madeSweets.Contains(requiredSweet))
                {
                    star--;
                }
            }

            // wasEat�̌��_
            if (wasEat > currentCondition.maxEat)
            {
                star--;
            }

            // �X�e�b�v���̌��_
            foreach (int penaltyStep in currentCondition.stepPenalties)
            {
                if (stepsTaken > penaltyStep)
                {
                    star--;
                }
            }
        }

        // ���̐���0�����ɂȂ�Ȃ��悤�ɒ���
        if (star < 0)
        {
            star = 0;
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

    /// <summary>
    /// ���U���g�V�[���Ɉړ�
    /// </summary>
    public void LoadResultScene()
    {
        SceneManager.LoadScene("RetryScene");
    }

    /// <summary>
    /// �`���[�g���A����p�V�[���ǂݍ��݊֐�
    /// </summary>
    /// <param name="num"></param>
    public void TutorialSceneLoad(int num)
    {
        //�����Ŕ��f
        string scenename = "";
        switch (num)
        {
            case 1:
                scenename = "Tutorial1";
                break;
            case 2:
                scenename = "Tutorial2";
                break;
            case 3:
                scenename = "Tutorial3";
                break;
            case 4:
                scenename = "Tutorial4";
                    break;
        }

        //�V�[���ǂݍ���
        SceneManager.LoadScene(scenename);
    }
}