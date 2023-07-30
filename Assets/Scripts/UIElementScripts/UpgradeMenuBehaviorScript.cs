using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeMenuBehaviorScript : MonoBehaviour
{
    [SerializeField]
    public List<UpgradeTierBehaviorScript> tiers;


    public float getUpgradeMultiplier(UpgradeButtonBehaviorScript.EnumBonusType upgradeType)
    {
        float mult = 1f;
        foreach (UpgradeTierBehaviorScript t in tiers)
        {
            mult *= t.getUpgradeMultiplier(upgradeType);
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
        foreach (UpgradeTierBehaviorScript u in tiers)
        {
            u.save();
        }
    }

    public void load()
    {
        foreach (UpgradeTierBehaviorScript u in tiers)
        {
            u.load();
        }
    }


}
