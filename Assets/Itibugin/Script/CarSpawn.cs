using UnityEngine;
using System.Collections;
public class CarSpawn : MonoBehaviour
{
    // 8種類（またはそれ以上）の車のプレハブをインスペクターから設定
    public GameObject[] carPrefabs; // 配列に変更

    public float spawnInterval = 3f;

    void Start()
    {
        // プレハブ配列が空でないか確認
        if (carPrefabs.Length == 0)
        {
            Debug.LogError("Car Prefabs array is empty! Please assign car prefabs in the Inspector.");
            return;
        }
        StartCoroutine(SpawnCarsRoutine());
    }

    IEnumerator SpawnCarsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 1. 配列のインデックスをランダムに決定
            // 0から (配列の長さ - 1) までの整数をランダムに取得
            int randomIndex = Random.Range(0, carPrefabs.Length);

            // 2. ランダムに選ばれたプレハブを取得
            GameObject carToSpawn = carPrefabs[randomIndex];

            // 3. 車を生成
            Instantiate(carToSpawn, transform.position, transform.rotation);
        }
    }
}
