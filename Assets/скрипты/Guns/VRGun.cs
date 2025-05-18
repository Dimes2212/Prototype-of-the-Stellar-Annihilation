using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRGun : MonoBehaviour
{
    public int maxAmmo = 10;
    public float fireRate = 0.3f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    public AudioSource shotSound;
    public InputActionProperty shootAction;
    public InputActionProperty reloadAction;
    private int currentAmmo;
    private float nextFireTime = 0f;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        currentAmmo = maxAmmo;
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogWarning("XRGrabInteractable not found on gun!");
        }
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
        if (grabInteractable != null && !grabInteractable.isSelected)
            return;

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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = firePoint.forward * 20f;
        }
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
