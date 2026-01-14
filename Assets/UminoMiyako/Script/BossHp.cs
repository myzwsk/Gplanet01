using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    public int maxHp = 3000;
    public int Hp = 0;
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hp = maxHp;
        slider.minValue = 0;
        slider.maxValue = maxHp;
        slider.value = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Hp;
    }
    public void Damage()
    {
        Hp -=1;
        Debug.Log("ボスにダメージ");
    }
}
