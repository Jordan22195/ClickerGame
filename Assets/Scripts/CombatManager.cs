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

    public static UpgradeMenuBehaviorScript characterStatUpgrades;

    private static List<CharacterBehavior> Characters = new List<CharacterBehavior>();
    private static CharacterBehavior playerCharacter;
    private static List<CharacterBehavior> EnemyCharacters = new List<CharacterBehavior>();

    public GameEvent combatManagerInitFinished;

    public static int gold = 500;

    private List<GameObject> enemyPrefabClones = new List<GameObject>();

    public GameObject EnemyPrefab;
    public GameObject friendlyPrefab;

    public GameObject CameraRef;

    public static GameObject cameraObj;

    public static int currentLevel = 1;

    private CharacterBehavior nextChar;
    private DateTime NextTick;

    public UnityEvent EndDungeonEvent;
    public UnityEvent DungeonWinEvent;

    public static CharacterBehavior targetEnemy;

    System.Random rand = new System.Random();



    public static int currencyToLevelUp = 100;

    public static DPSClass dps;


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


    public static void updateDPS(int d)
    {
        dps.addDamage(d);
    }

    public static bool canAscend()
    {
        return gold >= currencyToLevelUp;
    }

    public static float getPowerBarPercent()
    {
        return gold / (float)currencyToLevelUp;
    }

    // Use this for initialization
    void Start () {
        cameraObj = CameraRef;
        dps = new DPSClass();
        currencyToLevelUp = 100;
        Application.targetFrameRate = -1;
        NextTick = DateTime.Now.AddSeconds(1);
        currentLevel = 1;

        instatiateFriendly(friendlyPrefab, 0, "char1");

        combatManagerInitFinished.TriggerEvent();
       

    }
	

	// Update is called once per frame
	void Update ()
    {
        updateSpawns();

    }

    public static double getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType statType)
    {
        double baseStat = 0;
        FriendlyCharacterBehavior c = (FriendlyCharacterBehavior)playerCharacter;
        if (statType == UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER)
        {
            baseStat = Globals.getBaseDamage();
        }
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED) baseStat = c.attackSpeed;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) baseStat = c.clickDamage;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) baseStat = c.moveSpeed;
        else if (statType == UpgradeButtonBehaviorScript.EnumBonusType.PASSIVE_DAMAGE) baseStat = c.passiveDamage;

        baseStat += characterStatUpgrades.getUpgradeLinearIncrease(statType);
        baseStat *= characterStatUpgrades.getUpgradeMultiplier(statType);

        return baseStat;
    }



    public static void clearCharacaterLists()
    {
        EnemyCharacters.Clear();
        Characters.Clear();
    }

    public List<CharacterBehavior> getEnemyList()
    {
        return EnemyCharacters;
    }

    public static void RegisterCharacter(CharacterBehavior character)
    {
        Characters.Add(character);
        if (character.CharacterType == CharacterBehavior.chartype.FRIENDLY)
        {
            playerCharacter = character;
        }
        else
        {
            EnemyCharacters.Add(character);
        }
        Characters.Sort(new CharacterStatsSpeedCompare());
    }

    public static void DeRegisterCharacter(CharacterBehavior character)
    {
        Characters.Remove(character);

        if (character.CharacterType == CharacterBehavior.chartype.FRIENDLY)
        {
        }
        else
        {
            EnemyCharacters.Remove(character);
        }



    }

    public static CharacterBehavior getTargetEnemy(CharacterBehavior caller)
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
            new Vector3(DungeonUIBehaviorScript.cameraStartPos.x-1000, -1383, 50), 
           Quaternion.identity);
        clone.name = name;
        playerCharacter =  clone.GetComponent<FriendlyCharacterBehavior>();
    }

    public void instantiateEnemy(GameObject enemyPrefab, int level)
    {
        if(enemyPrefab == null)
        {
            Debug.Log("instantiateEnemy Error: empty prefab");
            return;
        }
        
        Vector3 pos = playerCharacter.gameObject.transform.position;
        pos.x = DungeonUIBehaviorScript.cameraStartPos.x + (currentLevel * DungeonUIBehaviorScript.chunkSize);
        GameObject clone = Instantiate(enemyPrefab, pos, Quaternion.identity);
        clone.GetComponent<CharacterBehavior>().stats.setLevelandStats(level);
        enemyPrefabClones.Add(clone);

    }

    public void updateSpawns()
    {
        while (currentLevel < DungeonUIBehaviorScript.chunksLoaded)
        {
            int totalSpawnLevel = 1;
            if (currentLevel >= 1 && currentLevel <= 5)
            {
                totalSpawnLevel = 1;
            }
            else if (currentLevel >= 6 && currentLevel <= 10)
            {
                totalSpawnLevel = 2;
            }
            else
            {
                totalSpawnLevel = (int)Math.Pow(3.0, Math.Log(Math.Floor((double)currentLevel - 1.0) / 5, 2.0));

            }
            //totalSpawnLevel = 1000;
            int maxSpawn = 1;
            if (totalSpawnLevel < maxSpawn)
            {
                maxSpawn = totalSpawnLevel;
            }
            //inclusive min, non inclusive max
            int spawnCount = rand.Next(1, maxSpawn + 1);
            int averageLevel = totalSpawnLevel / spawnCount;
            for (int i = 0; i < spawnCount; i++)
            {
                instantiateEnemy(EnemyPrefab, averageLevel);
                currentLevel++;
            }
        }


    }

    public static void applyXP(int xp)
    {
        playerCharacter.applyXP(xp);
        gold += xp;
    }

    public static void registerEnemy(CharacterBehavior e)
    {
        targetEnemy = e;
    }


    void OnMouseDown()
    {
        int d = (int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE);
        if (EnemyCharacters.Count == 0) return;
        EnemyCharacters[0].GetComponent<EnemyBehavior>().takeDamage(d);
        updateDPS(d);
        Debug.Log("screen click");
    }
}
