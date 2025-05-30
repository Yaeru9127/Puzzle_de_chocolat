using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 tilepos;
    private Vector2[] direction = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilepos = this.gameObject.transform.position;
        GetNeighborTiles();
    }

    //ó◊ê⁄Ç∑ÇÈÉ}ÉXÇÃéÊìæä÷êî
    private void GetNeighborTiles()
    {
        foreach (var dir in direction)
        {
            Vector2 check = (Vector2)this.gameObject.transform.position + dir * 1;
            Collider2D[] hits = Physics2D.OverlapBoxAll(check, new Vector2(1, 1), 0);
            foreach (var hit in hits)
            {
                if (hit != null && hit.gameObject != this.gameObject)
                {
                    
                }
            }
        }
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
