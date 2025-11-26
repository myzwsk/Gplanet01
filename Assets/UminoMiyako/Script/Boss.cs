using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class Boss : MonoBehaviour
{
    public GameObject AOE1Field;
    public GameObject AOE8Field;
    public GameObject AOECircle;
    public GameObject AOEThin;
    public GameObject[] Field;
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
            StartCoroutine(AttackThin());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Attack1Field());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Attack8Field());
        }
    }
    //外から内　内から外
    private IEnumerator AttackThin()
    {
        Vector3 startPosU = new Vector3(0f, 50f, -10.5f);
        Vector3 startPosL = new Vector3(-10.5f, 50f, 0f);
        Vector3 startPosR = new Vector3(10.5f, 50f, 0f);
        Vector3 startPosD = new Vector3(0f, 50f, 10.5f);
        for(int i = 0; i < 8; i++)
        {
           
            Attack(startPosU, AOEThin,90);
            Attack(startPosL, AOEThin,0);
            Attack(startPosR, AOEThin,0);
            Attack(startPosD, AOEThin,90);
            startPosU.z += 3;
            startPosL.x += 3;
            startPosR.x -= 3;
            startPosD.z -= 3;
            yield return new WaitForSeconds(1f);
        }
        
    }
    //１ブロック破壊
    private IEnumerator Attack1Field()
    {
        int rand=Random.Range(1, 17);
        Vector3 startPos = Field[rand - 1].transform.position;
        startPos.y = 50f;
        Attack(startPos, AOE1Field, 0);
        yield return new WaitForSeconds(1f);
        BossField FieldScript = Field[rand-1].GetComponent<BossField>();
        if (FieldScript != null)
        {
            FieldScript.ObjectFalse();
        }
        yield return new WaitForSeconds(5f);
        if (FieldScript != null)
        {
            FieldScript.ObjectTrue();
        }
    }
    //半面攻撃
    private IEnumerator Attack8Field()
    {
        int rand = Random.Range(1, 5);
        Vector3 startPos=new Vector3(0,50,0);
        int rota = 0;
        int[] field = { 0, 0, 0, 0, 0, 0, 0, 0 };
        switch (rand)
        {
            case 1:
                startPos.z = 6f; rota = 90; field = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }; break;
            case 2:
                startPos.x = -6f; field = new int[] { 1, 2, 5, 6, 9, 10, 13, 14 }; break;
            case 3:
                startPos.x = 6f; field = new int[] { 3, 4, 7, 8, 11, 12, 15, 16 }; break;
            case 4:
                startPos.z = -6f; rota = 90; field = new int[] { 9, 10, 11, 12, 13, 14, 15, 16 }; break;
        }
        Attack(startPos, AOE8Field, rota);
        yield return new WaitForSeconds(1f);
        BossField[] FieldScript= { null,null,null,null,null,null,null,null};
        for (int i=0;i<8;i++)
        {
            FieldScript[i] = Field[(field[i]-1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        yield return new WaitForSeconds(5f);
        if (FieldScript != null)
        {
            for(int i = 0; i < 8; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
    }
    //AOE表示処理
    private void Attack(Vector3 startPoint, GameObject prefab, float yRotationOffset)
    {
        Ray ray = new Ray(startPoint, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
        {
            // 地面の傾きに合わせた回転
            Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Y軸方向の追加回転を合成
            Quaternion extraRotation = Quaternion.AngleAxis(yRotationOffset, Vector3.up);

            // 合成した回転を使用
            Quaternion finalRotation = baseRotation * extraRotation;

            Instantiate(prefab, hit.point, finalRotation);
        }
    }
}
