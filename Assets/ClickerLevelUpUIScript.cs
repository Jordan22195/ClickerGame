using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickerLevelUpUIScript : LevelUpUIScript
{
    public override void Start()
    {
        CombatManager.currentClicker = this;
    }
}

