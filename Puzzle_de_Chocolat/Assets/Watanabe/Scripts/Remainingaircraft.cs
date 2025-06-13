using System.Collections.Generic;
using UnityEngine;

public class Remainingaircraft : MonoBehaviour
{
    // �c�@��\���X�v���C�g�i�܂���UI�j��o�^���郊�X�g
    public List<GameObject> lifeSprites;

    // ���݂̎c�@��
    private int currentLife;

    // �Q�[���J�n���Ɏc�@��������
    void Start()
    {
        // �c�@�������X�g�̗v�f������ݒ�
        currentLife = lifeSprites.Count;
    }

    // �c�@��1���炷����
    public void ReduceLife()
    {
        if (currentLife > 0)
        {
            // �c�@����1���炷
            currentLife--;

            // �Ή�����X�v���C�g���\���ɂ���
            lifeSprites[currentLife].SetActive(false);

            // �c�@��0�ȉ��ɂȂ�����Q�[���I�[�o�[�������Ă�
            if (currentLife <= 0)
            {
                GameOver();
            }
        }
    }

    // �Q�[���I�[�o�[���̏���
    void GameOver()
    {
        Debug.Log("Game Over");

        // �K�v�ɉ����Ă����ɃV�[���J��
    }
}
