using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private float _footstepDelay = 0.3f; // ћинимальное врем€ между шагами
    private float _lastFootstepTime;
    private bool _isRightFoot;

    public AudioSource audioSource;
    public AudioClip FootstepSound; // звук шагов
    public AudioClip HitSound; 
    public AudioClip TakeHitSound; 
    public AudioClip DeathSound; 
    public AudioClip TakeItemSound; 
    public AudioClip UseCheckPoint; 

    public static PlayerSounds Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayFootstep()
    {
        if (Time.time - _lastFootstepTime < _footstepDelay) return;

        _lastFootstepTime = Time.time;
        _isRightFoot = !_isRightFoot; 

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(FootstepSound);
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(HitSound);
    }
    public void PlayTakeHit()
    {
        audioSource.PlayOneShot(TakeHitSound);
    }

    public void PlayDeath()
    {
        audioSource.PlayOneShot(DeathSound);
    }

    public void PlayTakeItemSound()
    {
        audioSource.PlayOneShot(TakeItemSound);
    }

    public void PlayUseCheckPoint()
    {
        audioSource.PlayOneShot(UseCheckPoint);
    }
}
