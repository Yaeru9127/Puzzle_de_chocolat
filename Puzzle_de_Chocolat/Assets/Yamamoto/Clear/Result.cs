using UnityEngine;

public class Result : MonoBehaviour
{
    private InputSystem_Manager manager;
    //private CursorController cc;
    private StageManager stage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cc = CursorController.cc;
        stage = StageManager.stage;
        manager = InputSystem_Manager.manager;
        stage.phase = StageManager.Phase.Result;

        //cc.ChangeCursorEnable(true);
    }

    public void LoadResultScene()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
