using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    // Variables publicas
    public AudioClip bacgroundMusic;
    public AudioClip shootSFX;
    public AudioClip hitSFX;
    public AudioClip powerupSFX;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = bacgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
