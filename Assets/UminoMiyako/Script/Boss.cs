using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Boss : MonoBehaviour
{
    public GameObject AOE1Field;
    public GameObject AOE8Field;
    public GameObject AOECircle;
    public GameObject AOEBigCircle;
    public GameObject AOEThin;
    public GameObject AOEThinHalf;
    public GameObject AOEPush;
    public GameObject Nail;
    public GameObject[] Star;
    public GameObject[] Field;
    public GameObject[] EffectField;
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            AttackCircle();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(AttackVirtical());
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(AttackHrizon());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(AttackOut());
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(AttackIn());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(AttackStar());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(AttackStar2());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(AttackSword());
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(AttackSword2());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(AttackSword3());
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(AttackPush());
        }
    }
    //オブジェクトを落としてプレイヤーの移動を阻害--------------------------------------------------------------------------------------------------
    //プレイヤーを中心から吹き飛ばし---------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackPush()
    {
        float distance = 10f;
        float duration = 0.2f;
        Vector3 center = Vector3.zero;
        Vector3 goPos = new Vector3(center.x, 50, center.z);
        CharacterController[] players = FindObjectsOfType<CharacterController>();
        Attack(goPos, AOEPush, 0);
        yield return new WaitForSeconds(3f);
        foreach (var controller in players)
        {
            Vector3 startPos = controller.transform.position;

            // 中心からの方向ベクトル（XZのみ）
            Vector3 dir = (startPos - center);
            dir.y = 0f; // Y方向は無視
            if (dir.sqrMagnitude < 0.01f)
            {
                // ほぼ中心にいる場合は強制的にX方向へ押し出すなど
                dir = Vector3.right;
            }
            dir = dir.normalized;

            // 吹き飛ばし先の位置
            Vector3 targetPos = startPos + dir * distance;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                Vector3 newPos = Vector3.Lerp(startPos, targetPos, t);

                // 差分を渡す
                controller.Move(newPos - controller.transform.position);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
    //外側のフィールドの予兆からの攻撃3--------------------------------------------------------------------------------------------------
    private IEnumerator AttackSword3()
    {
        //外周削除
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        int[] In = new int[2];
        int rand= Random.Range(0, 2);
        if (rand == 0) In =new int[]{ 6, 11};
        else In = new int[] { 7, 10 };
        for (int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        for (int i = 0; i < 2; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        BossField[] OutFieldScript = new BossField[12];
        BossField[] InFieldScript = new BossField[2];
        BossField[] EffectFieldScript = new BossField[16];
        for (int i = 0; i < 12; i++)
        {
            OutFieldScript[i] = Field[(Out[i] - 1)].GetComponent<BossField>();
            if (OutFieldScript[i] != null)
            {
                OutFieldScript[i].ObjectFalse();
            }
        }
        for (int i = 0; i < 2; i++)
        {
            InFieldScript[i] = Field[(In[i] - 1)].GetComponent<BossField>();
            if (InFieldScript[i] != null)
            {
                InFieldScript[i].ObjectFalse();
            }
        }
        for (int i = 0; i < 16; i++)
        {
            EffectFieldScript[i] = EffectField[i].GetComponent<BossField>();
            if (EffectFieldScript[i] != null)
            {
                EffectFieldScript[i].ObjectTrue();
            }
        }
        yield return new WaitForSeconds(2f);
        rand = Random.Range(0, 8);
        int[] rota = new int[2];
        Vector3 goPos = Vector3.zero;
        GameObject[] sword = new GameObject[4];
        Vector3[] attackPos = new Vector3[4];
        Vector3[] attackPos2 = new Vector3[4];
        for (int j = 0; j < 2; j++)
        {
            switch (rand % 4)
            {
                case 0:
                    goPos = new Vector3(-4.5f, 3f, 30f);
                    if (rand >= 4) goPos.x += 3f;
                    startPos = new Vector3(goPos.x, 50f, goPos.z - 6);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(startPos.x, startPos.y, 0);
                        goPos.x += 6f;
                        startPos.x += 6f;
                    }
                    rota[j] = 0;
                    break;
                case 1:
                    goPos = new Vector3(30f, 3f, 4.5f);
                    if (rand >= 4) goPos.z -= 3f;
                    startPos = new Vector3(goPos.x - 6, 50f, goPos.z);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(0, startPos.y, startPos.z);
                        goPos.z -= 6f;
                        startPos.z -= 6f;
                    }
                    rota[j] = 90;
                    break;
                case 2:
                    goPos = new Vector3(-30f, 3f, 4.5f);
                    if (rand >= 4) goPos.z -= 3f;
                    startPos = new Vector3(goPos.x + 6, 50f, goPos.z);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(0, startPos.y, startPos.z);
                        goPos.z -= 6f;
                        startPos.z -= 6f;
                    }
                    rota[j] = 90;
                    break;
                case 3:
                    goPos = new Vector3(-4.5f, 3f, -30f);
                    if (rand >= 4) goPos.x += 3f;
                    startPos = new Vector3(goPos.x, 50f, goPos.z + 6);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(startPos.x, startPos.y, 0);
                        goPos.x += 6f;
                        startPos.x += 6f;
                    }
                    rota[j] = 0;
                    break;
            }
            if (rand % 4 == 0 || rand % 4 == 3)
            {
                int[] next = { 1, 2, 5, 6 };
                rand = next[Random.Range(0, 4)];
            }
            else
            {
                int[] next = { 0, 3, 4, 7 };
                rand = next[Random.Range(0, 4)];
            }
        }

        yield return new WaitForSeconds(5f);
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j]);
                Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j]);
            }
        }

        //外周エリア再出現
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            Destroy(sword[i]);
        }
        if (OutFieldScript != null)
        {
            for (int i = 0; i < 12; i++)
            {
                OutFieldScript[i].ObjectTrue();
            }
        }
        if (InFieldScript != null)
        {
            for (int i = 0; i < 2; i++)
            {
                InFieldScript[i].ObjectTrue();
            }
        }
        if (EffectFieldScript != null)
        {
            for (int i = 0; i < 16; i++)
            {
                EffectFieldScript[i].ObjectFalse();
            }
        }
    }
    //外側のフィールドの予兆からの攻撃2--------------------------------------------------------------------------------------------------
    private IEnumerator AttackSword2()
    {
        //外周削除
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        for (int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        BossField[] FieldScript = new BossField[12];
        BossField[] EffectFieldScript = new BossField[16];
        for (int i = 0; i < 12; i++)
        {
            FieldScript[i] = Field[(Out[i] - 1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        for (int i = 0; i < 16; i++)
        {
            EffectFieldScript[i] = EffectField[i].GetComponent<BossField>();
            if (EffectFieldScript[i] != null)
            {
                EffectFieldScript[i].ObjectTrue();
            }
        }
        yield return new WaitForSeconds(2f);
        int rand = Random.Range(0, 8);
        int[] rota = new int[2];
        Vector3 goPos = Vector3.zero;
        GameObject[] sword = new GameObject[4];
        Vector3[] attackPos = new Vector3[4];
        Vector3[] attackPos2 = new Vector3[4];
        for (int j = 0; j < 2; j++)
        {
            switch (rand % 4)
            {
                case 0:
                    goPos = new Vector3(-4.5f, 3f, 30f);
                    if (rand >= 4) goPos.x += 3f;
                    startPos = new Vector3(goPos.x, 50f, goPos.z - 6);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(startPos.x, startPos.y, 0);
                        goPos.x += 6f;
                        startPos.x += 6f;
                    }
                    rota[j] = 0;
                    break;
                case 1:
                    goPos = new Vector3(30f, 3f, 4.5f);
                    if (rand >= 4) goPos.z -= 3f;
                    startPos = new Vector3(goPos.x - 6, 50f, goPos.z);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(0, startPos.y, startPos.z);
                        goPos.z -= 6f;
                        startPos.z -= 6f;
                    }
                    rota[j] = 90;
                    break;
                case 2:
                    goPos = new Vector3(-30f, 3f, 4.5f);
                    if (rand >= 4) goPos.z -= 3f;
                    startPos = new Vector3(goPos.x + 6, 50f, goPos.z);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(0, startPos.y, startPos.z);
                        goPos.z -= 6f;
                        startPos.z -= 6f;
                    }
                    rota[j] = 90;
                    break;
                case 3:
                    goPos = new Vector3(-4.5f, 3f, -30f);
                    if (rand >= 4) goPos.x += 3f;
                    startPos = new Vector3(goPos.x, 50f, goPos.z + 6);
                    for (int i = 0; i < 2; i++)
                    {
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.identity);
                        attackPos[i + (j * 2)] = startPos;
                        attackPos2[i + (j * 2)] = new Vector3(startPos.x, startPos.y, 0);
                        goPos.x += 6f;
                        startPos.x += 6f;
                    }
                    rota[j] = 0;
                    break;
            }
            if (rand % 4 == 0 || rand % 4 == 3)
            {
                int[] next = { 1, 2, 5, 6 };
                rand=next[Random.Range(0, 4)];
            }
            else
            {
                int[] next = { 0, 3, 4, 7 };
                rand = next[Random.Range(0, 4)];
            }
        }
        
        yield return new WaitForSeconds(5f);
        for(int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j]);
                Attack(attackPos2[i+(j*2)], AOEThinHalf, rota[j]);
            }
        }
        
        //外周エリア再出現
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            Destroy(sword[i]);
        }
        if (FieldScript != null)
        {
            for (int i = 0; i < 12; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
        if (EffectFieldScript != null)
        {
            for (int i = 0; i < 16; i++)
            {
                EffectFieldScript[i].ObjectFalse();
            }
        }
    }
    //外側のフィールドの予兆からの攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackSword()
    {
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        for (int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        BossField[] FieldScript = new BossField[12];
        BossField[] EffectFieldScript = new BossField[16];
        for (int i = 0; i < 12; i++)
        {
            FieldScript[i] = Field[(Out[i] - 1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        for (int i = 0; i < 16; i++)
        {
            EffectFieldScript[i] = EffectField[i].GetComponent<BossField>();
            if (EffectFieldScript[i] != null)
            {
                EffectFieldScript[i].ObjectTrue();
            }
        }
        yield return new WaitForSeconds(2f);
        int rand = Random.Range(0, 8);
        int rota = default;
        Vector3 goPos = Vector3.zero;
        GameObject[] sword = new GameObject[2];
        Vector3[] attackPos=new Vector3[2];
        switch (rand % 4)
        {
            case 0:
                goPos = new Vector3(-4.5f, 3f, 30f);
                if (rand >= 4) goPos.x += 3f;
                startPos = new Vector3(goPos.x, 50f, goPos.z - 6);
                for (int i = 0; i < 2; i++)
                {
                    sword[i] = Instantiate(Nail, goPos, Quaternion.identity);
                    Attack(startPos, AOEThinHalf, 0);
                    attackPos[i] = new Vector3(startPos.x, startPos.y, 0);
                    goPos.x += 6f;
                    startPos.x += 6f;
                }
                rota = 0;
                break;
            case 1:
                goPos = new Vector3(30f, 3f, 4.5f);
                if (rand >= 4) goPos.z -= 3f;
                startPos = new Vector3(goPos.x - 6, 50f, goPos.z);
                for (int i = 0; i < 2; i++)
                {
                    sword[i] = Instantiate(Nail, goPos, Quaternion.identity);
                    Attack(startPos, AOEThinHalf, 90);
                    attackPos[i]= new Vector3(0,startPos.y,startPos.z);
                    goPos.z -= 6f;
                    startPos.z -= 6f;
                }
                rota = 90;
                break;
            case 2:
                goPos = new Vector3(-30f, 3f, 4.5f);
                if (rand >= 4) goPos.z -= 3f;
                startPos = new Vector3(goPos.x + 6, 50f, goPos.z);
                for (int i = 0; i < 2; i++)
                {
                    sword[i] = Instantiate(Nail, goPos, Quaternion.identity);
                    Attack(startPos, AOEThinHalf, 90);
                    attackPos[i] = new Vector3(0, startPos.y, startPos.z);
                    goPos.z -= 6f;
                    startPos.z -= 6f;
                }
                rota = 90;
                break;
            case 3:
                goPos = new Vector3(-4.5f, 3f, -30f);
                if (rand >= 4) goPos.z += 3f;
                startPos = new Vector3(goPos.x, 50f, goPos.z + 6);
                for (int i = 0; i < 2; i++)
                {
                    sword[i] = Instantiate(Nail, goPos, Quaternion.identity);
                    Attack(startPos, AOEThinHalf, 0);
                    attackPos[i] = new Vector3(startPos.x, startPos.y, 0);
                    goPos.x += 6f;
                    startPos.x += 6f;
                }
                rota = 0;
                break;
        }
        yield return new WaitForSeconds(3f);
        for(int i = 0; i < 2; i++)
        {
            Attack(attackPos[i], AOEThinHalf, rota);
        }
        //外周エリア再出現
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 2; i++)
        {
            Destroy(sword[i]);
        }
        if (FieldScript != null)
        {
            for (int i = 0; i < 12; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
        if (EffectFieldScript != null)
        {
            for (int i = 0; i < 16; i++)
            {
                EffectFieldScript[i].ObjectFalse();
            }
        }
    }
    //星が重なった場所から攻撃2--------------------------------------------------------------------------------------------------
    private IEnumerator AttackStar2()
    {
        int rand = Random.Range(0, 361);
        float startAngleDegrees = rand;
        GameObject[] StarMana = { null, null };
        Star[] StarSc = { null, null };
        Vector3 goPos = new Vector3(0, 0, 0);
        Vector3 startPos = default;
        int[] In = { 6, 7, 10, 11 };
        for (int i = 0; i < 4; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        BossField[] FieldScript = { null, null, null, null, null, null, null, null, null, null, null, null };
        for (int i = 0; i < 4; i++)
        {
            FieldScript[i] = Field[(In[i] - 1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 2; i++)
        {
            float angle = startAngleDegrees * Mathf.Deg2Rad;
            Vector3 center = new Vector3(0, 5, 0);
            float x = center.x + Mathf.Cos(angle) * 5;
            float z = center.z + Mathf.Sin(angle) * 5;
            goPos = new Vector3(x, center.y, z);
            StarMana[i] = Instantiate(Star[i], goPos, Quaternion.identity);
            StarSc[i] = StarMana[i].GetComponent<Star>();
            StarSc[i].angle = angle;
            if (startAngleDegrees > 180) startAngleDegrees -= 180;
            else startAngleDegrees += 180;
        }
        yield return new WaitUntil(() =>
            StarMana[0] != null && StarMana[1] != null &&
                Vector3.Distance(StarMana[0].transform.position, StarMana[1].transform.position) < 0.1f);
        goPos = StarMana[0].transform.position;
        Destroy(StarMana[0]);
        Destroy(StarMana[1]);
        Attack(goPos, AOEBigCircle, 0);
        yield return new WaitForSeconds(2f);
        if (FieldScript != null)
        {
            for (int i = 0; i < 4; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
    }
    //星が重なった場所から攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackStar()
    {
        int rand = Random.Range(0, 361);
        float startAngleDegrees = rand;
        GameObject[] StarMana = { null, null };
        Star[] StarSc = {null,null};
        Vector3 goPos = new Vector3(0, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            float angle = startAngleDegrees * Mathf.Deg2Rad;
            Vector3 center = new Vector3(0, 5, 0);
            float x = center.x + Mathf.Cos(angle) * 5;
            float z = center.z + Mathf.Sin(angle) * 5;
            goPos = new Vector3(x, center.y, z);
            StarMana[i]= Instantiate(Star[i], goPos, Quaternion.identity);
            StarSc[i]=StarMana[i].GetComponent<Star>();
            StarSc[i].angle = angle;
            if (startAngleDegrees > 180) startAngleDegrees -= 180;
            else startAngleDegrees += 180;
        }
        yield return new WaitUntil(() =>
            StarMana[0] != null && StarMana[1] != null &&
                Vector3.Distance(StarMana[0].transform.position, StarMana[1].transform.position) < 0.1f);
        goPos = StarMana[0].transform.position;
        Destroy(StarMana[0]);
        Destroy(StarMana[1]);
        Attack(goPos, AOEBigCircle, 0);
    }
    //横の1列以外に攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackHrizon()
    {
        int rand = Random.Range(0, 8);
        Vector3 startPos = new Vector3(15f, 3f, -10.5f);
        Vector3 goPos=new Vector3(0,0,0);
        GameObject[] nail= { null,null,null,null,null,null,null,null};
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                goPos = new Vector3(startPos.x, startPos.y, startPos.z + (3 * i));
                nail[i]=Instantiate(Nail, goPos, Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(5f);
        startPos = new Vector3(0, 50f, -10.5f);
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                Attack(startPos, AOEThin, 90);
            }
            startPos.z += 3;
        }
        for(int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                Destroy(nail[i]);
            }
        }
    }
    //縦の1列以外に攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackVirtical()
    {
        int rand = Random.Range(0, 8);
        Vector3 startPos = new Vector3(-10.5f, 3f, 15f);
        Vector3 goPos = new Vector3(0, 0, 0);
        GameObject[] nail = { null, null, null, null, null, null, null, null };
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                goPos = new Vector3(startPos.x + (3 * i), startPos.y, startPos.z);
                nail[i] = Instantiate(Nail, goPos, Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(5f);
        startPos = new Vector3(-10.5f, 50f, 0);
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                Attack(startPos, AOEThin, 0);
            }
            startPos.x += 3;
        }
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                Destroy(nail[i]);
            }
        }
    }
    //外周破壊--------------------------------------------------------------------------------------------------
    private IEnumerator AttackOut()
    {
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        for(int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i]-1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        BossField[] FieldScript = { null, null, null, null, null, null, null, null , null , null , null , null };
        for (int i = 0; i < 12; i++)
        {
            FieldScript[i] = Field[(Out[i] - 1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        yield return new WaitForSeconds(5f);
        if (FieldScript != null)
        {
            for (int i = 0; i < 12; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
    }
    //内側破壊--------------------------------------------------------------------------------------------------
    private IEnumerator AttackIn()
    {
        Vector3 startPos = default;
        int[] In = { 6,7,10,11 };
        for (int i = 0; i < 4; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        BossField[] FieldScript = { null, null, null, null, null, null, null, null, null, null, null, null };
        for (int i = 0; i < 4; i++)
        {
            FieldScript[i] = Field[(In[i] - 1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        yield return new WaitForSeconds(5f);
        if (FieldScript != null)
        {
            for (int i = 0; i < 4; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
    }
    //プレイヤー地点に攻撃--------------------------------------------------------------------------------------------------
    private void AttackCircle()
    {
        Vector3 startPos = new Vector3(0,0,0);
        GameObject obj = GameObject.FindWithTag("Player");
        if (obj == null) return;
        else
        {
            startPos = obj.transform.position;
        }
        startPos.y = 50;
        Attack(startPos, AOECircle, 0);
    }
    //外から内　内から外--------------------------------------------------------------------------------------------------
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
    //１ブロック破壊--------------------------------------------------------------------------------------------------
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
    //半面攻撃--------------------------------------------------------------------------------------------------------------------
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
    //AOE表示処理---------------------------------------------------------------------------------------------------------
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
