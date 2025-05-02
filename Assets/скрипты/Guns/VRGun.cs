using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRGun : MonoBehaviour
{
    [Header("Gun Settings")]
    public int maxAmmo = 10;  // Максимальное количество патронов
    public float fireRate = 0.3f;  // Задержка между выстрелами
    public Transform firePoint;  // Точка, из которой будет вылетать пуля
    public GameObject bulletPrefab;  // Префаб пули

    [Header("FX")]
    public ParticleSystem muzzleFlash;  // Вспышка на дуге
    public AudioSource shotSound;  // Звук выстрела

    [Header("Input")]
    public InputActionProperty shootAction;  // Действие для стрельбы
    public InputActionProperty reloadAction;  // Действие для перезарядки

    private int currentAmmo;  // Текущее количество патронов
    private float nextFireTime = 0f;  // Время следующего выстрела

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        shootAction.action.Enable();
        reloadAction.action.Enable();
    }

    void OnDisable()
    {
        shootAction.action.Disable();
        reloadAction.action.Disable();
    }

    void Update()
    {
        // Проверка нажатия кнопки стрельбы
        if (shootAction.action.WasPressedThisFrame())
        {
            Debug.Log("Shoot input detected");

            if (Time.time >= nextFireTime)
            {
                Shoot();
            }
        }

        // Проверка нажатия кнопки перезарядки
        if (reloadAction.action.WasPressedThisFrame())
        {
            Debug.Log("Reload input detected");
            Reload();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0) return;

        nextFireTime = Time.time + fireRate;

        // Инстанцируем пулю на точке firePoint с её ориентацией
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Проверяем, есть ли Rigidbody у пули для применения физики
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = firePoint.forward * 20f;  // Устанавливаем скорость пули
        }

        // Включаем визуальные и звуковые эффекты
        if (muzzleFlash) muzzleFlash.Play();
        if (shotSound) shotSound.Play();

        // Уменьшаем количество патронов
        currentAmmo--;
        Debug.Log("Ammo: " + currentAmmo);
    }

    void Reload()
    {
        // Перезаряжаем пистолет
        currentAmmo = maxAmmo;
        Debug.Log("Reloaded!");
    }
}
