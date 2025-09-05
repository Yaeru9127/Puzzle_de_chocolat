using UnityEngine;

public class NodeArrow2D : MonoBehaviour
{
    public playermove2 playerMove;       // 移動スクリプト
    public GameObject arrowPrefabLeft;   // 左向き矢印
    public GameObject arrowPrefabRight;  // 右向き矢印

    private GameObject leftArrowInstance;
    private GameObject rightArrowInstance;

    void Start()
    {
        if (playerMove == null || arrowPrefabLeft == null || arrowPrefabRight == null) return;

        Vector3 currentPos = playerMove.nodes[playerMove.currentNodeIndex].position;

        // 左右矢印生成
        leftArrowInstance = Instantiate(arrowPrefabLeft, currentPos, Quaternion.identity, transform);
        rightArrowInstance = Instantiate(arrowPrefabRight, currentPos, Quaternion.identity, transform);

        UpdateArrows();
    }

    void Update()
    {
        // プレイヤーが移動したら矢印位置を更新
        Vector3 currentPos = playerMove.nodes[playerMove.currentNodeIndex].position;
        leftArrowInstance.transform.position = currentPos;
        rightArrowInstance.transform.position = currentPos;

        UpdateArrows();
    }

    void UpdateArrows()
    {
        // 1番目なら右矢印非表示
        if (playerMove.currentNodeIndex == 0)
        {
            leftArrowInstance.SetActive(false);
            rightArrowInstance.SetActive(true);
        }
        // 4番目なら左矢印非表示
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
