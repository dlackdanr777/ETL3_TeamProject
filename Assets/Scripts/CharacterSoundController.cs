using UnityEngine;



public class CharacterSoundController : MonoBehaviour
{
    [Space]
    [Header("Components")]
    [SerializeField] private AudioSource _footStepSource;
    [SerializeField] private AudioSource _weaponSource;
    [SerializeField] private AudioSource _bodySource;


    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private AudioClip _attack1Sound;
    [SerializeField] private AudioClip _attack2Sound;
    [SerializeField] private AudioClip _attack3Sound;
    [SerializeField] private AudioClip _attack4Sound;
    [SerializeField] private AudioClip _guardSound;


    public void PlaySound(CharacterSoundType type)
    {
        switch (type)
        {
            case CharacterSoundType.FootStepWalk:
                _footStepSource.clip = _walkSound;
                _footStepSource.Play();
                break;

            case CharacterSoundType.Dash:
                _footStepSource.clip = _dashSound;
                _footStepSource.Play();
                break;

            case CharacterSoundType.Hit:
                int randInt = UnityEngine.Random.Range(0, _hitSounds.Length);
                _bodySource.clip = _hitSounds[randInt];
                _bodySource.Play();
                break;

            case CharacterSoundType.Attack1:
                _weaponSource.clip = _attack1Sound;
                _weaponSource.Play();
                break;

            case CharacterSoundType.Attack2:
                _weaponSource.clip = _attack2Sound;
                _weaponSource.Play();
                break;

            case CharacterSoundType.Attack3:
                _weaponSource.clip = _attack3Sound;
                _weaponSource.Play();
                break;

            case CharacterSoundType.Attack4:
                _weaponSource.clip = _attack4Sound;
                _weaponSource.Play();
                break;

            case CharacterSoundType.Guard:
                _weaponSource.clip = _guardSound;
                _weaponSource.Play();
                break;
        }
    }
}
