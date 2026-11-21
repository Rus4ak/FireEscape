using UnityEngine;

public enum MovingSoundType
{
    Walking,
    Swimming
}

public class PlayerSounds : MonoBehaviour
{
    [Header("Walking Sounds")]
    [SerializeField] private AudioSource _movingAudioSource;
    [SerializeField] private AudioClip _walkingSound;
    [SerializeField] private AudioClip _swimmingSound;

    [Header("Jumping Sounds")]
    [SerializeField] private AudioSource _jumpingAudioSource;
    [SerializeField] private AudioClip _startJumpingSound;
    [SerializeField] private AudioClip _endJumpingSound;

    public void PlayMovingSound(MovingSoundType soundType, bool isSprint)
    {
        _movingAudioSource.pitch = isSprint ? 1.2f : 1f;
        
        if (soundType == MovingSoundType.Walking)
            _movingAudioSource.resource = _walkingSound;

        else if (soundType == MovingSoundType.Swimming)
            _movingAudioSource.resource = _swimmingSound;

        if (!_movingAudioSource.isPlaying)
            _movingAudioSource.Play();
    }

    public void StopMovingSound()
    {
        if (_movingAudioSource.isPlaying)
            _movingAudioSource.Stop();
    }

    public void PlayStartJumpingSound()
    {
        _jumpingAudioSource.PlayOneShot(_startJumpingSound);
    }

    public void PlayEndJumpingSound()
    {
        if (!_jumpingAudioSource.isPlaying)
            _jumpingAudioSource.PlayOneShot(_endJumpingSound);
    }
}
