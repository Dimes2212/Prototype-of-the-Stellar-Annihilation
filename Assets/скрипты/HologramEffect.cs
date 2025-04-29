using UnityEngine;

public class HologramEffect : MonoBehaviour
{
    public Material hologramMaterial; // Материал с твоим шейдером

    void Update()
    {
        // Передаем значение времени в шейдер
        hologramMaterial.SetFloat("_CustomTime", Time.time);
    }
}
