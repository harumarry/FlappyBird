using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    // 싱글톤 : 씬 어디서나 접근 가능하게
    public static ScoreManager Instance { get; private set; }

    [SerializeField] ScoreUI scoreUI;

    // 점수 변경 시 UI나 다른 시스템에 알림을 보내는 이벤트
    public static event Action<int> OnScoreChanged;

    // 현재 점수
    public int CurrentScore { get; private set; }
    public int BestScore {  get; private set; }
    

    private void Awake()
    {
        // 중복 방지 (씬에 ScoreManager 여러 개 있으면 삭제)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 시작 시 점수 0으로 초기화
        CurrentScore = 0;
    }

    private void Start()
    {
        BestScore = PlayerPrefs.GetInt("BestScore");
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    public static void Add(int amount)
    {
        if (Instance == null) return;

        // 점수 누적
        Instance.CurrentScore += amount;

        // 이벤트 발행 -> UI에서 구독 중이면 바로 반영됨
        OnScoreChanged?.Invoke(Instance.CurrentScore);
    }

    private void HandleStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            scoreUI.UpdateBestScore(ReturnBestScore());
        }
    }

    private int ReturnBestScore()
    {
        if(BestScore < CurrentScore)
        {
            BestScore = CurrentScore;

            PlayerPrefs.SetInt("BestScore", BestScore);
        }

        return BestScore;
    }
}
