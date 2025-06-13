using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeController : MonoBehaviour
{
    // �Q�[�W�Ƃ��Ďg�p���� UI �� Slider ���C���X�y�N�^�[����ݒ�
    public Slider gaugeSlider;

    // GameOver �𐧌䂷��N���X�ւ̎Q��
    public GameOverController gameOverController;

    // �Q�[�W�̍ő�l
    private int maxValue = 10;

    // �Q�[�W�𑝉�������̂ɂ����鎞�ԁi�b�j
    private float increaseDuration = 0.5f;

    // �I�u�W�F�N�g���j�󂳂ꂽ�Ƃ��ɌĂяo���֐�
    public void OnObjectDestroyed()
    {
        // �Q�[�W��1���₷�������R���[�`���ŊJ�n
        StartCoroutine(IncreaseGaugeSmoothly(1));
    }

    // �Q�[�W�����炩�ɑ���������R���[�`��
    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        if (gaugeSlider == null)
        {
            yield break;
        }

        // ���݂̃Q�[�W�l���擾
        float startValue = gaugeSlider.value;

        // ������̒l���v�Z�imaxValue�𒴂��Ȃ��悤��Clamp�Ő����j
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue);

        // �o�ߎ��Ԃ̏�����
        float elapsed = 0f;

        // increaseDuration �b�Ԃ����ăQ�[�W�����炩�ɕω�������
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;

            // ���`��ԁiLerp�j�ŃQ�[�W�̒l���X�V
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration);

            yield return null;
        }

        // �ŏI�I�Ȓl�𐳊m�ɃZ�b�g�iLerp�̌덷��h�����߁j
        gaugeSlider.value = endValue;

        // GameOver�����`�F�b�N�i�덷�l���j
        if (Mathf.Approximately(gaugeSlider.value, maxValue) && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }
}
