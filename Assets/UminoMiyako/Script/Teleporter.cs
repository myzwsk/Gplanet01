using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject EffPre;
    public Vector3 telePosition;
    public Vector3 endScale;
    public float time = 1f;
    public bool istele = false;

    private Transform player;
    private GameObject Eff;
    private Vector3 startScale;
    private float count = 0;
    private bool OnPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = new Vector3(0.001f, 0.001f, 0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        if (OnPlayer)
        {
            if (count < time)
            {
                istele = false;
                count += Time.deltaTime;
                float t = count / time;
                Eff.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            }
            else
            {
                istele = true;
                Destroy(Eff);
                Teleport();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            count = 0;
            OnPlayer = true;
            Eff= Instantiate(EffPre, transform.position, transform.localRotation);
            player = other.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            count = 0;
            OnPlayer = false;
            Destroy(Eff);
        }
    }
    private void Teleport()
    {
        istele = true;
        OnPlayer = false;
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        player.position = telePosition;

        if (cc != null) cc.enabled = true;
    }
}
