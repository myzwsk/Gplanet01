using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int GetScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetScore = 0;

    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Score+=GetScore;
        }
    }
}
