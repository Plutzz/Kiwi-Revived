using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponDisplay : MonoBehaviour
{
    public Image currentWeaponImage;
    public Sprite weaponImage1;
    public Sprite weaponImage2;
    public Sprite weaponImage3;
    public TextMeshProUGUI tm;

    private void Start()
    {
        SetWeaponImage(weaponImage2, Color.blue, "Water");
    }

    public void ChangeWeaponImage(int weaponNumber)
    {
        switch (weaponNumber)
        {
            case 0:
                SetWeaponImage(weaponImage1, Color.red, "Fire");
                break;
            case 1:
                SetWeaponImage(weaponImage2, Color.blue, "Water");
                break;
            case 2:
                SetWeaponImage(weaponImage3, Color.green, "Poison");
                break;
        }
    }

    private void SetWeaponImage(Sprite weaponImage, Color color, string weaponName = "", bool preserveAspect = true)
    {
        currentWeaponImage.sprite = weaponImage;
        currentWeaponImage.color = color;
        currentWeaponImage.preserveAspect = true;
        tm.text = weaponName;
    }


}
