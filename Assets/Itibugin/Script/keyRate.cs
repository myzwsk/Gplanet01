using UnityEngine;
using System.Collections;
public class keyRate : MonoBehaviour
{
    // 公開変数として回転速度を定義します。
    // Unityエディター上から値を変更できます。
    public float rotationSpeed = 50f;
    public ParticleSystem destructionParticle;
    private const string TargetDestroyTag = "KeyText";
    // Update関数は毎フレーム呼び出されます
    void Update()
    {
        // オブジェクト自身のY軸（垂直方向）を中心に回転させる
        // rotationSpeed * Time.deltaTime を掛けることで、
        // 異なるPC環境でも同じ速さで回転するようになります（フレームレート依存の解消）。
        
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
    // 物理的な衝突が発生したときに一度だけ呼ばれる関数
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突してきたオブジェクトのタグが「Player」であるかを確認
        // collision引数には、衝突に関する詳細情報が含まれています。
        if (collision.gameObject.CompareTag("Player"))
        {
            // Playerとの衝突が確認された場合
            DestroyAllObjectsWithTag(TargetDestroyTag);
            // このGameObject（オブジェクト自体）を破壊してシーンから消去
            Destroy(gameObject);
            Destroy(destructionParticle);
        }
    }
    private void DestroyAllObjectsWithTag(string tagToDestroy)
    {
        // シーン内のすべての GameObject の中から指定されたタグが付いているものを探す
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tagToDestroy);

        if (objectsToDestroy.Length == 0)
        {
            Debug.Log("シーン内に " + tagToDestroy + " のオブジェクトは見つかりませんでした。");
            return;
        }

        // 見つかったすべてのオブジェクトに対してループ処理を行い、破棄する
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        Debug.Log("シーン内のすべての " + tagToDestroy + " オブジェクト (" + objectsToDestroy.Length + "個) を破棄しました。");
    }
}

