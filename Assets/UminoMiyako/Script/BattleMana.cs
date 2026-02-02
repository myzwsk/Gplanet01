using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BattleMana : MonoBehaviour
{
    public int maxHp = 300;
    public int Hp = 0;
    public int maxBullet = 5;
    public int fallDamage = 10;
    public float bulletCool = 5;
    public Slider slider;
    public TextMeshProUGUI text;
    public TextMeshProUGUI Maxtext;
    public Boss bossSc;
    public BossNormal NbossSc;
    public BossHp bosshpSc;
    public Teleporter teleSc;
    public PlayerHealth deathSc;
    public GameObject rest;
    public GameObject player;
    public GameObject diePre;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public Vector3 areaCenter;
    public Vector3 areaSize;

    private int rand = 0;
    private int bulletcount = 0;
    private float count = 0;
    private bool battle = false;
    private bool diesp=false;
    private GameObject die;
    private GameObject[] bullet;
    private escape Esc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHp = ScoreManager.Score;
        Maxtext.text = maxHp.ToString();
        Hp = maxHp;
        slider.minValue = 0;
        slider.maxValue = maxHp;
        slider.value = Hp;
        bullet = new GameObject[] { bullet1,bullet1, bullet1, bullet1, bullet1, bullet2, bullet2, bullet2, bullet3, bullet3, };
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            Hp = 0;
            if (!diesp)
            {
                die = Instantiate(diePre, player.transform.position, Quaternion.identity);
                diesp = true;
            }
        }
        slider.value = Hp;
        text.text = Hp.ToString()+"/";
        if (deathSc.isDie)
        {
            deathSc.isDie = false;
            if (!rest.activeSelf)
            {
                Hp -= fallDamage;
                rest.SetActive(true);
            }
            if (Hp <= 0)
            {
                if (diesp)
                {
                    bosshpSc.Hp = bosshpSc.maxHp;
                    Destroy(die);
                    diesp = false;
                    Hp = maxHp;
                }
            }
            if (bossSc.enabled)
            {
                battle = false;
                bossSc.go = false;
                bossSc.BossAttackAllReset();
            }

            if (NbossSc.enabled)
            {
                battle = false;
                NbossSc.go = false;
                NbossSc.BossAttackAllReset();
            }
        }
        if (teleSc.istele)
        {
            teleSc.istele = false;
            if (rest.activeSelf)
            {
                GetComponent<AudioSource>().Play();
                rest.SetActive(false);
            }
            if (bossSc.enabled)
            {
                battle = true;
                bossSc.go = true;
            }
            if (NbossSc.enabled)
            {
                battle = true;
                NbossSc.go = true;
            }
        }
        if (count > 0)
        {
            count -= Time.deltaTime;
        }
        else
        {
            if (battle)
            {
                bulletcount = FindObjectsOfType<DontSleep>().Length;
                if (bulletcount < maxBullet)
                {
                    SummonAboveTarget();
                    count = bulletCool;
                }
                bulletcount = 0; 
            }
            
        }
    }
    public void PDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("ボスにダメージ");
    }
    public void SummonAboveTarget()
    {
        const int maxTry = 20;

        for (int i = 0; i < maxTry; i++)
        {
            Vector3 randomPoint = GetRandomPointInArea(areaCenter, areaSize);

            if (TryRaycastToTarget(randomPoint, "Field", out RaycastHit hit))
            {
                Vector3 summonPos = hit.point + Vector3.up * 1.5f;
                rand=Random.Range(0, 10);
                Instantiate(bullet[rand], summonPos, Quaternion.identity);
                return;
            }
        }

        Debug.LogWarning("ターゲットに当たる位置が見つかりませんでした");
    }
    bool TryRaycastToTarget(Vector3 point, string targetTag, out RaycastHit hit)
    {
        Vector3 rayStart = point + Vector3.up * 50f; // 上空から
        Vector3 direction = Vector3.down;

        if (Physics.Raycast(rayStart, direction, out hit, 100f))
        {
            return hit.transform.CompareTag(targetTag);
        }

        return false;
    }
    Vector3 GetRandomPointInArea(Vector3 center, Vector3 size)
    {
        return new Vector3(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            center.y,
            Random.Range(center.z - size.z / 2, center.z + size.z / 2)
        );
    }

}
