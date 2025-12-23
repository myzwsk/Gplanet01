using UnityEngine;

public class Text : MonoBehaviour
{
    [SerializeField] private GameObject uiObj1;//表示させたいUIobj
    [SerializeField] private GameObject uiObj2;//表示させたいUIobj

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(uiObj1 !=null)uiObj1.SetActive(false);
        if (uiObj2 != null) uiObj2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)//エリアにはいったら表示
    {
        if (other.CompareTag("Player"))
        {
            uiObj1.SetActive(true);
            uiObj2.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)//でたら非表示
    {
        if (other.CompareTag("Player"))
        {
            uiObj1.SetActive(false);
            uiObj2.SetActive(false);
        }
    }
}
