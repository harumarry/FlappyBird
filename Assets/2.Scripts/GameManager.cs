using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 전체 흐름을 관리하는 매니저
public class GameManager : MonoBehaviour
{ 
    public enum GameState
    {
        Ready,      // 시작 전 : 물리 멈춤, 첫 탭 대기
        Play,       // 실제 플레이 중 : 점프 가능, 점수 증가 등 
        GameOver    // 죽은 후 : 입력 잠금, Bird만 떨어짐
    }

    // 2) 현재 상태 보관
    public GameState currentState = GameState.Ready;

    // 새의 점프/물리 제어 스크립트
    [SerializeField] private BirdController bird;

    [SerializeField] private GameObject animationBackGround;
    [SerializeField] private GameObject sceneUI;

    [SerializeField] private GameObject State_Ready;
    [SerializeField] private GameObject State_GameOver;

    [SerializeField] private Button restartButton;

    // 상태 변경 시 방송할 이벤트
    public static event Action<GameState> OnStateChanged;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(Restart);
    }

    void Start()
    {
        // 게임 시작하면 무조건 Ready 상태부터
        SetState(GameState.Ready);

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // 상태별로 입력 처리와 흐름 제어
        switch (currentState)
        {
            case GameState.Ready:
                HandleReadyInput(); // 첫 탭 기다리는 중
                State_Ready.SetActive(true);
                State_GameOver.SetActive(false);
                break;

            case GameState.Play:
                HandlePlayInput(); // 점프 입력 받는 중
                State_Ready.SetActive(false);
                break;

            case GameState.GameOver:
                // GameOver 상태에서는 입력을 무시
                State_GameOver.SetActive(true);
                break;
        }
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(Restart);
        //exitButton.onClick.RemoveListener(Exit);
    }

    // Ready 상태일 때 입력 로직
    void HandleReadyInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 첫 입력으로 곧바로 Play 상태 진입
            SetState(GameState.Play);

            // 첫 점프 즉시 실행
            bird.Jump();


        }
    }

    // Play 상태일 때 입력 로직
    void HandlePlayInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bird.Jump();
        }
    }

    // 상태를 바꿀 때 호출하는 함수 (중복 전환 방지 포함)
    public void SetState(GameState newstate)
    {
        if (currentState == newstate)
            return; // 같은 상태로 다시 바꾸려고 하면 무시 -> 이중 호출 방지

        currentState = newstate;

        // 이벤트 발사 (모든 구독자에게 통보)
        OnStateChanged?.Invoke(newstate);

        Debug.Log($"[GameManager] 상태 전환 -> {newstate}");

        // 상태가 바뀔 때 해야 할 일 정리
        switch (newstate)
        {
            case GameState.Ready:
                // 게임 시작 전: 세상 멈춤
                Time.timeScale = 0f;        // 전체 물리 정지
                bird.SetKinematic(true);    // 새도 중력 안 받게 고정
                break;

            case GameState.Play:
                // 실제 게임 시작
                Time.timeScale = 1f;        // 물리 재개
                bird.SetKinematic(false);   // 새 물리 활성화 (중력 받음)
                break;

            case GameState.GameOver:
                // 죽은 이후
                Time.timeScale = 1f;    // 시간은 흐르게 둔다
                                        // (파이프는 나중에 따로 멈추게 하고, 새는 그냥 떨어지도록)
                bird.OnGameOver();      // 새는 더 이상 조작 불가 상태로 전환
                break;
        }

        Debug.Log($"상태 전환: {newstate}");
    }

    // BirdController 쪽에서 '죽었어'라고 알려줄 때 호출할 함수
    public void OnPlayerDead()
    {
        // 이미 GameOver 라면 또 GameOver 안 들어가게 보호
        if (currentState != GameState.GameOver)
        {
            SetState(GameState.GameOver);
        }
    }

    void Restart()
    {
        sceneUI.SetActive(false);
        bird.gameObject.SetActive(false);

        animationBackGround.transform.DOMoveX(-13, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("Main");
        });       
    }
}
