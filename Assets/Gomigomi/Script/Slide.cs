using UnityEngine;

public class Slide : MonoBehaviour
{
    private Vector3 initialPosition;
    public float sX=0;
    public float sY=0;
    public float sZ=0;
    public float Speed = 0;//回転スピード

    public bool Circular = false;//円移動ならｔ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
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
}
