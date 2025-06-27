using UnityEngine;
using UnityEngine.UI;

public class GameClear
{
    public Image clearImage;  // �Q�[���N���A���ʂ�\������摜�iUI�j

    // ���ꂼ��̃N���A��Ԃɉ������X�v���C�g
    private  Sprite noRetryFastClearSprite;       // �@���g���C�Ȃ����ŒZ�N���A
    private  Sprite retryOrNoRetryClearSprite;    // �A���g���C���聕�ŒZ or ���g���C�Ȃ�
    private  Sprite retryClearSprite;             // �B���g���C����

    public int minStepsToClear = 10;  // �ŒZ�N���A�ɕK�v�Ȏ菇��
    public int minRetriesToClear = 2; // �Œ჊�g���C�񐔁i���̐��l�ȏ�Ȃ烊�g���C����ɂȂ�j

    ///public int retryCount = 0;  // ���g���C��
    public int stepsTaken = 0;  // �N���A�܂łɂ��������X�e�b�v��

    // ���g���C�񐔂�1���₷���\�b�h
    //public void AddRetry()
    //{
    //    retryCount++;
    //}

    // �X�e�b�v����1���₷���\�b�h
    public void AddStep()
    {
        stepsTaken++;
    }

    // �Q�[���N���A���ʂ�\�����郁�\�b�h
    public void ShowClearResult(int retry)
    {
        clearImage.gameObject.SetActive(true);  // �N���A���ʉ摜��\������

        // �ŒZ�N���A�ŁA���g���C�Ȃ��̏ꍇ
        if (retry == 0 && stepsTaken <= minStepsToClear)
        {
            clearImage.sprite = noRetryFastClearSprite;  // �ŒZ�N���A�Ń��g���C�Ȃ��X�v���C�g��ݒ�
        }
        // ���g���C�񐔂��Œ჊�g���C�񐔈ȏ�̏ꍇ�A���g���C����
        else if (retry >= minRetriesToClear || stepsTaken > minStepsToClear)
        {
            clearImage.sprite = retryClearSprite;  // ���g���C����X�v���C�g��ݒ�
        }
        // �ŒZ�N���A�ł͂Ȃ������g���C���Ȃ��ꍇ
        else
        {
            clearImage.sprite = retryOrNoRetryClearSprite;  // ���g���C����܂��͍ŒZ�N���A�ł͂Ȃ��X�v���C�g��ݒ�
        }
    }
}
