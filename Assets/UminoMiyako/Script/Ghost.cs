using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Vector3 initialPosition;
    public int damage = 1;
    public float sX=0;
    public float sY=0;
    public float sZ=0;
    public float Speed = 0;//回転スピード

    public bool Circular = false;//円移動ならｔ

    private BattleMana hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
        hp = FindAnyObjectByType<BattleMana>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time * Speed;

        if (Circular)
        {
            float x = Mathf.Cos(time) * sX;
            float z = Mathf.Sin(time) * sZ;
            transform.position = initialPosition + new Vector3(x, sY, z);
        }
        else
        {
            transform.position = new Vector3(Mathf.Sin(Time.time) * sX + initialPosition.x, Mathf.Sin(Time.time) * sY + initialPosition.y, Mathf.Sin(Time.time) * sZ + initialPosition.z);

        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤー死亡");

            if (hp != null)
            {
                hp.PDamage(damage);
            }
        }
    }
}
