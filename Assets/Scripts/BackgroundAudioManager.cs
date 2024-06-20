using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "AudioManager";
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    public AudioSource musicSource;
    public AudioClip[] musicClips;
    private int currentClipIndex = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        musicSource.loop = true;
        PlayNextClip();
    }

    public void PlayNextClip()
    {
        if (musicClips.Length == 0)
            return;

        musicSource.clip = musicClips[currentClipIndex];
        musicSource.Play();

        currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
    }
}