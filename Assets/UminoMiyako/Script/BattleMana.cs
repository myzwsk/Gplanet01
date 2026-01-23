using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BattleMana : MonoBehaviour
{
    public int maxHp = 300;
    public int Hp = 0;
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

    private bool diesp=false;
    private GameObject die;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Maxtext.text = maxHp.ToString();
        Hp = maxHp;
        slider.minValue = 0;
        slider.maxValue = maxHp;
        slider.value = Hp;
        //ここにスコアをまっくすHPにする処理を入れる
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
                rest.SetActive(true);
            }
            if (Hp == 0)
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
                bossSc.go = false;
                bossSc.BossAttackAllReset();
            }

            if (NbossSc.enabled)
            {
                NbossSc.go = false;
                NbossSc.BossAttackAllReset();
            }
        }
        if (teleSc.istele)
        {
            teleSc.istele = false;
            if (rest.activeSelf)
            {
                rest.SetActive(false);
            }
            if (bossSc.enabled)
            {
                bossSc.go = true;
            }
            if (NbossSc.enabled)
            {
                NbossSc.go = true;
            }
        }

    }
    public void PDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("ボスにダメージ");
    }
}
