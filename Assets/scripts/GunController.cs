using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int magSize = 30;
    public int maxAmmo = 300;
    public float reloadTime = 1.5f;

    [Header("Weapon Sway")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothness = 2.0f;

    [Header("Kickback")]
    public float kickbackForce = 0.1f;

    [Header("Camera Shake")]
    public float shakeDuration = 0.1f;
    public float shakeIntensity = 0.1f;

    bool canShoot;
    bool isReloading; 
    int currentAmmoInMag;
    int currentAmmo;

    //ADS
    public Vector3 hipPosition;
    public Vector3 adsPosition;
    public float adsSpeed = 9f;

    //muzzle flash
    public UnityEngine.UI.Image muzzleFlashImage;
    public Sprite[] muzzleFlashSprites;

    private Vector3 initialPosition;

    void Start()
    {
        currentAmmo = maxAmmo;
        currentAmmoInMag = magSize;
        canShoot = true;
        muzzleFlashImage.color = Color.clear;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        DetermineAim();

        if (Input.GetMouseButton(0) && canShoot && currentAmmoInMag > 0 && !isReloading)
        {
            canShoot = false;
            currentAmmoInMag--;
            StartCoroutine(Shoot());
            StartCoroutine(MuzzleFlash());
            ApplyKickback();
            StartCoroutine(CameraShake());
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmoInMag < magSize && currentAmmo > 0)
        {
            StartCoroutine(Reload());
        }

        ApplyWeaponSway();
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlashImage.sprite = muzzleFlashSprites[UnityEngine.Random.Range(0, muzzleFlashSprites.Length)];
        muzzleFlashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.color = Color.clear;
        muzzleFlashImage.sprite = null;
    }

    IEnumerator Reload()
    {
        isReloading = true; 

        // Lower the gun for reload
        Vector3 startReloadPosition = transform.localPosition;
        Vector3 endReloadPosition = new Vector3(startReloadPosition.x, startReloadPosition.y - 0.2f, startReloadPosition.z);

        float t = 0;
        while (t < reloadTime)
        {
            t += Time.deltaTime;
            float fraction = t / reloadTime;
            transform.localPosition = Vector3.Lerp(startReloadPosition, endReloadPosition, fraction);
            yield return null;
        }

        int roundsToReload = magSize - currentAmmoInMag;
        if (currentAmmo >= roundsToReload)
        {
            currentAmmoInMag += roundsToReload;
            currentAmmo -= roundsToReload;
        }
        else
        {
            currentAmmoInMag += currentAmmo;
            currentAmmo = 0;
        }
        isReloading = false; 
    }

    void ApplyKickback()
    {
        transform.localPosition -= Vector3.forward * kickbackForce;
    }

    void ApplyWeaponSway()
    {
        float movementX = -Input.GetAxis("Mouse X") * swayAmount;
        float movementY = -Input.GetAxis("Mouse Y") * swayAmount;

        movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
        movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + finalPosition, Time.deltaTime * swaySmoothness);
    }

    IEnumerator CameraShake()
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * Mathf.Clamp01(shakeIntensity);
            float y = UnityEngine.Random.Range(-1f, 1f) * Mathf.Clamp01(shakeIntensity);

            Camera.main.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPosition;
    }


    void DetermineAim()
    {
        Vector3 target = Input.GetMouseButton(1) ? adsPosition : hipPosition;

        // interpolate between current position and target position
        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * adsSpeed);
        transform.localPosition = desiredPosition;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public int GetCurrentAmmoInMag()
    {
        return currentAmmoInMag;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
