using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeTierBehaviorScript : MonoBehaviour
{
    [SerializeField]
    public List<UpgradeButtonBehaviorScript> upgrades;
    public UpgradeButtonBehaviorScript setBonusUpgrade;
    public int TierLevel = 1;
    bool tierComplete = false;
    bool tierCompleteUIActive = false;


    public float getUpgradeMultiplier(UpgradeButtonBehaviorScript.EnumBonusType upgradeType)
    {
        float mult = 1f;
        foreach (UpgradeButtonBehaviorScript u in upgrades)
        {
            if (u.bonusType == upgradeType)
            {
                float bonus = u.getBonus() / 100;
                mult *= (1 + bonus);
            }


        }

        if (setBonusUpgrade.bonusType == upgradeType && tierComplete)
        {
            float bonus = setBonusUpgrade.getBonus() / 100;
            mult *= (1 + bonus);
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
        checkTierCompletetion();
        if(tierComplete && !tierCompleteUIActive)
        {
            activateTierCompleteUI();
        }
    }

    void checkTierCompletetion()
    {
        bool complete = true;
        foreach (UpgradeButtonBehaviorScript u in upgrades)
        {
            if (!u.isUpgradeComplete())
            {
                complete = false;
            }
        }

        tierComplete = complete;

    }
    
    void activateTierCompleteUI()
    {
        tierCompleteUIActive = true;
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
