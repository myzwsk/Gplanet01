using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    AudioSource seSource;    // ワンショット専用
    AudioSource loopSource;  // ループ専用（シャワー）

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            seSource = gameObject.AddComponent<AudioSource>();
            seSource.loop = false;
            seSource.spatialBlend = 0f;

            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;
            loopSource.spatialBlend = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ===== ワンショットSE =====
    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        seSource.PlayOneShot(clip, volume);
    }

    // ===== ループSE（シャワー専用）=====
    public void PlayLoopSE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        // ★ 同じ音が鳴ってたら再生しない
        if (loopSource.isPlaying && loopSource.clip == clip)
            return;

        loopSource.clip = clip;
        loopSource.volume = volume;
        loopSource.Play();
    }

    public void StopLoopSE()
    {
        if (loopSource.isPlaying)
            loopSource.Stop();
    }
}
