using UnityEngine;

public class SetRenderOrder : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {
        // Установим sortingOrder для всего Canvas
        canvas.sortingOrder = 1; // Больше значение - выше по порядку
    }
}
