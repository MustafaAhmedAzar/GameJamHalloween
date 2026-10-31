using UnityEngine;

public class Crystal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        player p = other.GetComponent<player>();
        if (p == null) return;
        p.AddCrystal();
        Destroy(gameObject);
    }
}
