using System.Threading;
using UnityEngine;

public class AOE : MonoBehaviour
{
    public float time = 1f;
    public GameObject AOEeffPre;
    public GameObject Eff;

    private Vector3 startScale;
    private Vector3 endScale;
    private GameObject AOEeff;
    private bool isTouchingPlayer = false;
    private float count = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale= new Vector3(0.001f,0.001f,0.001f);
        endScale= transform.localScale;
        AOEeff = Instantiate(AOEeffPre, transform.position, transform.localRotation);
        AOEeff.transform.localScale=startScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (count < time)
        {
            count += Time.deltaTime;
            float t = count / time;
            AOEeff.transform.localScale = Vector3.Lerp(startScale, endScale, t);
        }
        else
        {
            Destroy();
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
    void Destroy()
    {

        if (isTouchingPlayer)
        {
            Debug.Log("プレイヤー死亡！");
        }
        if (Eff != null)
        {
            Instantiate(Eff, transform.position, Quaternion.identity);
        }
        Destroy(AOEeff);
        Destroy(gameObject);
    }
}
