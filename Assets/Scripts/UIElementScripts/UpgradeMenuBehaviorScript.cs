using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeMenuBehaviorScript : MonoBehaviour
{
    [SerializeField]
    public List<UpgradeButtonBehaviorScript> upgrades;


    public float getUpgradeMultiplier(UpgradeButtonBehaviorScript.EnumBonusType upgradeType)
    {
        float mult = 1;
        foreach (UpgradeButtonBehaviorScript u in upgrades)
        {
            UpgradeButtonBehaviorScript upgrade = u.GetComponent<UpgradeButtonBehaviorScript>();
            if (upgrade.bonusType == upgradeType)
            {
                mult *= (1 + (u.GetComponent<UpgradeButtonBehaviorScript>().getBonus()/100));
            }
            
        }
        return mult;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
     }


    public void save()
    {
        foreach (UpgradeButtonBehaviorScript u in upgrades)
        {
            u.saveToFile();
        }
    }

    public void load()
    {
        foreach (UpgradeButtonBehaviorScript u in upgrades)
        {
            u.loadFromFile();
        }
    }


}
