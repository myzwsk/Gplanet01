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
    }
    public void ObjectTrue()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }
    public void ObjectFalse()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }
}
