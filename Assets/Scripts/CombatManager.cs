using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Threading;
using UnityEngine.SceneManagement;
using Assets.Scritps;

public class CombatManager : MonoBehaviour {

    public double TICK_TIME = 0.25;

    public UpgradeMenuBehaviorScript characterStatUpgrades;

    public DungeonSceneGenerationScript dungeonUI;

    public FriendlyCharacterBehavior playerCharacter;
    private List<EnemyBehavior> EnemyCharacters = new List<EnemyBehavior>();

    public GameEvent combatManagerInitFinished;
    public int gold = 50000;

    public static CombatManager managerRef;


    
    public GameObject friendlyPrefab;

    public GameObject CameraRef;

    public GameObject cameraObj;

    private DateTime NextTick;


    public EnemyBehavior targetEnemy;

    System.Random rand = new System.Random();



    public int currencyToLevelUp = 100;

    public DPSClass dps;


    enum ASCENSION_LEVEL
    {
        F = 0,
        E,
        D,
        C,
        B,
        A,
        S,
        SS,
        SSS
    }


    public void updateDPS(int d)
    {
        dps.addDamage(d);
    }

    public bool canAscend()
    {
        return gold >= currencyToLevelUp;
    }

    public float getPowerBarPercent()
    {
        return gold / (float)currencyToLevelUp;
    }

    // Use this for initialization
    void Start () {
        managerRef = this;
        cameraObj = CameraRef;
        dps = new DPSClass();
        currencyToLevelUp = 100;
        Application.targetFrameRate = -1;
        NextTick = DateTime.Now.AddSeconds(1);

        instatiateFriendly(friendlyPrefab, 0, "char1");

        combatManagerInitFinished.TriggerEvent();
       

    }
	

    public double getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType statType)
    {
        double baseStat = 0;
        FriendlyCharacterBehavior c = (FriendlyCharacterBehavior)playerCharacter;
        if (statType == UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER)
        {
            baseStat = Globals.getBaseDamage();
        }
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED) baseStat = c.primaryAttackSpeed;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) baseStat = c.clickDamage;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) baseStat = c.moveSpeed;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.PASSIVE_DAMAGE) baseStat = c.passiveDamage;

        baseStat += characterStatUpgrades.getUpgradeLinearIncrease(statType);
        baseStat *= characterStatUpgrades.getUpgradeMultiplier(statType);

        return baseStat;
    }



    public void clearCharacaterLists()
    {
        EnemyCharacters.Clear();
    }





    public EnemyBehavior getTargetEnemy()
    {
        return targetEnemy;
    }

    public void instatiateFriendly(GameObject friendlyPrefab, int position, string name)
    {
        if (friendlyPrefab == null)
        {
            Debug.Log("instatiateFriendly Error: empty prefab");
            return;
        }

        GameObject clone = Instantiate(friendlyPrefab,
            new Vector3(Camera.main.transform.position.x, -700, 0), 
           Quaternion.identity);
        clone.name = name;
        playerCharacter =  clone.GetComponent<FriendlyCharacterBehavior>();
    }



    public void applyXP(int xp)
    {
        playerCharacter.applyXP(xp);
        gold += xp;
    }

    public void registerEnemy(EnemyBehavior e)
    {
        targetEnemy = e;
    }


    void OnMouseDown()
    {
        int d = (int)getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE);
        if (EnemyCharacters.Count == 0) return;
        EnemyCharacters[0].GetComponent<EnemyBehavior>().takeDamage(d);
        updateDPS(d);
        Debug.Log("screen click");
    }
}
