using UnityEngine;

public class DestroyEff : MonoBehaviour
{
    public float DestroyTime = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Destroy", DestroyTime);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
