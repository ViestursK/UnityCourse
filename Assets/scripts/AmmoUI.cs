using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    private TextMeshProUGUI ammoText;

    public GunController gunController;

    void Start()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Update the displayed ammo count
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        // Check if GunController reference is valid
        if (gunController != null)
        {
            // Display "Reloading" if the gun is currently reloading
            if (gunController.IsReloading())
            {
                ammoText.text = "Reloading...";
            }
            else
            {
                // Display the current ammo in the magazine and total ammo count
                ammoText.text = "Mag: " + gunController.GetCurrentAmmoInMag() + " / " + gunController.GetCurrentAmmo();
            }
        }
    }
}
