using UnityEngine;

public class NodeArrow2D : MonoBehaviour
{
    public playermove2 playerMove;       // �ړ��X�N���v�g
    public GameObject arrowPrefabLeft;   // ���������
    public GameObject arrowPrefabRight;  // �E�������

    private GameObject leftArrowInstance;
    private GameObject rightArrowInstance;

    void Start()
    {
        if (playerMove == null || arrowPrefabLeft == null || arrowPrefabRight == null) return;

        Vector3 currentPos = playerMove.nodes[playerMove.currentNodeIndex].position;

        // ���E��󐶐�
        leftArrowInstance = Instantiate(arrowPrefabLeft, currentPos, Quaternion.identity, transform);
        rightArrowInstance = Instantiate(arrowPrefabRight, currentPos, Quaternion.identity, transform);

        UpdateArrows();
    }

    void Update()
    {
        // �v���C���[���ړ���������ʒu���X�V
        Vector3 currentPos = playerMove.nodes[playerMove.currentNodeIndex].position;
        leftArrowInstance.transform.position = currentPos;
        rightArrowInstance.transform.position = currentPos;

        UpdateArrows();
    }

    void UpdateArrows()
    {
        // 1�ԖڂȂ�E����\��
        if (playerMove.currentNodeIndex == 0)
        {
            leftArrowInstance.SetActive(false);
            rightArrowInstance.SetActive(true);
        }
        // 4�ԖڂȂ獶����\��
        else if (playerMove.currentNodeIndex == playerMove.nodes.Length - 1)
        {
            leftArrowInstance.SetActive(true);
            rightArrowInstance.SetActive(false);
        }
        else
        {
            leftArrowInstance.SetActive(true);
            rightArrowInstance.SetActive(true);
        }
    }
}
