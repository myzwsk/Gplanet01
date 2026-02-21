using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOb : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            if (Input.GetKey(KeyCode.P))
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    int currentIndex = SceneManager.GetActiveScene().buildIndex;
                    SceneManager.LoadScene(currentIndex + 1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SceneManager.LoadScene("TitleScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SceneManager.LoadScene("StoryScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    SceneManager.LoadScene("YamadaScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    SceneManager.LoadScene("YneuraScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    SceneManager.LoadScene("KidsRoomScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    SceneManager.LoadScene("BathRoomScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    SceneManager.LoadScene("BossScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    SceneManager.LoadScene("BossHardScene");
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    SceneManager.LoadScene("ResultScene");
                }
            }
        }
    }
}
