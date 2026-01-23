using UnityEngine;

public class BossField : MonoBehaviour
{
    public void ObjectStealth()
    {
        Renderer r = GetComponent<Renderer>();

        if (r.enabled == true)   // すでに表示中なら非表示にする
        {
            r.enabled = false;
        }
    }

    public void ObjectReStealth()
    {
        Renderer r = GetComponent<Renderer>();

        if (r.enabled == false)  // すでに非表示なら表示にする
        {
            r.enabled = true;
        }
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
