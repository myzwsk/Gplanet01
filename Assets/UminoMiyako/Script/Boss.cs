using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject AOE1Field;
    public GameObject AOE8Field;
    public GameObject AOECircle;
    public GameObject AOEThin;
    public LayerMask targetLayerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AttackThin();
        }
    }
    public void AttackThin()
    {
        Vector3 startPosU = new Vector3(0f, 50f, -7.5f);
        Vector3 startPosL = new Vector3(-7.5f, 50f, 0f);
        Vector3 startPosR = new Vector3(7.5f, 50f, 0f);
        Vector3 startPosD = new Vector3(0f, 50f, 7.5f);
        for(int i = 0; i < 8; i++)
        {
            startPosU.y += i * 3;
            startPosL.x += i * 3;
            startPosR.x -= i * 3;
            startPosD.y -= i * 3;
            Attack(startPosU, AOEThin);
            Attack(startPosL, AOEThin);
            Attack(startPosR, AOEThin);
            Attack(startPosD, AOEThin);
        }
        
    }
    public void Attack(Vector3 startPoint,GameObject prefab)
    {
        Ray ray = new Ray(startPoint, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
        {
            // 地面の傾きに合わせて召喚したい場合は Quaternion.FromToRotation を使用
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Instantiate(prefab, hit.point, rotation);
        }
    }
}
