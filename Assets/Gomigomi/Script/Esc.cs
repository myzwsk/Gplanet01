using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class escape : MonoBehaviour
{
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // このオブジェクトを保持
    }
}
