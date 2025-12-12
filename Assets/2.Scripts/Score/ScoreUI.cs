using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TMP_Text currentScoreText; // 현재 점수 텍스트
    [SerializeField] private TMP_Text bestScoreText; // 최고 점수 텍스트

    int bestScore = 0;

    private void OnEnable()
    {
        // 점수 변경 이벤트 구독 (UI 활성화될 때 등록)
        ScoreManager.OnScoreChanged += HandleScoreChanged;

        // 시작 시 표시 초기화 (씬 시작 시 0점 출력)
        if (ScoreManager.Instance != null)
            HandleScoreChanged(ScoreManager.Instance.CurrentScore);
    }

    private void OnDisable()
    {
        // 이벤트 등록 해제 (UI 비활성화 시 중복 방지)
        ScoreManager.OnScoreChanged -= HandleScoreChanged;
    }

    // 점수 값이 바뀔 때마다 호출되어 텍스트를 갱신 
    private void HandleScoreChanged(int value)
    {
        if (currentScoreText != null)
            currentScoreText.text = value.ToString();
    }

    public void UpdateBestScore(int value)
    {
        bestScoreText.text = value.ToString();
    }
}
