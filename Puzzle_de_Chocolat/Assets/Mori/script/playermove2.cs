using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermove2 : MonoBehaviour
{
    private FadeController fadeController;
    private CharacterAnimation characterAnimation;
    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    private StageManager stage;

    public Transform[] nodes; // ノード座標
    public Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    public int currentNodeIndex = 0;
    private int targetNodeIndex = -1;

    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Vector3 targetPosition;

    [Header("シーンに置いた矢印オブジェクト")]
    public GameObject arrowLeft;   // 左矢印（シーンに配置済み）
    public GameObject arrowRight;  // 右矢印（シーンに配置済み）

    void Start()
    {
        fadeController = Object.FindFirstObjectByType<FadeController>();
        characterAnimation = GetComponent<CharacterAnimation>();
        manager = InputSystem_Manager.manager;
        stage = StageManager.stage;
        actions = manager.GetActions();
        manager.PlayerOff();

        if (Gamepad.all.Count > 0)
        {
            manager.MouseOff();
            manager.GamePadOn();
        }
        else if (Gamepad.all.Count == 0)
        {
            manager.GamePadOff();
            manager.MouseOn();
        }

        // ノード接続（直線移動）
        nodeConnections[0] = new List<int> { 1 };
        nodeConnections[1] = new List<int> { 0, 2 };
        nodeConnections[2] = new List<int> { 1, 3 };
        nodeConnections[3] = new List<int> { 2 };

        // 初期位置
        transform.position = nodes[currentNodeIndex].position;

        UpdateArrows();
    }

    void Update()
    {
        // Esc / Bボタンでタイトルへ戻る
        if (actions.Mouse.Back.WasPressedThisFrame() || actions.GamePad.Back.WasPressedThisFrame())
        {
            fadeController.FadeOutAndLoadScene("TitleScene");
        }

        Vector2 input = Vector2.zero;

        if (!isMoving)
        {
#if ENABLE_INPUT_SYSTEM
            Gamepad pad = Gamepad.current;
            if (pad != null) input = pad.leftStick.ReadValue();
#endif
            if (input == Vector2.zero)
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");
            }

            if (Mathf.Abs(input.x) > 0.5f)
            {
                TryMove(input.x > 0 ? -1 : 1); // 右キーは左に進む
            }

            //GamePad操作のとき
            if (actions.GamePad.enabled && !actions.Mouse.enabled)
            {
                if (actions.GamePad.Click.WasPressedThisFrame()) SceneChange();
            }
            //Keyboard操作のとき
            else if (!actions.GamePad.enabled && actions.Mouse.enabled)
            {
                if (actions.Mouse.Click.WasPressedThisFrame()) SceneChange(); 
            }

            characterAnimation.SetMove(input.normalized, false);
        }
        else
        {
            MoveToTarget();

            float dirX = targetNodeIndex - currentNodeIndex;
            if ((currentNodeIndex == 1 && targetNodeIndex == 2) ||
                (currentNodeIndex == 2 && targetNodeIndex == 1))
            {
                dirX *= -1f;
            }

            Vector2 dir = new Vector2(Mathf.Sign(dirX), 0f);
            characterAnimation.SetMove(dir, true);
        }

        UpdateArrows();
    }

    void SceneChange()
    {
        switch (currentNodeIndex)
        {
            case 0: fadeController.FadeOutAndLoadScene("Tutorial1");
                stage.stagenum = 0;
                break;
            case 1: fadeController.FadeOutAndLoadScene("Stage01");
                stage.stagenum = 1;
                break;
            case 2: fadeController.FadeOutAndLoadScene("Stage02");
                stage.stagenum = 2;
                break;
            case 3: fadeController.FadeOutAndLoadScene("Stage03");
                stage.stagenum = 3;
                break;
        }
    }

    void TryMove(int direction)
    {
        if (!nodeConnections.ContainsKey(currentNodeIndex)) return;

        foreach (int connectedNode in nodeConnections[currentNodeIndex])
        {
            if (connectedNode == currentNodeIndex + direction)
            {
                targetNodeIndex = connectedNode;
                targetPosition = nodes[targetNodeIndex].position;
                isMoving = true;
                break;
            }
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            currentNodeIndex = targetNodeIndex;
            targetNodeIndex = -1;
            isMoving = false;
        }
    }

    void UpdateArrows()
    {
        if (arrowLeft == null || arrowRight == null) return;

        if (currentNodeIndex == 0)
        {
            // 最初のノード → 右矢印を非表示
            arrowLeft.SetActive(true);
            arrowRight.SetActive(false);
        }
        else if (currentNodeIndex == nodes.Length - 1)
        {
            // 最後のノード → 左矢印を非表示
            arrowLeft.SetActive(false);
            arrowRight.SetActive(true);
        }
        else
        {
            // 中間ノード → 両方表示
            arrowLeft.SetActive(true);
            arrowRight.SetActive(true);
        }
    }
}
