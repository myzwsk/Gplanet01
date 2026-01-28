using System.Threading;
using UnityEngine;

public class AOE : MonoBehaviour
{
    public int damage = 1;
    public int EffLota = 0;
    public float time = 1f;
    public float Efftime = 0;
    public float SEdelay = 0;
    public GameObject AOEeffPre;
    public GameObject Eff;
    public AudioSource SE;

    private Vector3 startScale;
    private Vector3 endScale;
    private GameObject AOEeff;
    private bool isTouchingPlayer = false;
    private bool EffFlag = false;
    private float count = 0;
    private BattleMana hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale= new Vector3(0.001f,0.001f,0.001f);
        endScale= transform.localScale;
        AOEeff = Instantiate(AOEeffPre, transform.position, transform.localRotation);
        AOEeff.transform.localScale=startScale;
        hp = FindAnyObjectByType<BattleMana>();
    }

    // Update is called once per frame
    void Update()
    {
        if (count < time)
        {
            count += Time.deltaTime;
            float t = count / time;
            AOEeff.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            if (count >= time - Efftime && EffFlag == false)
            {
                if (Eff != null)
                {
                    EffFlag = true;
                    Instantiate(Eff, transform.position, Quaternion.Euler(0,90*EffLota,0));
                    if (SE != null)
                    {
                        SE.PlayDelayed(SEdelay);
                    }
                }
            }
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
            if (hp != null)
            {
                hp.PDamage(damage);
            }
        }
        if (Eff != null&&EffFlag==false)
        {
            Instantiate(Eff, transform.position, Quaternion.Euler(0, 90 * EffLota, 0));
            if (SE != null)
            {
                SE.PlayDelayed(SEdelay);
            }
        }
        Destroy(AOEeff);
        Destroy(gameObject);
    }
}
