using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private float WidthHeight()
    {
        int num = this.gameObject.transform.childCount;
        float nums = Mathf.Sqrt(num);

        return nums;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
