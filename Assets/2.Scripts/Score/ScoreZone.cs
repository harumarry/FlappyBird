using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("Bird 레이어만 인식하도록 설정")]
    [SerializeField] private LayerMask birdLayer;

    [Tooltip("획득 점수 (기본 1)")]
    [SerializeField] private int scoreValue = 1;

    // 같은 구간에서 중복 점수 방지용 플래그
    private bool _used;

    // 캐시된 Collider2D (isTrigger 보장)
    private Collider2D _col;

    private void Awake()
    {
        // Collider 가져오고 isTrigger 설정 강제
        _col = GetComponent<BoxCollider2D>();
        _col.isTrigger = true;
    }

    private void OnEnable()
    {
        // 풀링 재사용 대비해서 매번 새로 초기화
        _used = false;
        // _coll.enabled = true; // 필요 시 여기서 활성화 복구 가능
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 한 번 통과 처리된 트리거면 무시
        if (_used) return;

        // Bird 레이어만 인식하도록 필터링
        if ((birdLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        // 점수 추가
        ScoreManager.Add(scoreValue);

        // 중복 방지 플래그 설정
        _used = true;

        // (선택) 더 확실히 막을려면 즉시 Collider 비활성화
        // _coll.enabled = false;
    }
}
