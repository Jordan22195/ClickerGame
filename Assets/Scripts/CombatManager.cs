using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Threading;
using UnityEngine.SceneManagement;
using Assets.Scritps;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    public float TICK_TIME = 0.25f;

    public UpgradeMenuBehaviorScript characterStatUpgrades;

    public DungeonSceneGenerationScript dungeonUI;

    public FriendlyCharacterBehavior playerCharacter;
    private List<EnemyBehavior> EnemyCharacters = new List<EnemyBehavior>();

    public GameEvent combatManagerInitFinished;
    public float gold = 0;

    public static CombatManager managerRef;

    public GameObject ascendButton;

    
    public GameObject friendlyPrefab;

    public GameObject CameraRef;

    public GameObject cameraObj;



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
        return gold >= (float)Globals.getAscensionCostRequiement();
    }

    public float getPowerBarPercent()
    {
        return gold / (float)Globals.getAscensionCostRequiement() ;
    }

    private void Update()
    {
        if (canAscend())
        {
            ascendButton.SetActive(true);
        }

    }

    // Use this for initialization
    void Start () {
        managerRef = this;
        cameraObj = CameraRef;
        dps = new DPSClass();
        currencyToLevelUp = 100;
        Application.targetFrameRate = -1;

        instatiateFriendly(friendlyPrefab, 0, "char1");

        combatManagerInitFinished.TriggerEvent();
       

    }
	

    public float getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType statType)
    {
        float baseStat = 0;
        FriendlyCharacterBehavior c = (FriendlyCharacterBehavior)playerCharacter;
        if (statType == UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER)
        {
            baseStat = Globals.getBaseDamage();
        }
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED) baseStat = c.primaryAttackSpeed;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) baseStat = c.clickDamage;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) baseStat = c.moveSpeed;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.PASSIVE_DAMAGE) baseStat = c.passiveDamage;

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


    public void OpenAscensionScreen()
    {
        SceneManager.LoadScene("AscentionScreen", LoadSceneMode.Single);
    }
}
