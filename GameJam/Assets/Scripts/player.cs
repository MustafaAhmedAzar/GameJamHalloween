using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float speed = 2f;
    public float fallSpeed = 3f;
    public Animator animator;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityY = -fallSpeed;
        Vector3 localPos = transform.localPosition;
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveX(localPos.x + speed, 0.5f);
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveX(localPos.x - speed, 0.5f);
        }
        /*if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveY(localPos.y + speed, 0.5f);
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveY(localPos.y - speed, 0.5f);
        }*/
    }
}
