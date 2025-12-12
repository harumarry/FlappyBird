using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RockMover : MonoBehaviour
{
    [SerializeField] float speed = 2f; // 왼쪽으로 흐르는 속도
    [SerializeField] float destroyX = -10f; // 화면 왼쪽 바깥으로 충분히 나간 지점 (카메라보타 훨씬 왼쪽)

    RockPooler pooler;

    bool canMove = true;

    private void Start()
    {
        pooler = FindObjectOfType<RockPooler>();

        // 이벤트 구독
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        // 구독 해제 (메모리 누수 방지)
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    void HandleStateChanged(GameManager.GameState newState)
    {
        // Play 일 때만 움직임
        canMove = (newState == GameManager.GameState.Play);
    }

    void Update()
    {
        if (!canMove) return;

        // 왼쪽으로 이동
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 너무 왼쪽으로 가면 제거
        if (transform.position.x < destroyX)
        {
            // 풀로 반환
            pooler.ReturnRock(gameObject);
        }
    }
}
