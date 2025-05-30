using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 tilepos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilepos = this.gameObject.transform.position;
    }

    public Vector2 GetTilePos()
    {
        return tilepos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
