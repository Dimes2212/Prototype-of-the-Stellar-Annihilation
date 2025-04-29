using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRGun : MonoBehaviour
{
    [Header("Gun Settings")]
    public int maxAmmo = 10;
    public float fireRate = 0.3f;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("FX")]
    public ParticleSystem muzzleFlash;
    public AudioSource shotSound;

    [Header("Input")]
    public InputActionProperty shootAction;
    public InputActionProperty reloadAction;

    private int currentAmmo;
    private float nextFireTime = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame() && Time.time >= nextFireTime)
        {
            Shoot();
        }

        if (reloadAction.action.WasPressedThisFrame())
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0) return;

        nextFireTime = Time.time + fireRate;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (muzzleFlash) muzzleFlash.Play();
        if (shotSound) shotSound.Play();

        currentAmmo--;
        Debug.Log("Ammo: " + currentAmmo);
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        Debug.Log("Reloaded!");
    }
}
