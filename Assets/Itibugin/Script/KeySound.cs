using UnityEngine;

public class KeySound : MonoBehaviour
{
    private AudioSource audioSource;
    // 一度再生されたかどうかを記録するフラグ
    private bool hasPlayed = false; // ★ ここに警告が出ている

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 他のColliderがTriggerに入ったときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // Playerと接触したかをタグで判定
        // ★ hasPlayedが false（未再生）の場合にのみ、if文のブロックを実行する
        if (other.CompareTag("Player") && hasPlayed == false)
        {
            // 音を再生
            audioSource.Play();

            // 再生したので、フラグを true に設定する
            hasPlayed = true;
        }
    }
}
