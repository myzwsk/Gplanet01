using UnityEngine;

public class Canon : MonoBehaviour
{
 
    public void ObjectTrue()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            
        }
    }
    public void ObjectFalse()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
      
        }
    }
}
