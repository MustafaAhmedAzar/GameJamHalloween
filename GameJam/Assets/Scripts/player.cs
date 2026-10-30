using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPos = transform.localPosition;
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveX(localPos.x + speed, 2);
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveX(localPos.x - speed, 2);
        }
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveX(localPos.y + speed, 2);
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            transform.DOLocalMoveX(localPos.y - speed, 2);
        }
    }
}
