using TMPro;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    public int maxHp = 3000;
    public int Hp = 0;
    public TextMeshProUGUI text;
    public Slider slider;
    public State state;
    public enum State
    {
        normal,
        gear2,
        gear3,
        gear4
    }
    public string[] 
        name;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hp = maxHp;
        slider.minValue = 0;
        slider.maxValue = maxHp;
        slider.value = Hp;
        state= State.normal;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Hp;
        CheckStateByHp();
    }
    public void Damage()
    {
        Hp -=1;
        Debug.Log("ボスにダメージ");
    }
    void CheckStateByHp()
    {
        float rate = (float)Hp / maxHp;

        if (rate <= 0.25f)
        {
            state = State.gear4;
            text.text = name[3];
        }
        else if (rate <= 0.5f)
        {
            state = State.gear3;
            text.text = name[2];
        }
        else if (rate <= 0.75f)
        {
            state = State.gear2;
            text.text = name[1];
        }
        else
        {
            state = State.normal;
            text.text = name[0];
        }
    }


   
}
