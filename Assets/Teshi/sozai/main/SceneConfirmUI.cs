using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConfirmUI : MonoBehaviour
{
    public GameObject panel;
    public string nextSceneName;

    public void Open()
    {
        panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnYes()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnNo()
    {
        panel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
