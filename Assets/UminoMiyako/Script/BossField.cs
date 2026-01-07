using UnityEngine;

public class BossField : MonoBehaviour
{
    public void ObjectStealth()
    {
        GetComponent<Renderer>().enabled = false;
    }
    public void ObjectReStealth()
    {
        GetComponent<Renderer>().enabled = true;
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
