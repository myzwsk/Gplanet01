using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int bullet = 0;
    public float cooldown = 1;
    public BossHp bosshp;
    private float count=0;
    private bool go = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go)
        {
            if (bullet > 0)
            {
                bosshp.Damage();
                bullet--;
                go = false;
                count = cooldown;
            }
        }
        else
        {
            if (count > 0)
            {
                count -= Time.deltaTime;
            }
            else
            {
                go = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object01"))
        {
            bullet += 1;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Object02"))
        {
            bullet += 2;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Object03"))
        {
            bullet += 3;
            Destroy(other.gameObject);
        }
    }
}
