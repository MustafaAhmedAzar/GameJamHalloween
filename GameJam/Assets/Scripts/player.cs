using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public Volume volume;
    public float vignetteIntensity = 0.45f;
    private Vignette vignette;
    public float speed = 2.8f;
    public float fallSpeed = 3f;
    // Inner edges of the rock walls in world X; the player's centre stays inside these
    public float leftBorder = -3.5f;
    public float rightBorder = 3.5f;
    public int lives = 3;
    // Seconds of blinking after a hit, during which obstacles can't hurt again
    public float invincibleTime = 1f;
    // How fast the player moves while poisoned (0.5 = half speed)
    public float slowMultiplier = 0.5f;
    public Animator animator;
    // Played every time the player switches lanes
    public AudioClip moveSound;
    [Range(0f, 1f)] public float moveSoundVolume = 1f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private float targetX;
    private float currentX;
    private Tween moveTween;
    private Tween blinkTween;
    private float slowTimer;
    private float invincibleTimer;
    private int crystals;
    private float Timer;
    public float levelTime = 240f;
    private int nextLevel = 1;

    private float SpeedFactor => slowTimer > 0f ? slowMultiplier : 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        targetX = currentX = rb.position.x;
        volume.profile.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= levelTime)
        {
            SceneManager.LoadScene(nextLevel);
            nextLevel++;
            fallSpeed++;
        }
        slowTimer -= Time.deltaTime;
        invincibleTimer -= Time.deltaTime;
        if (invincibleTimer <= 0f && vignette.intensity.value > 0f)
        {
            vignette.intensity.value = 0f;
        }
        animator.speed = SpeedFactor;

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            MoveTo(targetX + speed);
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            MoveTo(targetX - speed);
        }
    }

    // Both axes go through the Rigidbody so the fall never gets interrupted by the sideways tween
    void FixedUpdate()
    {
        Vector2 pos = rb.position;
        pos.x = currentX;
        pos.y -= fallSpeed * SpeedFactor * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }

    void MoveTo(float x)
    {
        // Ignore moves that would push the player into the rock walls
        if (x < leftBorder || x > rightBorder) return;
        targetX = x;
        moveTween?.Kill();
        // Tween only the X value; FixedUpdate applies it to the Rigidbody
        moveTween = DOTween.To(() => currentX, v => currentX = v, targetX, 0.5f / SpeedFactor);
        audioSource.PlayOneShot(moveSound, moveSoundVolume);
    }

    // Called by obstacles; slowDuration > 0 also poisons the player for that long
    public void TakeHit(float slowDuration, bool showVignette = false)
    {
        if (invincibleTimer > 0f) return;

        lives--;
        if (lives <= 0)
        {
            // Out of lives: restart the location from the beginning
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        invincibleTimer = invincibleTime;
        if (showVignette) vignette.intensity.value = vignetteIntensity;
        slowTimer = Mathf.Max(slowTimer, slowDuration);

        blinkTween?.Kill();
        sr.color = Color.white;
        int blinks = Mathf.RoundToInt(invincibleTime / 0.1f) / 2 * 2; // even count so it ends fully visible
        blinkTween = sr.DOFade(0.2f, 0.1f).SetLoops(blinks, LoopType.Yoyo);
    }

    public void AddCrystal()
    {
        crystals++;
    }

    void OnDestroy()
    {
        moveTween?.Kill();
        blinkTween?.Kill();
    }

    // Placeholder HUD until there's a proper UI
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 28 };
        GUI.Label(new Rect(20, 20, 600, 40), $"Lives: {lives}    Crystals: {crystals}", style);
    }

    // Shows the borders in the Scene view so they're easy to tune
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float y = transform.position.y;
        Gizmos.DrawLine(new Vector3(leftBorder, y - 20f), new Vector3(leftBorder, y + 20f));
        Gizmos.DrawLine(new Vector3(rightBorder, y - 20f), new Vector3(rightBorder, y + 20f));
    }
}
