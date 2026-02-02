using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.Windows.Speech;
using Unity.VisualScripting.Antlr3.Runtime;
using static UnityEngine.Rendering.DebugUI;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.Rendering;


public class BossNormal : MonoBehaviour
{

    public AudioSource[] SE;
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
    public EffectClass[] Effect;
    public LayerMask targetLayerMask;
    public Slider slider;
    public TextMeshProUGUI text;
    public bool go = false;

    private float cooldown = 0;
    private BossHp bosshp;
    private field[] fi = new field[16];
    private field[] Effi = new field[16];
    struct field
    {
        public bool fiOn;
        public GameObject fiPre;
        public BossField fiSc;
    }
    [System.Serializable]
    public class EffectClass
    {
        public float EffEarlyTime;
        public GameObject EffPrefab;
    }
    public List<AttackCoro> Nfunc = new List<AttackCoro>();
    public List<AttackCoro> G2func = new List<AttackCoro>();
    public List<AttackCoro> G3func = new List<AttackCoro>();
    public List<AttackCoro> G4func = new List<AttackCoro>();

    public class AttackCoro
    {
        public bool flag;
        public Func<IEnumerator> func;
    }
    private Coroutine currentCombo = default(Coroutine);
    private List<Coroutine> runcoro = new List<Coroutine>();
    private List<GameObject> objList= new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Effect[0] = null;
        SE[0] = null;
        bosshp = GetComponent<BossHp>();
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboA()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboB()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboC()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboD()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboE()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboF()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboG()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboH()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboI()
        });
        Nfunc.Add(new AttackCoro
        {
            flag = false,
            func = () => NComboJ()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboA()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboB()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboC()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboD()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboE()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboF()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboG()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboH()
        });
        G2func.Add(new AttackCoro
        {
            flag = false,
            func = () => G2ComboI()
        });
        G3func.Add(new AttackCoro
        {
            flag = false,
            func = () => G3ComboA()
        });
        G3func.Add(new AttackCoro
        {
            flag = false,
            func = () => G3ComboB()
        });
        G3func.Add(new AttackCoro
        {
            flag = false,
            func = () => G3ComboC()
        });
        G3func.Add(new AttackCoro
        {
            flag = false,
            func = () => G3ComboD()
        });
        G3func.Add(new AttackCoro
        {
            flag = false,
            func = () => G3ComboE()
        });
        G3func.Add(new AttackCoro
        {
            flag = false,
            func = () => G3ComboF()
        });
        G4func.Add(new AttackCoro
        {
            flag = false,
            func = () => G4ComboA()
        });
        G4func.Add(new AttackCoro
        {
            flag = false,
            func = () => G4ComboB()
        });
        G4func.Add(new AttackCoro
        {
            flag = false,
            func = () => G4ComboC()
        });
        G4func.Add(new AttackCoro
        {
            flag = false,
            func = () => G4ComboD()
        });
        Debug.Log("左シフト：\n1.外内,2.エリア破壊,3.半面破壊,4.円,5.縦爪,6.横爪");
        Debug.Log("右シフト：\n1.外側破壊,2.内側破壊,3.星,4.星内破壊,5.剣,6.剣交差,7.剣交差内破壊");
        Debug.Log("左オルト：\n1.押し出し,2.引き寄せ,3.ドーナツ,4.バー,5.回転バー,6.ステルス,7.全消し");
        Debug.Log("Pキー :\n1.上下に弾召喚,2.内側に弾召喚,3.お化け召喚");
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
        switch (bosshp.state)
        {
            case BossHp.State.normal:
                if (go)
                {
                    // 1. 全部 true ならリセット
                    bool allTrue = true;
                    for (int i = 0; i < Nfunc.Count; i++)
                    {
                        if (!Nfunc[i].flag)
                        {
                            allTrue = false;
                            break;
                        }
                    }
                    if (allTrue)
                    {
                        for (int i = 0; i < Nfunc.Count; i++)
                        {
                            var tmp = Nfunc[i];
                            tmp.flag = false;
                            Nfunc[i] = tmp;
                        }
                    }
                    int r = Random.Range(0, Nfunc.Count);
                    while (Nfunc[r].flag)
                    {
                        r = Random.Range(0, Nfunc.Count);
                    }
                    currentCombo = StartCoroutine(Nfunc[r].func());
                    Nfunc[r].flag = true;
                    go = false;
                }
                break;
            case BossHp.State.gear2:
                if (go)
                {
                    // 1. 全部 true ならリセット
                    bool allTrue = true;
                    for (int i = 0; i < G2func.Count; i++)
                    {
                        if (!G2func[i].flag)
                        {
                            allTrue = false;
                            break;
                        }
                    }
                    if (allTrue)
                    {
                        for (int i = 0; i < G2func.Count; i++)
                        {
                            var tmp = G2func[i];
                            tmp.flag = false;
                            G2func[i] = tmp;
                        }
                    }
                    int r = Random.Range(0, G2func.Count);
                    while (G2func[r].flag)
                    {
                        r = Random.Range(0, G2func.Count);
                    }
                    currentCombo = StartCoroutine(G2func[r].func());
                    G2func[r].flag = true;
                    go = false;
                }
                break;
            case BossHp.State.gear3:
                if (go)
                {
                    // 1. 全部 true ならリセット
                    bool allTrue = true;
                    for (int i = 0; i < G3func.Count; i++)
                    {
                        if (!G3func[i].flag)
                        {
                            allTrue = false;
                            break;
                        }
                    }
                    if (allTrue)
                    {
                        for (int i = 0; i < G3func.Count; i++)
                        {
                            var tmp = G3func[i];
                            tmp.flag = false;
                            G3func[i] = tmp;
                        }
                    }
                    int r = Random.Range(0, G3func.Count);
                    while (G3func[r].flag)
                    {
                        r = Random.Range(0, G3func.Count);
                    }
                    currentCombo = StartCoroutine(G3func[r].func());
                    G3func[r].flag = true;
                    go = false;
                }
                break;
            case BossHp.State.gear4:
                if (go)
                {
                    // 1. 全部 true ならリセット
                    bool allTrue = true;
                    for (int i = 0; i < G4func.Count; i++)
                    {
                        if (!G4func[i].flag)
                        {
                            allTrue = false;
                            break;
                        }
                    }
                    if (allTrue)
                    {
                        for (int i = 0; i < G4func.Count; i++)
                        {
                            var tmp = G4func[i];
                            tmp.flag = false;
                            G4func[i] = tmp;
                        }
                    }
                    int r = Random.Range(0, G4func.Count);
                    while (G4func[r].flag)
                    {
                        r = Random.Range(0, G4func.Count);
                    }
                    currentCombo = StartCoroutine(G4func[r].func());
                    G4func[r].flag = true;
                    go = false;
                }
                break;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackThin(1));
                StartCoroutine(Cast("こうげき！", 5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(Attack1Field(1,5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(Attack8Field(3,5,0));
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(AttackLockOn(1, 1, 1));
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(AttackVirtical(1,5,0));
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(AttackHrizon(1,5,0));
            }
            
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackOut(1,5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(AttackIn(1,5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(AttackStar(1));
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(AttackStar2(1,2,1,2));
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(AttackSword(1,1,2,2,3));
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(AttackSword2(1,1,3,2,5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                StartCoroutine(AttackSword3(1,1,2,2,5));
            }
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackPush(3));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(AttackPull(3));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttackDonut(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(AttackBar(0));
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(AttackStick(2,10));
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(AttackStealth(1,5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                StartCoroutine(AttackAllBreak(1,1));
            }
        }
        if (Input.GetKey(KeyCode.P))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(AttackShot2(5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(AttackShotIn(1,5));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(AttackGhost(3,15));
            }
        }
        
       
    }
    //gear4用コルーチン------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator G4ComboA()
    {
        StartCoroutine(Cast("よちょうみてね", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackSword2(2, 1, 2, 2, 4));
        runcoro.Add(c1);
        yield return new WaitForSeconds(12);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G4ComboB()
    {
        StartCoroutine(Cast("スター", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackStar2(3,2,0.75f,3));
        runcoro.Add(c1);
        yield return new WaitForSeconds(16);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G4ComboC()
    {
        StartCoroutine(Cast("うちがわからタマ", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackShotIn(3, 15));
        runcoro.Add(c1);
        StartCoroutine(Cast("ぐるぐる", 2));
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(AttackStick(2, 10));
        runcoro.Add(c2);
        yield return new WaitForSeconds(17);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G4ComboD()
    {
        StartCoroutine(Cast("たてよこ", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackVirtical(0.5f, 5, 0));
        runcoro.Add(c1);
        Coroutine c2 = StartCoroutine(AttackHrizon(0.5f, 5, 0));
        runcoro.Add(c2);
        StartCoroutine(Cast("タイミング", 2));
        yield return new WaitForSeconds(2);
        Coroutine c3 = StartCoroutine(AttackAllBreak(2, 0.7f));
        runcoro.Add(c3);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }

    //gear3用コルーチン------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator G3ComboA()
    {
        StartCoroutine(Cast("スター", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackStar(1));
        runcoro.Add(c1);
        yield return new WaitForSeconds(10);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G3ComboB()
    {
        StartCoroutine(Cast("たまいっぱい", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackShot2(10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(11);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G3ComboC()
    {
        StartCoroutine(Cast("よちょうみてね", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackSword(2, 1, 2, 2, 4));
        runcoro.Add(c1);
        yield return new WaitForSeconds(12);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G3ComboD()
    {
        StartCoroutine(Cast("そとから", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackThin(2));
        runcoro.Add(c1);
        yield return new WaitForSeconds(17);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G3ComboE()
    {
        StartCoroutine(Cast("たてよこ", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackVirtical(0.5f, 5, 0));
        runcoro.Add(c1);
        Coroutine c2 = StartCoroutine(AttackHrizon(0.5f, 5, 0));
        runcoro.Add(c2);
        yield return new WaitForSeconds(7);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G3ComboF()
    {
        StartCoroutine(Cast("ぐるぐる", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackStick(2, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(3);
        StartCoroutine(Cast("バー", 2));
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(AttackBar(1));
        runcoro.Add(c2);
        Coroutine c3 = StartCoroutine(AttackBar(3));
        runcoro.Add(c3);
        yield return new WaitForSeconds(4);
        Coroutine c4 = StartCoroutine(AttackBar(2));
        runcoro.Add(c4);
        Coroutine c5 = StartCoroutine(AttackBar(4));
        runcoro.Add(c5);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }

    //gear2用コルーチン------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator G2ComboA()
    {
        StartCoroutine(Cast("うちがわからタマ", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackShotIn(3, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(14);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboB()
    {
        StartCoroutine(Cast("タイミング", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackAllBreak(3, 0.7f));
        runcoro.Add(c1);
        yield return new WaitForSeconds(5);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboC()
    {
        StartCoroutine(Cast("ステルス", 3));
        yield return new WaitForSeconds(3);
        Coroutine c1 = StartCoroutine(AttackStealth(3, 5));
        runcoro.Add(c1);
        yield return new WaitForSeconds(9);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboD()
    {
        StartCoroutine(Cast("バー", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackBar(1));
        runcoro.Add(c1);
        Coroutine c2 = StartCoroutine(AttackBar(3));
        runcoro.Add(c2);
        yield return new WaitForSeconds(2);
        StartCoroutine(Cast("ついび", 2));
        yield return new WaitForSeconds(2);
        Coroutine c3 = StartCoroutine(AttackLockOn(1.5f, 2, 3));
        runcoro.Add(c3);
        yield return new WaitForSeconds(2);
        Coroutine c4 = StartCoroutine(AttackBar(2));
        runcoro.Add(c4);
        Coroutine c5 = StartCoroutine(AttackBar(4));
        runcoro.Add(c5);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboE()
    {
        StartCoroutine(Cast("ぐるぐる", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackStick(2, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(5);
        StartCoroutine(Cast("レフトサイド", 2));
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(Attack8Field(3, 5, 3));
        runcoro.Add(c2);
        yield return new WaitForSeconds(3);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboF()
    {
        StartCoroutine(Cast("ぐるぐる", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackStick(2, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(5);
        StartCoroutine(Cast("ライトサイド", 2));
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(Attack8Field(3, 5, 2));
        runcoro.Add(c2);
        yield return new WaitForSeconds(3);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboG()
    {
        StartCoroutine(Cast("そとがわ", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackOut(2, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(3);
        StartCoroutine(Cast("ぐるぐる", 2));
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(AttackStick(2, 5));
        runcoro.Add(c2);
        yield return new WaitForSeconds(8);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboH()
    {
        StartCoroutine(Cast("うちがわ", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackIn(2, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(3);
        StartCoroutine(Cast("ついび", 2));
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(AttackLockOn(1.5f, 2, 2));
        runcoro.Add(c2);
        yield return new WaitForSeconds(8);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator G2ComboI()
    {
        StartCoroutine(Cast("おしだす", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackPush(3));
        runcoro.Add(c1);
        yield return new WaitForSeconds(4);
        StopAllAttackCoroutines();
        go = true;
    }

    //normal用コルーチン------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator NComboA()
    {
        StartCoroutine(Cast("エリアけすよ", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(Attack1Field(2, 100));
        runcoro.Add(c1);
        yield return new WaitForSeconds(2);
        Coroutine c2 = StartCoroutine(Attack1Field(2, 100));
        runcoro.Add(c2);
        yield return new WaitForSeconds(2);
        Coroutine c3 = StartCoroutine(Attack1Field(2, 100));
        runcoro.Add(c3);
        yield return new WaitForSeconds(5);
        ReField();
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboB()
    {
        StartCoroutine(Cast("ついび", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackLockOn(2, 2, 3));
        runcoro.Add(c1);
        yield return new WaitForSeconds(13);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboC()
    {
        StartCoroutine(Cast("バーティカル", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackVirtical(0.5f, 5,0));
        runcoro.Add(c1);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboD()
    {
        StartCoroutine(Cast("ホライゾン", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackHrizon(0.5f, 5, 0));
        runcoro.Add(c1);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboE()
    {
        StartCoroutine(Cast("バー", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackBar(1));
        runcoro.Add(c1);
        Coroutine c2 = StartCoroutine(AttackBar(3));
        runcoro.Add(c2);
        yield return new WaitForSeconds(4);
        Coroutine c3 = StartCoroutine(AttackBar(2));
        runcoro.Add(c3);
        Coroutine c4 = StartCoroutine(AttackBar(4));
        runcoro.Add(c4);
        yield return new WaitForSeconds(6);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboF()
    {
        StartCoroutine(Cast("ぐるぐる", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackStick(2, 10));
        runcoro.Add(c1);
        yield return new WaitForSeconds(15);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboG()
    {
        StartCoroutine(Cast("そとがわ", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackOut(2, 5));
        runcoro.Add(c1);
        yield return new WaitForSeconds(8);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboH()
    {
        StartCoroutine(Cast("うちがわ", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(AttackIn(2, 5));
        runcoro.Add(c1);
        yield return new WaitForSeconds(8);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboI()
    {
        StartCoroutine(Cast("レフトサイド", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(Attack8Field(2, 5, 3));
        runcoro.Add(c1);
        yield return new WaitForSeconds(8);
        StopAllAttackCoroutines();
        go = true;
    }
    private IEnumerator NComboJ()
    {
        StartCoroutine(Cast("ライトサイド", 2));
        yield return new WaitForSeconds(2);
        Coroutine c1 = StartCoroutine(Attack8Field(2, 5, 2));
        runcoro.Add(c1);
        yield return new WaitForSeconds(8);
        StopAllAttackCoroutines();
        go = true;
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //お化け--------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackGhost(float st,float et)
    {
        Vector3 startPoint = new Vector3(0.6f, 50f, -0.4f);
        Ray ray = new Ray(startPoint, Vector3.down);
        RaycastHit hit;
        GameObject spawnPrefab = default;
        GameObject[] ghost=new GameObject[9];
        Ghost[] ghostSc=new Ghost[9];
        int[] ghostlota = new int[] { 180, 135, 225, 45, 315, 180, 90, 270, 0 };
        int[] speed = new int[] { 1, 3, 3, 3, 3, 2, 2, 2, 2 };
        Vector3[] startPos = new Vector3[]
        {
            new Vector3 (0,0,0),
            new Vector3 (-6,0,6),new Vector3 (6,0,6),new Vector3 (-6,0,-6),new Vector3 (6,0,-6),
            new Vector3 (0,0,12),new Vector3 (-12,0,0),new Vector3 (12,0,0),new Vector3 (0,0,-12),
        };
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
        {
            spawnPrefab = Instantiate(AOEDonut, hit.point, Quaternion.identity);
            spawnPrefab.GetComponent<AOEDonut>().time =st;
        }
        yield return new WaitForSeconds(st- Effect[10].EffEarlyTime);
        Instantiate(Effect[10].EffPrefab, new Vector3(0, 0.8f, 0), Quaternion.Euler(0,0,0));
        Instantiate(Effect[10].EffPrefab, new Vector3(0, 0.8f, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(Effect[10].EffPrefab, new Vector3(0, 0.8f, 0), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(Effect[10].EffEarlyTime);
        for (int i = 0; i < ghost.Length; i++)
        {
            ghost[i] = Instantiate(Ghost, startPos[i], Quaternion.Euler(0, ghostlota[i], 0));
            objList.Add(ghost[i]);
            ghostSc[i] = ghost[i].GetComponent<Ghost>();
            ghostSc[i].Speed = speed[i];
            if (i <= 4) { ghostSc[i].Circular = true; }
            if (i == 1 || i == 4) { ghostSc[i].sZ = -4; }
            if (i == 5) { ghostSc[i].sX = 4; ghostSc[i].sZ = 0; }
            else if (i == 6) { ghostSc[i].sX = 0; ghostSc[i].sZ = 4; }
            else if (i == 7) { ghostSc[i].sX = 0; ghostSc[i].sZ = -4; }
            else if (i == 8) { ghostSc[i].sX = -4; ghostSc[i].sZ = 0; }
        }
        SE[11].PlayDelayed(0);
        yield return new WaitForSeconds(et);
        for (int i = 0; i < ghost.Length;i++)
        {
            objList.Remove(ghost[i]);
            Destroy(ghost[i]);
        }
    }
    //内側消去から中心から弾を召喚------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackShotIn(float st,float et)
    {
        Vector3 startPos = default;
        int[] In = { 6, 7, 10, 11 };
        for (int i = 0; i < 4; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 0, 0,0,0);
        }
        yield return new WaitForSeconds(st);
        DestroyField(In);
        

        GameObject shooter = default;
        startPos = new Vector3(0f, 2f, 0f);
        shooter = Instantiate(Shooter, startPos, Quaternion.identity);
        objList.Add(shooter);
        yield return new WaitForSeconds(et);
        objList.Remove(shooter);
        Destroy(shooter);
        ReField();
    }
    //弾をいっぱい召喚---------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackShot4(float et)
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
            objList.Add(shooter[i]);
        }
        yield return new WaitForSeconds(et);
        for(int i = 0; i < 4; i++)
        {
            objList.Remove(shooter[i]);
            Destroy(shooter[i]);
        }
        CanonOn();
    }
    //砲を消さずに弾をいっぱい召喚---------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackShot2(float et)
    {
        GameObject[] shooter = new GameObject[4];
        Vector3[] startPos = new Vector3[]
        {
            new Vector3(0f, 2f, 13f),
            new Vector3(0f, 2f, -13f)
        };
        for (int i = 0; i < 2; i++)
        {
            shooter[i] = Instantiate(Shooter, startPos[i], Quaternion.identity);
            objList.Add(shooter[i]);
        }
        yield return new WaitForSeconds(et);
        for (int i = 0; i < 2; i++)
        {
            objList.Remove(shooter[i]);
            Destroy(shooter[i]);
        }
    }
    //床全消し--------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackAllBreak(float st,float et)
    {
        Vector3 startPos = default;
        float SEDelay = 0.5f;
        int[] All = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        for(int i = 0; i < 16; i++)
        {
            startPos = Field[i].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 0, 0, 0, 0);
        }
        yield return new WaitForSeconds(st-SEDelay);
        SE[8].PlayDelayed(0);
        yield return new WaitForSeconds(SEDelay);
        Instantiate(Effect[9].EffPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        DestroyField(All);
        yield return new WaitForSeconds(et);
        ReField();
        
    }
    //床複数破壊かつ床透明化--------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackStealth(float st,float et)
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
            Attack(startPos, AOE1Field, 0,st, 6, 0, 0, 0);
        }
        yield return new WaitForSeconds(st);
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

        yield return new WaitForSeconds(et);
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
    private IEnumerator AttackStick(float st,float et)
    {
        Vector3 startPos = new Vector3(0,1,0);
        int value=Random.Range(0,2);
        if (value == 0) value -= 1;
        Attack(startPos, AOEThin, 0,st, 0, 0, 0, 0);
        yield return new WaitForSeconds(st);
        GameObject Bar = Instantiate(BLOCKBarLong, startPos, Quaternion.identity);
        objList.Add(Bar);
        float duration = et;      // 回転させる時間（秒）
        float elapsed = 0f;       // 経過時間

        while (elapsed < duration)
        {
            Bar.transform.Rotate(0, 90 * Time.deltaTime * value, 0);

            elapsed += Time.deltaTime;
            yield return null;    // 次のフレームまで待つ
        }
        objList.Remove(Bar);
        // 回転終了後に消すなら
        Destroy(Bar);
    }
    //移動してくるバー----------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackBar(int value)
    {
        int rand = 0;
        if (value == 0) { rand = Random.Range(1, 5); }
        else { rand = value; }
        float time = 5;
        float elapsed = 0;
        Vector3 startPos = default;
        Vector3 endPos = default;
        GameObject Bar = null;
        switch (rand)
        {
            case 1:
                startPos = new Vector3(0, 1f, 13f);
                endPos = new Vector3(0, 1f, -13f);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 90, 0));
                break;
            case 2:
                startPos = new Vector3(-13, 1f, 0);
                endPos = new Vector3(13, 1f, 0);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 0, 0));
                break;
            case 3:
                startPos = new Vector3(13, 1f, 0);
                endPos = new Vector3(-13, 1f, 0);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 0, 0));
                break;
            case 4:
                startPos = new Vector3(0, 1f, -13f);
                endPos = new Vector3(0, 1f, 13f);
                Bar = Instantiate(BLOCKBar, startPos, Quaternion.Euler(0, 90, 0));
                break;
        }
        objList.Add(Bar);
        while (elapsed<time)
        {
            float t = elapsed / time;
            Bar.transform.position= Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Bar.transform.position = endPos;
        objList.Remove(Bar);
        Destroy(Bar);
    }
    //ドーナッツ範囲------------------------------------------------------------------------------------------------------------------------------
    private void AttackDonut(float st)
    {
        Vector3 startPoint = new Vector3(0.6f,50f,-0.4f);
        Ray ray = new Ray(startPoint, Vector3.down);
        RaycastHit hit;
        GameObject spawnPrefab = default;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
        {
            spawnPrefab = Instantiate(AOEDonut, hit.point, Quaternion.identity);
            spawnPrefab.GetComponent<AOEDonut>().time = st;
        }
    }
    //プレイヤー引き寄せ-------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AttackPull(float st)
    {
        float distance = 10f;
        float duration = 0.4f;
        Vector3 center = Vector3.zero;
        Vector3 goPos = new Vector3(center.x, 50, center.z);
        CharacterController[] players = FindObjectsOfType<CharacterController>();
        Attack(goPos, AOEPush, 0,st, 0, 0, 0, 0);
        yield return new WaitForSeconds(st);
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
    private IEnumerator AttackPush(float st)
    {
        float distance = 10f;
        float duration = 0.4f;
        Vector3 center = Vector3.zero;
        Vector3 goPos = new Vector3(center.x, 50, center.z);
        CharacterController[] players = FindObjectsOfType<CharacterController>();
        Attack(goPos, AOEPush, 0,st, 8, 0, 7, 0);
      
        yield return new WaitForSeconds(st);
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
    private IEnumerator AttackSword3(float st,float st2,float et,float cool,float let)
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
            Attack(startPos, AOE1Field, 0,st, 6, 0, 6, 0);
        }
        for (int i = 0; i < 2; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 0, 0, 6, 0);
        }
        yield return new WaitForSeconds(st);
        DestroyField(Out);
        DestroyField(In);
        CanonOff();
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiSc != null)
            {
                Effi[i].fiSc.ObjectTrue();
                Effi[i].fiOn = true;
            }
        }
        yield return new WaitForSeconds(cool);
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 0, 0)); attackPos[i + (j * 2)] = startPos;
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0,270, 0)); attackPos[i + (j * 2)] = startPos;
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 90, 0)); attackPos[i + (j * 2)] = startPos;
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 180, 0)); attackPos[i + (j * 2)] = startPos;
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

        yield return new WaitForSeconds(let);
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                if (rand % 4 == 0 || rand % 4 == 3)
                {
                    if (j == 0)
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                    }
                    else
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                    }
                }
                else
                {
                    if (j == 1)
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                    }
                    else
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                    }
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            objList.Add(sword[i]);
        }
        //外周エリア再出現
        yield return new WaitForSeconds(et);

        for (int i = 0; i < 4; i++)
        {
            objList.Remove(sword[i]);
            Destroy(sword[i]);
        }
        ReField();
        CanonOn();
        for (int i = 0; i < 16; i++)
        {
            Effi[i].fiSc.ObjectFalse();
            Effi[i].fiOn = false;
        }
    }
    //外側のフィールドの予兆からの攻撃2--------------------------------------------------------------------------------------------------
    private IEnumerator AttackSword2(float st,float st2,float et,float cool,float let)
    {
        //外周削除
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        for (int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 6, 0, 6, 0);
        }
        yield return new WaitForSeconds(st);
        DestroyField(Out);
        CanonOff();
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiSc != null)
            {
                Effi[i].fiSc.ObjectTrue();
                Effi[i].fiOn = true;
            }
        }
        yield return new WaitForSeconds(cool);
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 0, 0)); attackPos[i + (j * 2)] = startPos;
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 270, 0)); attackPos[i + (j * 2)] = startPos;
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 90, 0)); attackPos[i + (j * 2)] = startPos;
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
                        sword[i + (j * 2)] = Instantiate(Nail, goPos, Quaternion.Euler(0, 180, 0)); attackPos[i + (j * 2)] = startPos;
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
        for (int i = 0; i < 4; i++)
        {
            objList.Add(sword[i]);
        }
        yield return new WaitForSeconds(let);
        for(int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                if (rand % 4 == 0 || rand % 4 == 3)
                {
                    if (j == 0)
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                    }
                    else
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                    }
                }
                else
                {
                    if (j == 1)
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 1, 2, 0);
                    }
                    else
                    {
                        Attack(attackPos[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                        Attack(attackPos2[i + (j * 2)], AOEThinHalf, rota[j], st2, 5, 0, 2, 0);
                    }
                }
            }
        }
        
        //外周エリア再出現
        yield return new WaitForSeconds(et);

        for (int i = 0; i < 4; i++)
        {
            Destroy(sword[i]);
            objList.Remove(sword[i]);
        }
        ReField();
        CanonOn();
        for (int i = 0; i < 16; i++)
        {
            Effi[i].fiSc.ObjectFalse();
            Effi[i].fiOn = false;
        }
    }
    //外側のフィールドの予兆からの攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackSword(float st,float st2,float et,float cool,float let)
    {
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        for (int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 6, 0, 6, 0);
        }
        yield return new WaitForSeconds(st);
        DestroyField(Out);
        CanonOff();
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiSc != null)
            {
                Effi[i].fiSc.ObjectTrue();
                Effi[i].fiOn = true;
            }
        }
        yield return new WaitForSeconds(cool);
        int rand = Random.Range(0, 8);
        int rota = default;
        Vector3 goPos = Vector3.zero;
        GameObject[] sword = new GameObject[2];
        Vector3[] attackPos=new Vector3[2];
        Vector3[] attackPos2 = new Vector3[2];
        switch (rand % 4)
        {
            case 0:
                goPos = new Vector3(-4.5f, 3f, 30f);
                if (rand >= 4) goPos.x += 3f;
                startPos = new Vector3(goPos.x, 50f, goPos.z - 6);
                for (int i = 0; i < 2; i++)
                {
                    sword[i] = Instantiate(Nail, goPos, Quaternion.Euler(0,0,0));
                    attackPos[i] = new Vector3(startPos.x, startPos.y, 0);
                    attackPos2[i] = startPos;
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
                    sword[i] = Instantiate(Nail, goPos, Quaternion.Euler(0, 90, 0));
                    attackPos[i]= new Vector3(0,startPos.y,startPos.z);
                    attackPos2[i] = startPos;
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
                    sword[i] = Instantiate(Nail, goPos, Quaternion.Euler(0, 270, 0));
                    attackPos[i] = new Vector3(0, startPos.y, startPos.z);
                    attackPos2[i] = startPos;
                    goPos.z -= 6f;
                    startPos.z -= 6f;
                }
                rota = 90;
                break;
            case 3:
                goPos = new Vector3(-4.5f, 3f, -30f);
                if (rand >= 4) goPos.x += 3f;
                startPos = new Vector3(goPos.x, 50f, goPos.z + 6);
                for (int i = 0; i < 2; i++)
                {
                    sword[i] = Instantiate(Nail, goPos, Quaternion.Euler(0, 180, 0));
                    attackPos[i] = new Vector3(startPos.x, startPos.y, 0);
                    attackPos2[i] = startPos;
                    goPos.x += 6f;
                    startPos.x += 6f;
                }
                rota = 0;
                break;
        }
        for (int i = 0; i < 2; i++)
        {
            objList.Add(sword[i]);
        }
        yield return new WaitForSeconds(let);
        for(int i = 0; i < 2; i++)
        {
            if (rand % 4 == 0 || rand % 4 == 3)
            {
                Attack(attackPos[i], AOEThinHalf, rota, st2, 5, 0, 2, 0);
                Attack(attackPos2[i], AOEThinHalf, rota, st2, 5, 0, 2, 0);
            }
            else
            {
                Attack(attackPos[i], AOEThinHalf, rota, st2, 5, 1, 2, 0);
                Attack(attackPos2[i], AOEThinHalf, rota, st2, 5, 1, 2, 0);
            }
        }
        //外周エリア再出現
        yield return new WaitForSeconds(et);

        for (int i = 0; i < 2; i++)
        {
            objList.Remove(sword[i]);
            Destroy(sword[i]);
        }
        ReField();
        CanonOn();
        for (int i = 0; i < 16; i++)
        {
            Effi[i].fiSc.ObjectFalse();
            Effi[i].fiOn = false;
        }
    }
    //星が重なった場所から攻撃2--------------------------------------------------------------------------------------------------
    private IEnumerator AttackStar2(float st,float cool,float st2,float et)
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
            Attack(startPos, AOE1Field, 0,st, 6, 0, 0, 0);
        }
        yield return new WaitForSeconds(st);
        BossField[] FieldScript = { null, null, null, null, null, null, null, null, null, null, null, null };
        for (int i = 0; i < 4; i++)
        {
            FieldScript[i] = Field[(In[i] - 1)].GetComponent<BossField>();
            if (FieldScript[i] != null)
            {
                FieldScript[i].ObjectFalse();
            }
        }
        yield return new WaitForSeconds(cool);
        for (int i = 0; i < 2; i++)
        {
            float angle = startAngleDegrees * Mathf.Deg2Rad;
            Vector3 center = new Vector3(0, 5, 0);
            float x = center.x + Mathf.Cos(angle) * 5;
            float z = center.z + Mathf.Sin(angle) * 5;
            goPos = new Vector3(x, center.y, z);
            StarMana[i] = Instantiate(Star[i], goPos, Quaternion.identity);
            objList.Add(StarMana[i]);
            StarSc[i] = StarMana[i].GetComponent<Star>();
            StarSc[i].angle = angle;
            if (startAngleDegrees > 180) startAngleDegrees -= 180;
            else startAngleDegrees += 180;
        }
        SE[4].PlayDelayed(0);
        yield return new WaitUntil(() =>
            StarMana[0] != null && StarMana[1] != null &&
                Vector3.Distance(StarMana[0].transform.position, StarMana[1].transform.position) < 0.1f);
        goPos = StarMana[0].transform.position;
        for(int i = 0; i < 2; i++)
        {
            objList.Remove(StarMana[i]);
            Destroy(StarMana[i]);
        }
        SE[4].Stop();
        Attack(goPos, AOEBigCircle, 0,st2, 7, 0, 3, 0);
        yield return new WaitForSeconds(et);
        if (FieldScript != null)
        {
            for (int i = 0; i < 4; i++)
            {
                FieldScript[i].ObjectTrue();
            }
        }
    }
    //星が重なった場所から攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackStar(float st)
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
            objList.Add(StarMana[i]);
            StarSc[i]=StarMana[i].GetComponent<Star>();
            StarSc[i].angle = angle;
            if (startAngleDegrees > 180) startAngleDegrees -= 180;
            else startAngleDegrees += 180;
        }
        SE[4].PlayDelayed(0);
        yield return new WaitUntil(() =>
            StarMana[0] != null && StarMana[1] != null &&
                Vector3.Distance(StarMana[0].transform.position, StarMana[1].transform.position) < 0.1f);
        goPos = StarMana[0].transform.position;
        for (int i = 0; i < 2; i++)
        {
            objList.Remove(StarMana[i]);
            Destroy(StarMana[i]);
        }
        SE[4].Stop();
        Attack(goPos, AOEBigCircle, 0,st, 7, 0, 3, 0);
    }
    //横の1列以外に攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackHrizon(float st, float let, float value)
    {
        int rand = Random.Range(0, 8);
        Vector3 startPos = new Vector3(15f, 3f, -10.5f);
        Vector3 goPos = new Vector3(0, 0, 0);
        Vector3 letPos = new Vector3(45 * value, 90, 0);
        GameObject[] nail = { null, null, null, null, null, null, null, null };
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                goPos = new Vector3(startPos.x, startPos.y, startPos.z + (3 * i));
                nail[i] = Instantiate(Nail, goPos, Quaternion.Euler(letPos));
                objList.Add(nail[i]);
            }
        }
        yield return new WaitForSeconds(let);
        startPos = new Vector3(0, 50f, -10.5f);
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                Attack(startPos, AOEThin, 90, st, 5, 1, 2, 0);
            }
            startPos.z += 3;
        }
        yield return new WaitForSeconds(st);
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                objList.Remove(nail[i]);
                Destroy(nail[i]);
            }
        }
    }
    //縦の1列以外に攻撃--------------------------------------------------------------------------------------------------
    private IEnumerator AttackVirtical(float st, float let, float value)
    {
        int rand = Random.Range(0, 8);
        Vector3 startPos = new Vector3(-10.5f, 3f, 15f);
        Vector3 goPos = new Vector3(0, 0, 0);
        Vector3 letPos = new Vector3(0, 0, 45 * value);
        GameObject[] nail = { null, null, null, null, null, null, null, null };
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                goPos = new Vector3(startPos.x + (3 * i), startPos.y, startPos.z);
                nail[i] = Instantiate(Nail, goPos, Quaternion.Euler(letPos));
                objList.Add(nail[i]);
            }
        }
        yield return new WaitForSeconds(let);
        startPos = new Vector3(-10.5f, 50f, 0);
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                Attack(startPos, AOEThin, 0, st, 5, 0, 2, 0);
                
            }
            startPos.x += 3;
        }
        yield return new WaitForSeconds(st);
        for (int i = 0; i < 8; i++)
        {
            if (rand != i)
            {
                objList.Remove(nail[i]);
                Destroy(nail[i]);
            }
        }
    }
    //外周破壊--------------------------------------------------------------------------------------------------
    private IEnumerator AttackOut(float st,float et)
    {
        Vector3 startPos = default;
        int[] Out = { 1, 2, 3, 4, 5, 8, 9, 12, 13, 14, 15, 16 };
        for(int i = 0; i < 12; i++)
        {
            startPos = Field[(Out[i]-1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 6,0, 6, 0);
        }
        yield return new WaitForSeconds(st);
        DestroyField(Out);
        yield return new WaitForSeconds(et);
        ReField();
    }
    //内側破壊--------------------------------------------------------------------------------------------------
    private IEnumerator AttackIn(float st,float et)
    {
        Vector3 startPos = default;
        int[] In = { 6,7,10,11 };
        for (int i = 0; i < 4; i++)
        {
            startPos = Field[(In[i] - 1)].transform.position;
            startPos.y = 50;
            Attack(startPos, AOE1Field, 0,st, 6, 0, 6, 0);
        }
        yield return new WaitForSeconds(st);
        DestroyField (In);
        yield return new WaitForSeconds(et);
        ReField();
    }
    //プレイヤー地点に追尾攻撃-------------------------------------------------------------------------------------------
    private IEnumerator AttackLockOn(float st,float cool,int value)
    {
        for(int i = 0; i < value; i++)
        {
            AttackCircle(st);
            yield return new WaitForSeconds(cool);
        }
    }
    //プレイヤー地点に攻撃--------------------------------------------------------------------------------------------------
    private void AttackCircle(float st)
    {
        Vector3 startPos = new Vector3(0,0,0);
        GameObject obj = GameObject.FindWithTag("Player");
        if (obj == null) return;
        else
        {
            startPos = obj.transform.position;
        }
        startPos.y = 50;
        Attack(startPos, AOECircle, 0,st, 1, 0, 10, st-0.8f);
    }
    //外から内　内から外--------------------------------------------------------------------------------------------------
    private IEnumerator AttackThin(float st)
    {
        Vector3 startPosU = new Vector3(0f, 50f, -10.5f);
        Vector3 startPosL = new Vector3(-10.5f, 50f, 0f);
        Vector3 startPosR = new Vector3(10.5f, 50f, 0f);
        Vector3 startPosD = new Vector3(0f, 50f, 10.5f);
        for(int i = 0; i < 8; i++)
        {
            Attack(startPosU, AOEThin,90,st, 2, 1, 5, 0);
            Attack(startPosL, AOEThin,0,st, 2,0, 5, 0);
            Attack(startPosR, AOEThin,0,st, 2, 2, 5, 0);
            Attack(startPosD, AOEThin,90,st, 2, 3, 5, 0);
            startPosU.z += 3;
            startPosL.x += 3;
            startPosR.x -= 3;
            startPosD.z -= 3;
            yield return new WaitForSeconds(st);
        }
        
    }
    //１ブロック破壊--------------------------------------------------------------------------------------------------
    private IEnumerator Attack1Field(float st,float et)
    {
        int rand=Random.Range(1, 17);
        Vector3 startPos = Field[rand - 1].transform.position;
        startPos.y = 50f;
        Attack(startPos, AOE1Field, 0,st, 4,0, 9, st-0.8f);
        yield return new WaitForSeconds(st);
        if (fi[rand-1].fiSc != null)
        {
            fi[rand - 1].fiSc.ObjectFalse();
            fi[rand - 1].fiOn = false;
        }
        yield return new WaitForSeconds(et);
        ReField();
    }
    //半面攻撃--------------------------------------------------------------------------------------------------------------------
    private IEnumerator Attack8Field(float st,float et,int value)
    {
        int rand = 0;
        if (value == 0) { rand = Random.Range(1, 5); }
        else { rand = value; }
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
        if (rand == 1 || rand == 4)
        {
            Attack(startPos, AOE8Field, rota, st, 3,0, 1, 0.2f);
        }
        else
        {
            Attack(startPos, AOE8Field, rota, st, 3, 1, 1, 0.2f);
        }
        yield return new WaitForSeconds(st);
        DestroyField(field);
        yield return new WaitForSeconds(et);
        ReField();
    }
    //AOE表示処理---------------------------------------------------------------------------------------------------------
    private void Attack(Vector3 startPoint, GameObject prefab, float yRotationOffset, float t, int EffectNumber,int EffLota,int SENumber,float SEDelayTime)
    {
        startPoint.y = 50;
        Ray ray = new Ray(startPoint, Vector3.down);
        RaycastHit hit;
        GameObject spawnPrefab = default;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
        {
            // 地面の傾きに合わせた回転
            Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Y軸方向の追加回転を合成
            Quaternion extraRotation = Quaternion.AngleAxis(yRotationOffset, Vector3.up);

            // 合成した回転を使用
            Quaternion finalRotation = baseRotation * extraRotation;

            spawnPrefab = Instantiate(prefab, hit.point, finalRotation);
            if (SE[SENumber] != null)
            {
                spawnPrefab.GetComponent<AOE>().SE = SE[SENumber];
                spawnPrefab.GetComponent<AOE>().SEdelay = SEDelayTime;
            }
            if (Effect[EffectNumber] != null)
            {
                spawnPrefab.GetComponent<AOE>().EffLota = EffLota;
                spawnPrefab.GetComponent<AOE>().Efftime = Effect[EffectNumber].EffEarlyTime;
                spawnPrefab.GetComponent<AOE>().Eff = Effect[EffectNumber].EffPrefab;
            }
            spawnPrefab.GetComponent<AOE>().time = t;
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
        Canon[] canonSc = new Canon[2];
        for(int i = 0; i < 2; i++)
        {
            canonSc[i] = Canon[i].GetComponent<Canon>();
            canonSc[i].ObjectFalse();
        }
    }
    private void CanonOn()
    {
        Canon[] canonSc = new Canon[2];
        for (int i = 0; i < 2; i++)
        {
            canonSc[i] = Canon[i].GetComponent<Canon>();
            canonSc[i].ObjectTrue();
        }
    }
    private IEnumerator Cast(string cast,float time)
    {
        text.text = cast;
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            slider.value = t / time;
            yield return null;
        }
        slider.value = 0f;
        text.text = "のん";

    }
    private void StopAllAttackCoroutines()
    {
        // 親コルーチンを止める
        if (currentCombo != null)
        {
            StopCoroutine(currentCombo);
            currentCombo = null;
        }

        // 子コルーチンを全部止める
        foreach (var c in runcoro)
        {
            if (c != null)
                StopCoroutine(c);
        }

        // リストをクリア
        runcoro.Clear();
    }
    public void BossAttackAllReset()
    {
        StopAllAttackCoroutines();
        for (int i = 0; i < 16; i++)
        {
            fi[i].fiSc.ObjectReStealth();
        }
        for (int i = 0; i < 16; i++)
        {
            fi[i].fiSc.ObjectTrue();
            fi[i].fiOn = true;
        }
        for (int i = 0; i < 16; i++)
        {
            if (Effi[i].fiOn == true)
            {
                Effi[i].fiSc.ObjectFalse();
                Effi[i].fiOn = false;
            }
        }
        CanonOn();
        foreach (var obj in objList)
        {
            if (obj != null)
                Destroy(obj);
        }
        objList.Clear();
        ReField();
    }
}
