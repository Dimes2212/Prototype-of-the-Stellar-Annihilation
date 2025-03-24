

using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {

    }

    void Update()
    {
        var keyboard = Keyboard.current;

        // Движение вперед (W)
        if (keyboard.wKey.isPressed)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Движение назад (S)
        if (keyboard.sKey.isPressed)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        // Влево (A)
        if (keyboard.aKey.isPressed)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Вправо (D)
        if (keyboard.dKey.isPressed)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}