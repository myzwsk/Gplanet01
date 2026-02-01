using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    AudioSource seSource;       // ワンショット用
    AudioSource loopSource;     // ★ シャワー用（ループ）

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 効果音用
            seSource = gameObject.AddComponent<AudioSource>();
            seSource.loop = false;
            seSource.spatialBlend = 0f;

            // ★ ループ音用（シャワー）
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;
            loopSource.spatialBlend = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 通常SE
    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        seSource.PlayOneShot(clip, volume);
    }

    // ★ ループSE（シャワーなど）
    public void PlayLoopSE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        loopSource.clip = clip;
        loopSource.volume = volume;
        loopSource.Play();
    }

    public void StopLoopSE()
    {
        loopSource.Stop();
    }
}
