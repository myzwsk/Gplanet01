using UnityEngine;

public class ShowerController : MonoBehaviour
{
    public ParticleSystem showerParticle;

    void Start()
    {
        // 念のため最初は止める
        showerParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartShower();

        if (Input.GetKeyDown(KeyCode.O))
            StopShower();
    }


    public void StartShower()
    {
        if (showerParticle.isPlaying) return;

        showerParticle.Play();
        Debug.Log("シャワー開始");
    }

    public void StopShower()
    {
        if (!showerParticle.isPlaying) return;

        showerParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Debug.Log("シャワー停止");
    }
}
