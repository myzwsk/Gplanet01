using UnityEngine;

public class AOEDonut : MonoBehaviour
{
    public int damage = 1;
    public bool isHitCol1 = false;
    public bool isHitCol2 = false;
    public float time = 1f;
    public GameObject AOEeffPre;
    private Vector3 startScale;
    private Vector3 endScale;
    private GameObject AOEeff;
    private bool isTouchingPlayer = false;
    private float count = 0;
    private BattleMana hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = new Vector3(0.001f, 0.001f, 0.001f);
        endScale = transform.localScale;
        AOEeff = Instantiate(AOEeffPre, transform.position, transform.localRotation);
        AOEeff.transform.localScale = startScale;
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
        }
        else
        {
            Destroy();
        }

    }
    void Destroy()
    {

        if (isHitCol1 && !isHitCol2)
        {
            if (hp != null)
            {
                hp.PDamage(damage);
            }
            Debug.Log("プレイヤー死亡！");
        }
        else
        {
            Debug.Log("生存");
        }
        Destroy(AOEeff);
        Destroy(gameObject);
    }
}
