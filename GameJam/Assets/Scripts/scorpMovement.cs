using UnityEngine;

public class scorpMovement : MonoBehaviour
{
    public float speed = 2f;
    // World X limits it bounces between (match your wall borders)
    public float leftLimit = -3.5f;
    public float rightLimit = 3.5f;

    private int dir;

    void Start()
    {
        dir = Random.value <= 0.5f ? -1 : 1;
        Flip();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += dir * speed * Time.deltaTime;

        if (pos.x <= leftLimit)
        {
            pos.x = leftLimit;
            dir = 1;
            Flip();
        }
        else if (pos.x >= rightLimit)
        {
            pos.x = rightLimit;
            dir = -1;
            Flip();
        }

        transform.position = pos;
    }

    // Faces the sprite in its movement direction (assumes it faces right by default; swap the sign if not)
    void Flip()
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        transform.localScale = s;
    }
}