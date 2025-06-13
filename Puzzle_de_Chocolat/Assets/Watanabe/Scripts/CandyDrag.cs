using UnityEngine;

public class CandyDrag : MonoBehaviour
{
    // �}�E�X�ƃI�u�W�F�N�g�̋�����ێ����邽�߂̃I�t�Z�b�g
    private Vector3 offset;

    // �}�E�X���N���b�N�����Ƃ��̃��[���h���W
    private Vector3 mouseDownPos;

    // ���݃h���b�O�����ǂ����̃t���O
    private bool isDragging = false;

    // �Ō�̑��삪�h���b�O���ǂ������O������Q�Ƃł���v���p�e�B
    public bool IsDragged { get; private set; } = false;

    // �c�@�Ǘ��X�N���v�g�̎Q�Ɓi�C���X�y�N�^�[����A�^�b�`�j
    public Remainingaircraft remainingAircraft;

    // �}�E�X�{�^�����������Ƃ��ɌĂ΂��
    void OnMouseDown()
    {
        // �}�E�X�̌��݈ʒu�i���[���h���W�j���擾
        mouseDownPos = GetMouseWorldPos();

        // �}�E�X�ʒu�ƃI�u�W�F�N�g�ʒu�̍����I�t�Z�b�g�Ƃ��ĕۑ�
        offset = transform.position - mouseDownPos;

        // �h���b�O��Ԃ�������
        isDragging = false;
        IsDragged = false;
    }

    // �}�E�X���h���b�O���Ă���ԁA���t���[���Ă΂��
    void OnMouseDrag()
    {
        // ���݂̃}�E�X�ʒu���擾
        Vector3 currentMousePos = GetMouseWorldPos();

        // �I�u�W�F�N�g���}�E�X�̈ʒu�{�I�t�Z�b�g�Ɉړ�������
        transform.position = currentMousePos + offset;

        // �h���b�O����F���ȏ�}�E�X����������h���b�O�Ɣ���
        if (!isDragging && Vector3.Distance(currentMousePos, mouseDownPos) > 0.1f)
        {
            isDragging = true;
            IsDragged = true;
        }
    }

    // �}�E�X�{�^���𗣂����Ƃ��ɌĂ΂��
    void OnMouseUp()
    {
        // �h���b�O���s���Ă����ꍇ�A�c�@�����炷
        if (isDragging && remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        // �h���b�O��Ԃ����Z�b�g
        isDragging = false;
    }

    // �}�E�X�̃X�N���[�����W�����[���h���W�ɕϊ�����
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;

        // Z���W���J��������̋����ɐݒ�
        mousePos.z = -Camera.main.transform.position.z;

        // �X�N���[�����W�����[���h���W�ɕϊ����ĕԂ�
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
