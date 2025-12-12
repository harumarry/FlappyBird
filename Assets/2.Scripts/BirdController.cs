using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float jumpVelocity;
    float startY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
    }

    private void Update()
    {
        if (!rb.simulated)
        {
            // Ready 상태에서만 idle 애니메이션 (위아래 살짝 흔들림)
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Sin(Time.time * 2f) * 0.05f + startY,
                transform.position.z
                );
        }
    }

    // 점프 동작: 위쪽 속도를 강제로 세팅
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    // Ready 상태에서 물리 비활성화할 때 사용
    public void SetKinematic(bool isOn)
    {
        // isKinematic은 3D 전용, 2D에서는 아래처럼, rb.simulated로 처리하는 것도 가능
        rb.simulated = !isOn;
        // isOn == true -> 시뮬레이션 끔 (멈춤)
        // isOn == false -> 시뮬레이션 켬 (중력 받기 시작)
    }

    // GameOver 상태 돌입 시 새가 조작 불가해지는 처리
    public void OnGameOver()
    {
        // 여기서는 입력은 GameManager에서 이미 막았으므로
        // 굳이 아무 것도 안 해도 된다.
        // 하지만 추후에 여기에서 "죽음 애니메이션", "회전 고정" 등 넣을 수 있다.
    }

    // 파이프나 땅에 부딪혔을 때 호출된다고 가정
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Rock"))
        {
            // GameManager에 "죽음"을 알린다
            FindObjectOfType<GameManager>().OnPlayerDead();
        }
        
    }
}
