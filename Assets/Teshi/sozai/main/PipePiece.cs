using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PipePiece : MonoBehaviour, IPointerClickHandler
{
    public int correctRotation;

    RectTransform rect;
    int currentRotation;
    Image img;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        int[] angles = { 0, 90, 180, 270 };
        currentRotation = angles[Random.Range(0, angles.Length)];
        rect.localEulerAngles = new Vector3(0, 0, currentRotation);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentRotation = (currentRotation + 90) % 360;
        rect.localEulerAngles = new Vector3(0, 0, currentRotation);
    }
    public void RandomizeRotation()
    {
        int[] angles = { 0, 90, 180, 270 };
        currentRotation = angles[Random.Range(0, angles.Length)];
        rect.localEulerAngles = new Vector3(0, 0, currentRotation);
    }


    public bool IsCorrect()
    {
        return currentRotation == correctRotation;
    }

    void Update()
    {
        img.color = IsCorrect() ? Color.yellow : Color.white;
    }
}
