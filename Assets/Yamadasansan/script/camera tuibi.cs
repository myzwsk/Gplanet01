using UnityEngine;

public class cameratuibi : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        transform.position = player.position + offset;
        if(pos.y < -45)
        {
            pos.y = -45;
        }
    }
}
