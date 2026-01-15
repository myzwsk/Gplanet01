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
    public GameObject AOEDonut;
    public GameObject BLOCKBar;
    public GameObject BLOCKBarLong;
    public GameObject Nail;
    public GameObject Shooter;
    public GameObject Ghost;
    public GameObject[] Star;
    public GameObject[] Field;
    public GameObject[] EffectField;
    public GameObject[] Canon;
    public LayerMask targetLayerMask;
    private field[] fi = new field[16];
    private field[] Effi = new field[16];
    struct field
    {
        public bool fiOn;
        public GameObject fiPre;
        public BossField fiSc;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("左シフト：\n1.外内,2.エリア破壊,3.半面破壊,4.円,5.縦爪,6.横爪");
        Debug.Log("右シフト：\n1.外側破壊,2.内側破壊,3.星,4.星内破壊,5.剣,6.剣交差,7.剣交差内破壊");
        Debug.Log("左オルト：\n1.押し出し,2.引き寄せ,3.ドーナツ,4.バー,5.回転バー,6.ステルス,7.全消し");
        Debug.Log("Pキー :\n1.四方に弾召喚,2.内側に弾召喚");
        for (int i = 0; i < 16; i++)
        {
            fi[i].fiOn = true;
            Effi[i].fiOn = false;
            fi[i].fiPre = Field[i];
            Effi[i].fiPre = EffectField[i];
            fi[i].fiSc = Field[i].GetComponent<BossField>();
            Effi[i].fiSc = EffectField[i].GetComponent<BossField>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackThin());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(Attack1Field());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(Attack8Field());
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AttackCircle();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(AttackVirtical());
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(AttackHrizon());
            }
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackOut());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(AttackIn());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(AttackStar());
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(AttackStar2());
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(AttackSword());
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(AttackSword2());
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                StartCoroutine(AttackSword3());
            }
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackPush());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(AttackPull());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttackDonut();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(AttackBar());
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(AttackStick());
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(AttackStealth());
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                StartCoroutine(AttackAllBreak());
            }
        }
        if (Input.GetKey(KeyCode.P))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackShot4());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(AttackShotIn());
            }
        }
    }
    //内側消去から中心から弾を召喚------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackShotIn()
    {
        Vector3 startPos = default;
        int[] In = { 6, 7, 10, 11 };
        for (int i = 0; i < 4; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        DestroyField(In);
        

        GameObject shooter = default;
        startPos = new Vector3(0f, 2f, 0f);
        shooter = Instantiate(Shooter, startPos, Quaternion.identity);
        yield return new WaitForSeconds(5f);
        Destroy(shooter);
        ReField();
    }
    //弾をいっぱい召喚---------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackShot4()
    {
        GameObject[] shooter = new GameObject[4];
        Vector3[] startPos = new Vector3[]
        {
            new Vector3(0f, 2f, 13f),
            new Vector3(-13f, 2f, 0f),
            new Vector3(13f, 2f, 0f),
            new Vector3(0f, 2f, -13f)
        };
        CanonOff();
        for(int i = 0; i < 4; i++)
        {
            shooter[i] = Instantiate(Shooter, startPos[i], Quaternion.identity);
        }
        yield return new WaitForSeconds(5f);
        for(int i = 0; i < 4; i++)
        {
            Destroy(shooter[i]);
        }
        CanonOn();
    }
    //床全消し--------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackAllBreak()
    {
        Vector3 startPos = default;
        int[] All = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        for(int i = 0; i < 16; i++)
        {
            startPos = Field[i].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        DestroyField(All);
        yield return new WaitForSeconds(1f);
        ReField();
    }
    //床複数破壊かつ床透明化--------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackStealth()
    {
        int[] OutField = { 0,0,0,0};
        int rand = default;
        bool stealth = false;
        Vector3 startPos = default;
        while (true)
        {
            rand = Random.Range(1, 17);
            bool duplicate = false;
            // 重複チェック
            for (int i = 0; i < OutField.Length; i++)
            {
                if (OutField[i] == rand)
                {
                    duplicate = true;
                    break;
                }
            }
            // 重複していなければ代入
            if (!duplicate)
            {
                for (int i = 0; i < OutField.Length; i++)
                {
                    if (OutField[i] == 0)
                    {
                        OutField[i] = rand;
                        break;
                    }
                }
            }
            if (OutField[3] != 0) break;
            yield return null;
        }
        for (int i = 0; i < 4; i++)
        {
            startPos = fi[OutField[i]-1].fiPre.transform.position;
            Attack(startPos, AOE1Field, 0);
        }
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 16; i++)
        {
            stealth = true;
            for(int j = 0; j < 4; j++)
            {
                if (i == OutField[j] - 1)
                {
                    stealth = false;
                }
            }
            if (stealth == true)
            {
                fi[i].fiSc.ObjectStealth();
            }
        }
        DestroyField(OutField);

        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 16; i++)
        {
            stealth = true;
            for (int j = 0; j < 4; j++)
            {
                if (i == OutField[j] - 1)
                {
                    stealth = false;
                }
            }
            if (stealth == true)
            {
                fi[i].fiSc.ObjectReStealth();
            }
        }
        ReField();
    }
    //回転するバー--------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackStick()
    {
        Vector3 startPos = new Vector3(0,1,0);
        int value=Random.Range(0,2);
        if (value == 0) value -= 1;
        Attack(startPos, AOEThin, 0);
        yield return new WaitForSeconds(1f);
        GameObject Bar = Instantiate(BLOCKBarLong, startPos, Quaternion.identity);

        float duration = 10f;      // 回転させる時間（秒）
        float elapsed = 0f;       // 経過時間

        while (elapsed < duration)
        {
            Bar.transform.Rotate(0, 90 * Time.deltaTime * value, 0);

            elapsed += Time.deltaTime;
            yield return null;    // 次のフレームまで待つ
        }

        // 回転終了後に消すなら
        Destroy(Bar);
    }

    //移動してくるバー----------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackBar()
    {
        int rand = Random.Range(0, 4);
        float time = 5;
        float elapsed = 0;
        Vector3 startPos = default;
        Vector3 endPos = default;
        GameObject Bar = null;
        switch (rand)
        {
            case 0:
                startPos = new Vector3(0, 1f, 13f);
                endPos = new Vector3(0, 1f, -13f);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 90, 0));
                break;
            case 1:
                startPos = new Vector3(-13, 1f, 0);
                endPos = new Vector3(13, 1f, 0);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 0, 0));
                break;
            case 2:
                startPos = new Vector3(13, 1f, 0);
                endPos = new Vector3(-13, 1f, 0);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 0, 0));
                break;
            case 3:
                startPos = new Vector3(0, 1f, -13f);
                endPos = new Vector3(0, 1f, 13f);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 90, 0));
                break;
        }
        while (elapsed<time)
        {
            float t = elapsed / time;
            Bar.transform.position= Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Bar.transform.position = endPos;
        Destroy(Bar);
    }
    //ドーナッツ範囲------------------------------------------------------------------------------------------------------------------------------
    private void AttackDonut()
    {
        Vector3 startPos = Vector3.zero;
        Attack(startPos, AOEDonut, 0);
    }
    //プレイヤー引き寄せ-------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackPull()
    {
        float distance = 10f;
        float duration = 0.4f;
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

            // 中心までの距離
            float distToCenter = Vector3.Distance(startPos, center);

            // 中心を越えないようにクランプ
            float pullDistance = Mathf.Min(distToCenter, distance);

            // 吹き飛ばし先の位置（中心を越えない）
            Vector3 targetPos = startPos + dir * -pullDistance;

            float elapsed = 0f;
            float speed = distance / duration;
            while (elapsed < duration) 
            { // 一定速度で中心方向へ移動
                Vector3 newPos = Vector3.MoveTowards( controller.transform.position, targetPos, speed * Time.deltaTime ); 
                controller.Move(newPos - controller.transform.position); 
                elapsed += Time.deltaTime; 
                yield return null; 
            }
        }
    }
    //プレイヤーを中心から吹き飛ばし---------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackPush()
    {
        float distance = 10f;
        float duration = 0.4f;
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
        DestroyField(Out);
        DestroyField(In);
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiSc != null)
            {
                Effi[i].fiSc.ObjectTrue();
                Effi[i].fiOn = true;
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
        ReField();
        for (int i = 0; i < 16; i++)
        {
            Effi[i].fiSc.ObjectFalse();
            Effi[i].fiOn = false;
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
        DestroyField(Out);
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiSc != null)
            {
                Effi[i].fiSc.ObjectTrue();
                Effi[i].fiOn = true;
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
        ReField();
        for (int i = 0; i < 16; i++)
        {
            Effi[i].fiSc.ObjectFalse();
            Effi[i].fiOn = false;
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
        DestroyField(Out);
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiSc != null)
            {
                Effi[i].fiSc.ObjectTrue();
                Effi[i].fiOn = true;
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
        ReField();
        for (int i = 0; i < 16; i++)
        {
            Effi[i].fiSc.ObjectFalse();
            Effi[i].fiOn = false;
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
        DestroyField(Out);
        yield return new WaitForSeconds(5f);
        ReField();
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
        DestroyField (In);
        yield return new WaitForSeconds(5f);
        ReField();
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
        if (fi[rand-1].fiSc != null)
        {
            fi[rand - 1].fiSc.ObjectFalse();
            fi[rand - 1].fiOn = false;
        }
        yield return new WaitForSeconds(5f);
        ReField();
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
        yield return new WaitForSeconds(3f);
        DestroyField(field);
        yield return new WaitForSeconds(5f);
        ReField();
    }
    //AOE表示処理---------------------------------------------------------------------------------------------------------
    private void Attack(Vector3 startPoint, GameObject prefab, float yRotationOffset)
    {
        startPoint.y = 50;
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
    //中央床破壊処理----------------------------------------------------------------------------------------------------------------------------
    private void DestroyField(int[] value)
    {
        for(int i = 0; i < value.Length; i++)
        {
            if (fi[value[i] - 1].fiSc != null)
            {
                fi[value[i] - 1].fiSc.ObjectFalse();
                fi[value[i] - 1].fiOn = false;
            }
        }
    }
    //中央床全表示-----------------------------------------------------------------------------------------------------------------------
    private void ReField()
    {
        for(int i = 0; i < 16; i++)
        {
            if (fi[i].fiOn == false)
            {
                fi[i].fiSc.ObjectTrue();
                fi[i].fiOn = true;
            }
        }
    }
    private void CanonOff()
    {
        BossField[] canonSc = new BossField[2];
        for(int i = 0; i < 2; i++)
        {
            canonSc[i] = Canon[i].GetComponent<BossField>();
            canonSc[i].ObjectFalse();
        }
    }
    private void CanonOn()
    {
        BossField[] canonSc = new BossField[2];
        for (int i = 0; i < 2; i++)
        {
            canonSc[i] = Canon[i].GetComponent<BossField>();
            canonSc[i].ObjectTrue();
        }
    }
}
