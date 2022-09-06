using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelUpUIScript : MonoBehaviour
{
    public int damage = 1;
    public int levelUpCost = 1;
    public int level = 1;


    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI damageText;

    public List<GameObject> upgrades;
   

    public int getDamage()
    {
        int d = damage;
        foreach (GameObject u in upgrades)
        {
            d *= u.GetComponent<TierUpgradeBehaviorScript>().bonus;
        }
        return d;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        CombatManager.currentChar = this;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = level.ToString();
        costText.text = "Cost: " + levelUpCost.ToString();
        damageText.text = "Damage: " + getDamage().ToString();
    }

    protected virtual void OnMouseDown()
    {
        if (CombatManager.gold >= levelUpCost)
        {
            CombatManager.gold -= levelUpCost;
            levelUpCost++;
            damage++;
            level++;
        }
    }
}
