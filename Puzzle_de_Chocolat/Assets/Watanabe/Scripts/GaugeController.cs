using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeController : MonoBehaviour
{
    // �Q�[�W�Ƃ��Ďg�p���� UI �� Slider ���C���X�y�N�^�[����ݒ�
    public Slider gaugeSlider;

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
        // ���݂̃Q�[�W�l���擾
        float startValue = gaugeSlider.value;

        // ������̒l���v�Z�imaxValue�𒴂��Ȃ��悤��Clamp�Ő����j
        float endValue = Mathf.Clamp(startValue + amount, 0, maxValue);

        // �o�ߎ��Ԃ̏�����
        float elapsed = 0f;

        // increaseDuration �b�Ԃ����ăQ�[�W�����炩�ɕω�������
        while (elapsed < increaseDuration)
        {
            // �o�ߎ��Ԃ����Z
            elapsed += Time.deltaTime;

            // ���`��ԁiLerp�j�ŃQ�[�W�̒l���X�V
            gaugeSlider.value = Mathf.Lerp(startValue, endValue, elapsed / increaseDuration);

            // 1�t���[���ҋ@���Ă��玟�̃��[�v��
            yield return null;
        }

        // �ŏI�I�Ȓl�𐳊m�ɃZ�b�g�iLerp�̌덷��h�����߁j
        gaugeSlider.value = endValue;
    }
}
