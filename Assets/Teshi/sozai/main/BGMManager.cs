using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // BGM再生
    public void PlayBGM(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return; // 同じBGMなら何もしない

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    // BGM停止
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
