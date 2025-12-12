using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPooler : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;
    [SerializeField] Transform container;
    [SerializeField] int poolSize = 5; // 미리 만둘어둘 오브젝트 개수

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        // 초기 풀 생성
        for (int i=0; i<poolSize; i++)
        {
            GameObject rock = Instantiate(rockPrefab, container);
            rock.SetActive(false);
            pool.Enqueue(rock);
        }
    }

    public GameObject GetRock(Vector3 position)
    {
        GameObject rock;

        // 만약 풀에 비활성화된 오브젝트가 있으면 꺼내서 재사용
        if (pool.Count > 0)
        {
            rock = pool.Dequeue();
        }

        else
        {
            // 혹시 부족할 때 대비해 하나 새로 만든다 (유연성)
            rock = Instantiate(rockPrefab, container);
        }

        rock.transform.position = position;
        rock.SetActive(true);
        return rock;
    }

    public void ReturnRock(GameObject rock)
    {
        rock.SetActive(false);
        pool.Enqueue(rock);
    }
}
