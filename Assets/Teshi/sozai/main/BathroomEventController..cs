using UnityEngine;
using System.Collections;

public class BathroomEventController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject bathroomLight;
    public ParticleSystem showerParticle;   // ★ ParticleSystem 本体
    public GameObject bathWater;
    public GameObject path;

    [Header("Water Settings")]
    public float verticalSpeed = 10f;
    public float horizontalSpeed = 3f;
    public float horizontalDelay = 0.5f;
    public Vector3 targetWaterScale = new Vector3(35f, 35f, 13f);

    [Header("Shower Settings")]
    public float showerMinRate = 200f;
    public float showerMaxRate = 1200f;

    bool isPlayed = false;

    public void PlayBathroomEvent()
    {
        if (isPlayed) return;
        isPlayed = true;

        if (bathroomLight != null)
            bathroomLight.SetActive(true);

        if (showerParticle != null)
        {
            var emission = showerParticle.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(showerMinRate);
            showerParticle.Play();
        }

        if (bathWater != null)
        {
            bathWater.transform.localScale = new Vector3(
                targetWaterScale.x * 0.2f,
                0.01f,
                targetWaterScale.z * 0.2f
            );

            bathWater.SetActive(true);
            StartCoroutine(FillBath());
        }

        if (path != null)
            path.SetActive(true);
    }

    IEnumerator FillBath()
    {
        Vector3 scale = bathWater.transform.localScale;
        float elapsed = 0f;

        while (
            scale.y < targetWaterScale.y ||
            scale.x < targetWaterScale.x ||
            scale.z < targetWaterScale.z
        )
        {
            float waterRate = Mathf.InverseLerp(0.01f, targetWaterScale.y, scale.y);

            scale.y = Mathf.MoveTowards(
                scale.y,
                targetWaterScale.y,
                verticalSpeed * Time.unscaledDeltaTime
            );

            if (elapsed >= horizontalDelay)
            {
                scale.x = Mathf.MoveTowards(
                    scale.x,
                    targetWaterScale.x,
                    horizontalSpeed * Time.unscaledDeltaTime
                );

                scale.z = Mathf.MoveTowards(
                    scale.z,
                    targetWaterScale.z,
                    horizontalSpeed * Time.unscaledDeltaTime
                );
            }

            bathWater.transform.localScale = scale;

            // ★ 安全な Emission 操作
            if (showerParticle != null)
            {
                var emission = showerParticle.emission;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(
                    Mathf.Lerp(showerMinRate, showerMaxRate, waterRate)
                );
            }

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        bathWater.transform.localScale = targetWaterScale;

        if (showerParticle != null)
            showerParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
