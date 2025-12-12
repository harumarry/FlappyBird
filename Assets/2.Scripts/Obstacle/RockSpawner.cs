using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] RockPooler pooler; // pooler 참조
    [SerializeField] float spawnInterval = 1.5f;    // 몇 초마다 생성할지
    [SerializeField] float spawnX = 5f;             // 화면 오른쪽 바깥 스폰 위치
    [SerializeField] float minY = -1f;              // rockGroup 프리팹 최소 높이
    [SerializeField] float maxY = 1f;               // rockGroup 프리팹 최대 높이

    float timer = 0f;
    bool canSpawn = false; // Play 상태일 때만 true

    private void OnEnable()
    {
        // GameManager의 이벤트를 "구독"
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        // 씬 전환 시 오류 방지를 위해 "구독 해제"
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    // 상태가 바뀔 때마다 자동으로 호출됨
    void HandleStateChanged(GameManager.GameState newState)
    {
        canSpawn = (newState == GameManager.GameState.Play);

        Debug.Log($"[RockSpawner] 상태 감지 -> {newState}, canSpawn = {canSpawn}");
    }

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            float spawnY = Random.Range(minY, maxY);

            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

            // 풀에서 꺼내기
            pooler.GetRock(spawnPos);
        }
    }
}
