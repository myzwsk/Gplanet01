using UnityEngine;

public class AOEDonut : MonoBehaviour
{
    public bool isHitCol1 = false;
    public bool isHitCol2 = false;

    void Start()
    {
        Invoke("Destroy", 4f);
    }
    void Destroy()
    {
        if (isHitCol1&&!isHitCol2)
        {
            Debug.Log("プレイヤー死亡！");
        }
        else
        {
            Debug.Log("生存");
        }

        Destroy(gameObject);
    }
}
