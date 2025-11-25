using UnityEngine;

public class BossField : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {
            ObjectFalse();
        }
        else if (false)
        {
            ObjectTrue();
        }
    }
    void ObjectTrue()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }
    void ObjectFalse()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }
}
