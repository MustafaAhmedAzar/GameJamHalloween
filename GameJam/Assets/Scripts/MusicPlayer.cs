using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip music;
    [Range(0f, 1f)] public float volume = 0.5f;

    void Start()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = music;
        source.volume = volume;
        source.loop = true;
        source.Play();
    }
}
