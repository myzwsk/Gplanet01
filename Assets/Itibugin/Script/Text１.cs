using UnityEngine;
using System.Collections; // コルーチンを使うために必要
using TMPro;

public class Text１ : MonoBehaviour
{
    public TMP_Text collisionText;
    public float displayDuration = 2.0f;
    private AudioSource audioSource;
    public AudioClip collisionSoundClip;

    private bool isReadyToPlay = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 【ここを強化】開始時に強制的にテキストを非表示にする
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(false);
            Debug.Log("テキストを初期状態で非表示にしました");
        }
        else
        {
            // もしここが出たら、インスペクターでテキストがセットされていません
            Debug.LogError("collisionText がアタッチされていません！");
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Stealth_Wall"))
        {
            if (isReadyToPlay)
            {
                isReadyToPlay = false;

                // 以前の処理を止めてから開始（二重表示防止）
                StopAllCoroutines();
                StartCoroutine(DisplayAndHideTextAndCoolDown());

                PlayCollisionSound();
            }
        }
    }

    void PlayCollisionSound()
    {
        if (audioSource != null && collisionSoundClip != null)
        {
            audioSource.PlayOneShot(collisionSoundClip);
        }
    }

    IEnumerator DisplayAndHideTextAndCoolDown()
    {
        Debug.Log("テキストを表示します");
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(displayDuration);

        Debug.Log("テキストを非表示にします");
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(false);
        }

        isReadyToPlay = true;
    }
}
