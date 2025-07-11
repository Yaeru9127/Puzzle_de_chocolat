using UnityEngine;

public class AllGameManager : MonoBehaviour
{
    public static AllGameManager agm { get; private set; }

    //ƒQ[ƒ€‚ÌƒV[ƒ“ó‘Ôenum
    public enum Phase
    {
        Title,
        Game,
        Pause,
        Result
    }
    public Phase phase;

    private void Awake()
    {
        if (agm == null) agm = this;
        else if (agm != null) Destroy(agm);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        phase = Phase.Title;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
