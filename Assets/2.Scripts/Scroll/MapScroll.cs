using UnityEngine;

public class MapScroll : MonoBehaviour
{
    float speed = 2f;
    bool canMove = false;

    // 배경
    [SerializeField] Transform backGround1;
    [SerializeField] Transform backGround2;

    // 지면
    [SerializeField] Transform ground;

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    void Update()
    {
        if (canMove)
        {
            // 맵 스크롤
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // 배경
            if (backGround1.position.x <= -10.21f)
            {
                backGround1.position = new Vector2(10.21f, 0);
            }

            if (backGround2.position.x <= -10.21f)
            {
                backGround2.position = new Vector2(10.21f, 0);
            }

            // 지면
            if (ground.position.x < -6)
            {
                ground.position = Vector2.zero;
            }
        }
    }

    void HandleStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Play)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }
}
