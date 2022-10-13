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
    private static List<CharacterBehavior> FriendlyCharacters = new List<CharacterBehavior>();
    private static List<CharacterBehavior> EnemyCharacters = new List<CharacterBehavior>();

    public static int gold = 10000;

    private List<GameObject> enemyPrefabClones = new List<GameObject>();

    public List<GameObject> FriendlyPositions;
    public List<GameObject> EnemeyPositions;

    public GameObject EnemyPrefab;
    public GameObject friendlyPrefab;

    public static int currentLevel = 1;

    private CharacterBehavior nextChar;
    private DateTime NextTick;
    private int characterTurnIndex = 0;

    public UnityEvent EndDungeonEvent;
    public UnityEvent DungeonWinEvent;

    private int endDungeonTickCounter = 0;
    private int nextDungeonTickCounter = 0;
    System.Random rand = new System.Random();

    public enum DungeonState { STARTING, COMBAT, RUNNING, EXITING, WIN, OTHER }
    public static DungeonState currentDungeonState;


    public static int damageToLevelUp;

    public static DPSClass dps;

    static float highestDps = 0;

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
        return highestDps >= damageToLevelUp;
    }

    public static float getPowerBarPercent()
    {
        float damage =  dps.getDPS();
        if (damage > highestDps) highestDps = damage;
        return damage / (float)damageToLevelUp;
    }

    // Use this for initialization
    void Start () {
        dps = new DPSClass();
        damageToLevelUp = 100;
        Application.targetFrameRate = -1;
        NextTick = DateTime.Now.AddSeconds(1);
        currentLevel = 1;
        currentDungeonState = DungeonState.STARTING;

        if(party.char1 != null)
        {
           instatiateFriendly(friendlyPrefab, 0, "char1");
        }
        if (party.char2 != null)
        {
            instatiateFriendly(friendlyPrefab, 0, "char2");
        }
        if (party.char3 != null)
        {
            instatiateFriendly(friendlyPrefab, 0, "char3");
        }
        if (party.char4 != null)
        {
            instatiateFriendly(friendlyPrefab, 0, "char4");
        }

    }
	

	// Update is called once per frame
	void Update ()
    {
        Debug.Log("DPS: " + dps.getDPS().ToString());
        // 0.5 second game tick
        if (DateTime.Compare(DateTime.Now, NextTick) >= 0)
        {
            //NextTick = DateTime.Now.AddSeconds(TICK_TIME);

            CombatStateMachine();
        }
        else
        {

        }



    }

    public static double getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType statType)
    {
        double baseStat = 0;
        FriendlyCharacterBehavior c = (FriendlyCharacterBehavior)FriendlyCharacters[0];
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


    public void friendlyAttack()
    {
        getNextCharacter();
        foreach (CharacterBehavior c in FriendlyCharacters)
        {
            c.setStatusText("idle");
            c.gameObject.GetComponent<CharacterBehavior>().attack();
        }
    }

    double nextAttack = 0;
    void CombatStateMachine()
    {


        if (currentDungeonState == DungeonState.STARTING)
        {
 
            Debug.Log("start level");
            startDungeonLevel();
            currentDungeonState = DungeonState.RUNNING;

        }
        if (currentDungeonState == DungeonState.RUNNING)
        {
            //Debug.Log("running");
            nextAttack = Time.time + (1/getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED)); 
        }
        else if (currentDungeonState == DungeonState.COMBAT)
        {
            getNextCharacter();
            foreach (CharacterBehavior c in Characters)
            {
                c.setStatusText("idle");

            }
            foreach (CharacterBehavior c in FriendlyCharacters)
            {
               

            }
            if(Time.time >= nextAttack)
            {
                nextAttack = Time.time + (1/ getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED)); 
                friendlyAttack();
            }
                //perform action
                if (nextChar != null)
            {
                nextChar.gameObject.GetComponent<CharacterBehavior>().performAction();
            }
            //all characters are dead, go to end dungeon state
            if (FriendlyCharacters.Count == 0)
            {
                currentDungeonState = DungeonState.EXITING;
            }
            if (EnemyCharacters.Count == 0)
            {
                NextTick = DateTime.Now.AddSeconds(TICK_TIME);
                currentDungeonState = DungeonState.WIN;
            }
            //all enemies are dead, go to next level
        }
        else if (currentDungeonState == DungeonState.EXITING)
        {
            if (endDungeonTickCounter == 0) //make sure invoke only happens once
            {
                EndDungeonEvent.Invoke();
                clearCharacaterLists();
            }
            if (endDungeonTickCounter >= 3) //wait a few seconds before leaving the scene
            {
                OpenTownScene();

            }
            endDungeonTickCounter++;
        }
        else if (currentDungeonState == DungeonState.WIN)
        {
            if (cleanupEnemyPrefabs())
            {
                currentDungeonState = DungeonState.STARTING;
                NextTick = DateTime.Now.AddSeconds(TICK_TIME);
            }
        }

    }

    public void simulation(int numSeconds)
    {
        int numTicks = (int)((1.0 / TICK_TIME) * (double)numSeconds);
        for (int ticks = 0; ticks < numTicks; ticks++)
        {
            CombatStateMachine();
        }
    }

    void getNextCharacter()
    {
        if (Characters.Count == 0)
            return; 

        if (characterTurnIndex >= Characters.Count-1)
        {
            characterTurnIndex = 0;
        }
        else
        {
            characterTurnIndex++;
        }
        nextChar = Characters[characterTurnIndex];
        //Debug.Log("next character: " + nextChar.name);


    }

    public static void clearCharacaterLists()
    {
        EnemyCharacters.Clear();
        Characters.Clear();
        FriendlyCharacters.Clear();
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
            FriendlyCharacters.Add(character);
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
            FriendlyCharacters.Remove(character);
        }
        else
        {
            EnemyCharacters.Remove(character);
        }



    }

    public static CharacterBehavior GetWeakestEnemy(CharacterBehavior caller)
    {
        List<CharacterBehavior> enemychar;
        if (caller.CharacterType == CharacterBehavior.chartype.FRIENDLY)
        {
            enemychar = EnemyCharacters;
        }
        else
        {
            enemychar = FriendlyCharacters;
        }

        int lowestHP = int.MaxValue;
        CharacterBehavior returnChar = null;
        foreach (CharacterBehavior c in enemychar)
        {
            if ( c.stats.currentHP < lowestHP)
            {
                returnChar = c;
            }
        }
        return returnChar;
    }

    public static CharacterBehavior GetWeakestFriedly(CharacterBehavior caller)
    {
        List<CharacterBehavior> friendlychars;
        if (caller.CharacterType == CharacterBehavior.chartype.FRIENDLY)
        {
            friendlychars = FriendlyCharacters;
        }
        else
        {
            friendlychars = EnemyCharacters;
        }

        int lowestHP = int.MaxValue;
        CharacterBehavior returnChar = null;
        foreach (CharacterBehavior c in EnemyCharacters)
        {
            if (c.stats.currentHP < lowestHP)
            {
                returnChar = c;
            }
        }
        return returnChar;
    }

    public void OpenTownScene()
    {
        SceneManager.LoadScene("town", LoadSceneMode.Single);
    }

    public bool cleanupEnemyPrefabs()
    {
        if(nextDungeonTickCounter == 2)
        {
            DungeonWinEvent.Invoke();
            foreach (GameObject e in enemyPrefabClones)
            {
                Destroy(e);
            }
            if(DungeonUIBehaviorScript.autoAdvanceLevel)
            {
            }
            currentLevel++;

            nextDungeonTickCounter = 0;
            return true;
        }



        nextDungeonTickCounter++;
        return false;
    }

    public void instatiateFriendly(GameObject friendlyPrefab, int position, string name)
    {
        if (friendlyPrefab == null)
        {
            Debug.Log("instatiateFriendly Error: empty prefab");
            return;
        }
        if (position < 0 || position > FriendlyPositions.Count - 1)
        {
            Debug.Log("instatiateFriendly Error: bad position index");
            return;
        }
        if (FriendlyPositions[position] == null)
        {
            Debug.Log("instatiateFriendly Error: null friendly position");
            return;
        }
       GameObject clone = Instantiate(friendlyPrefab, 
           FriendlyPositions[position].transform.position, 
           Quaternion.identity);
        clone.name = name;
    }

    public void instantiateEnemy(GameObject enemyPrefab, int position, int level)
    {
        if(enemyPrefab == null)
        {
            Debug.Log("instantiateEnemy Error: empty prefab");
            return;
        }
        if (position < 0 || position > EnemeyPositions.Count -1)
        {
            Debug.Log("instantiateEnemy Error: bad position index");
            return;
        }
        if (EnemeyPositions[position] == null)
        {
            Debug.Log("instantiateEnemy Error: null enemy position");
            return;
        }
        GameObject clone = Instantiate(enemyPrefab, EnemeyPositions[position].transform.position, Quaternion.identity);
        clone.GetComponent<CharacterBehavior>().stats.setLevelandStats(level);
        enemyPrefabClones.Add(clone);

    }

    public void startDungeonLevel()
    {
        characterTurnIndex = 0;
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
        totalSpawnLevel = 1000;
        int maxSpawn = 1;
        if(totalSpawnLevel < maxSpawn)
        {
            maxSpawn = totalSpawnLevel;
        }
        //inclusive min, non inclusive max
        int spawnCount = rand.Next(1, maxSpawn+1);
        int averageLevel = totalSpawnLevel / spawnCount;
        for (int i = 0; i < spawnCount; i++)
        {
            instantiateEnemy(EnemyPrefab, i, averageLevel);
        }


    }

    public static void applyXP(int xp)
    {
        foreach(CharacterBehavior c in FriendlyCharacters)
        {
            c.applyXP(xp);
        }
        gold += xp;
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
