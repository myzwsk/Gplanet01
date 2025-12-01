using UnityEngine;

public class RedObject : MonoBehaviour
{
    // 衝突の相手として確認したいタグ名 (tag2)
    private const string TargetCollisionTag = "signal";
    // 最終的に破棄したいオブジェクトのタグ名 (tag3)
    private const string TargetDestroyTag = "Stealth_Wall";
    private const string TargetDestroyTag2 = "Car";

    // コライダーが Is Trigger の場合に呼び出されます。
    private void OnTriggerEnter(Collider other)
    {
        // 1. 衝突の相手が tag2 であるかを確認する
        if (other.gameObject.CompareTag(TargetCollisionTag))
        {
            Debug.Log(gameObject.name + " (tag1) が " + other.gameObject.name + " (tag2) に衝突しました。");

            // 2. tag3のオブジェクトを検索し、破棄する関数を呼び出す
            DestroyAllObjectsWithTag(TargetDestroyTag);
            DestroyAllObjectsWithTag(TargetDestroyTag2);
            // （オプション）tag1のオブジェクト自身も消したい場合は、次の行を追加
            // Destroy(gameObject);
        }
    }

    // シーン内の指定されたタグを持つすべてのオブジェクトを破棄する関数
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
