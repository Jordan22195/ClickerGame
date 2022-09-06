using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TierUpgradeBehaviorScript : MonoBehaviour
{

    public int bonus = 1;
    public int cost = 100;
    public int level = 0;
    public int maxLevel = 5;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    public GameObject icon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        costText.text = "Cost: " + cost.ToString();
        levelText.text = "Lvl " + level.ToString() + "/" + maxLevel.ToString();

    }

    private void OnMouseDown()
    {
        if (CombatManager.gold >= cost && level < maxLevel)
        {
            CombatManager.gold -= cost;
            cost = (int) ((double)cost * 2.5);
            bonus *= 2;
            level++;
        }
    }
}
