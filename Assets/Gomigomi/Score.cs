using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int GetScore=100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("!!!!!!!");
        if (other.CompareTag("Player"))
        {
            
            ScoreManager.Score+=GetScore;
            //Debug.Log("スコアゲット"+ ScoreManager.Score);
        }
    }
}
