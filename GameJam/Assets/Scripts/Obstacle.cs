using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Extra poison effect in seconds (poisonous mushroom); 0 = just lose a life (sharp rock)
    public float slowDuration = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        player p = other.GetComponent<player>();
        if (p == null) return;
        p.TakeHit(slowDuration);
    }
}
