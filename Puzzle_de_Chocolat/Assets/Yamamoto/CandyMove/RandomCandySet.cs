using UnityEngine;
using System.Collections.Generic;

/*メモ
 * テスト用でランダムにお菓子を配置する*/
public class RandomCandySet : MonoBehaviour
{
    private List<Transform> tiles = new List<Transform>();
    [SerializeField] private GameObject potato;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomSet();
    }

    private void RandomSet()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            tiles.Add(this.gameObject.transform.GetChild(i));
        }

        int rand = Random.Range(0, tiles.Count);
        Instantiate(potato, tiles[rand].position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
