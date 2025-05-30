using UnityEngine;
using System.Collections.Generic;

/*����
 * �e�X�g�p�Ń����_���ɂ��َq��z�u����*/
public class RandomCandySet : MonoBehaviour
{
    private List<Transform> tiles = new List<Transform>();
    [SerializeField] private GameObject potato;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomSet();
    }

    //�i�e�X�g�j���َq�������_���ȃ^�C���ɐݒu����֐�
    private void RandomSet()
    {
        //�����̎q�I�u�W�F�N�g�̃^�C�������X�g�ɒǉ�
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            tiles.Add(this.gameObject.transform.GetChild(i));
        }

        //�����_���ȏꏊ�ɐݒu
        int rand = Random.Range(0, tiles.Count);
        Instantiate(potato, tiles[rand].position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
