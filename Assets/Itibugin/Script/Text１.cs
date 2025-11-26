using UnityEngine;
using System.Collections; // コルーチンを使うために必要
using TMPro;

public class Text１ : MonoBehaviour
{
    public TMP_Text collisionText;

    // テキストを表示する時間（秒）
    public float displayDuration = 2.0f;
    private AudioSource audioSource;
    public AudioClip collisionSoundClip;

    private bool isReadyToPlay = true;
    // 衝突を検出したときに一度だけ呼ばれる関数
    void Start()
    {
        // ゲーム開始時に、このオブジェクトにアタッチされているAudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceコンポーネントが見つかりません。プレイヤーにアタッチしてください！");
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 衝突した相手が「Wall」タグを持っているか確認
        if (hit.gameObject.CompareTag("Stealth_Wall"))
        {
            if (isReadyToPlay)
            {
                // フラグをfalseにし、連続再生をブロック
                isReadyToPlay = false;

                // 以前のコルーチンを停止し、テキスト表示と音の再生を開始
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
            // PlayOneShotを使うことで、他の音が鳴っていても重ねて再生できる
            audioSource.PlayOneShot(collisionSoundClip);
        }
    }
    // コルーチン（一定時間待機する処理などに使う）
    // ★★★ 修正：テキスト非表示後にクールダウン処理を追加したコルーチン ★★★
    IEnumerator DisplayAndHideTextAndCoolDown()
    {
        // 1. テキストを表示
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(true);
        }

        // 2. 指定された時間だけ待機 (テキスト表示時間)
        // この間はisReadyToPlayがfalseなので、音は鳴らない
        yield return new WaitForSeconds(displayDuration);

        // 3. テキストを非表示
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(false);
        }

        // ★★★ クールダウン終了：フラグをtrueに戻す ★★★
        isReadyToPlay = true;
    }
}
