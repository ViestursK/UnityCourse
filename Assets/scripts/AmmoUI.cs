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
        UpdateAmmoText();
    }

    // Update the displayed ammo count
    void UpdateAmmoText()
    {
        if (gunController != null)
        {
            if (gunController.IsReloading())
            {
                ammoText.text = "Reloading...";
            }
            else
            {
                ammoText.text = "Mag: " + gunController.GetCurrentAmmoInMag() + " / " + gunController.GetCurrentAmmo();
            }
        }
    }
}
