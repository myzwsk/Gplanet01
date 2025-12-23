using UnityEngine;

public class ChangeSignal : MonoBehaviour
{
    [Header("マテリアルを変えたいオブジェクト")]
    public GameObject targetObject;

    [Header("新しいマテリアル")]
    public Material newMaterial;

    private Material originalMaterial;
    private Renderer targetRenderer;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (targetObject != null)
        {
            // 指定されたオブジェクトからRendererを取得
            targetRenderer = targetObject.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                // 元のマテリアルを保存（後で戻せるように）
                originalMaterial = targetRenderer.material;
            }
        }
    }

    // プレイヤーが「ここ（センサー）」に入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object01") && targetRenderer != null)
        {
            audioSource.Play();
            targetRenderer.material = newMaterial;
            Debug.Log(targetObject.name + " のマテリアルを変更しました");
        }
    }

    // プレイヤーが「ここ（センサー）」から出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object01") && targetRenderer != null)
        {
            // 元のマテリアルに戻す
            targetRenderer.material = originalMaterial;
            Debug.Log(targetObject.name + " のマテリアルを元に戻しました");
        }
    }
}
