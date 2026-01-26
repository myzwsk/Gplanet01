using UnityEngine;

public class Light : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset = new Vector3 (0, 0, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Player != null)
        {
            transform.position = Player.position + offset;
        }
    }
}
