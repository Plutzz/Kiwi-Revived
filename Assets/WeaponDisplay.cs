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
        currentWeaponImage.sprite = weaponImage2;
        currentWeaponImage.color = Color.blue;
        tm.text = "Water";
    }

    public void ChangeWeaponImage(int weaponNumber)
    {
        switch (weaponNumber)
        {
            case 0:
                currentWeaponImage.sprite = weaponImage1;
                currentWeaponImage.color =  Color.red;
                tm.text = "Fire";
                break;
            case 1:
                currentWeaponImage.sprite = weaponImage2;
                currentWeaponImage.color =  Color.blue;
                tm.text = "Water";
                break;
            case 2:
                currentWeaponImage.sprite = weaponImage3;
                currentWeaponImage.color =  Color.green;
                tm.text = "Poison";
                break;
        }
    }


}
