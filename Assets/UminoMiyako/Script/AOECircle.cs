using UnityEngine;

public class AOECircle : MonoBehaviour
{
    private string touchedTag = null; // 最後に触れたタグを記録
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Destroy", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        touchedTag = other.gameObject.tag;
    }
    void Destroy()
    {

        if (touchedTag != null)
        {
            if (touchedTag == "Player")
            {
                Debug.Log("破壊時に触れていたタグ: " + touchedTag);
            }

        }
        Destroy(gameObject);
    }
}
