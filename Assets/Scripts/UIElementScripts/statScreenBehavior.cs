using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statScreenBehavior : MonoBehaviour
{

    public GameObject nameText;
    public GameObject xpText;
    public GameObject levelText;
    public GameObject HPText;
    public GameObject attackText;
    public GameObject defenseText;
    public GameObject statPointsText;

    public GameObject confirmButton;
    public GameObject resetButton;

    private CharacterStats statsRef;

    bool previewMode = false;

    private int tempHP;
    private int tempCurrentHP;
    private int tempAtt;
    private float tempDef;
    private int tempPts;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (previewMode)
        {
            previewStats();
        }
        else if (previewMode == false)
        {
            refreshStats();
        }
    }

    private void OnEnable()
    {
        disablePreviewMode();
    }

    void setGameObjectText(GameObject obj, string newtext)
    {
        if(obj.GetComponent<Text>()!= null)
        {
            obj.GetComponent<Text>().text = newtext.ToString();
        }
    }

    private void OnMouseDown()
    {
    //    this.gameObject.SetActive(false);
    }

    public void refreshStats()
    {
        if (statsRef!=null)
        {
            loadStats(statsRef);
        }
    }

    public void updateXPtext()
    {
        setGameObjectText(xpText, "XP: " + (statsRef.xp - statsRef.xpForCurrentLevel) +
" / " + (statsRef.xpForNextLevel-statsRef.xpForCurrentLevel)
+ " (" + (int)((float)(statsRef.xp - statsRef.xpForCurrentLevel) / 
(float)(statsRef.xpForNextLevel-statsRef.xpForCurrentLevel) * 100)
+ "%)");
    }

    public void loadStats(CharacterStats stats)
    {
        if (stats == null)
        {
            return;
        }
        statsRef = stats;

        if (!this.gameObject.activeSelf) return;

        setGameObjectText(nameText, stats.name);
        updateXPtext();
        setGameObjectText(levelText, "Level: " + stats.level);
        setGameObjectText(HPText, "HP: " + stats.currentHP +" / "+stats.maxHP);
        setGameObjectText(attackText, "Attack: " + stats.attack);
        setGameObjectText(defenseText, "attacks per second: " + tempDef);
        setGameObjectText(statPointsText, "Stat Points: " +stats.statPoints);
    }

    public void previewStats()
    {

        updateXPtext();
        setGameObjectText(nameText, statsRef.name);
        setGameObjectText(levelText, "Level: " + statsRef.level);
        setGameObjectText(HPText, "HP: " + statsRef.currentHP + " / " + tempHP);
        setGameObjectText(attackText, "Attack: " + tempAtt);
        setGameObjectText(defenseText, "attacks per second: " + tempDef);
        setGameObjectText(statPointsText, "Stat Points: " + tempPts);
    }

    public void onPlusHPButtonClick()
    {
        
        enablePreviewMode();
        if (tempPts > 0)
        {
            tempHP += 10;
            tempPts--;
        }
        onConfirmButtonClick();

    }

    public void onPlusAttackButtonClick()
    {
        enablePreviewMode();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (tempPts > 0)
            {
                tempAtt += tempPts;
                tempPts = 0;
            }
        }
         
        else if (tempPts > 0)
        {
            tempAtt += 1;
            tempPts--;
        }
        onConfirmButtonClick();

    }

    public void onPlusDefenseButtonClick()
    {

        enablePreviewMode();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (tempPts > 0)
            {
                if (tempDef == 0)
                {
                    tempDef = 1;
                    tempPts--;
                }
                
                
                tempDef += (0.01f * (float)tempPts);
                tempPts = 0;
            }
        }
        else if (tempPts > 0)
        {
            if (tempDef == 0)
                tempDef = 1;
            else
            {
                tempDef += 0.01f;
            }
            tempPts -= 1;
        }
        onConfirmButtonClick();

    }

    public void enablePreviewMode()
    {
        if (previewMode == false && statsRef.statPoints > 0)
        {
            //confirmButton.SetActive(true);
            //resetButton.SetActive(true);
            previewMode = true;
            tempHP = statsRef.maxHP;
            tempAtt = statsRef.attack;
            tempDef = statsRef.defense;
            tempPts = statsRef.statPoints;
        }
    }

    public void disablePreviewMode()
    {
        previewMode = false;
        confirmButton.SetActive(false);
        resetButton.SetActive(false);
    }

    public void onConfirmButtonClick()
    {
        if (previewMode)
        {
            statsRef.maxHP = tempHP;
            statsRef.attack = tempAtt;
            statsRef.defense = tempDef;
            statsRef.statPoints = tempPts;
            disablePreviewMode();
        }
    }

    public void onResetButtonClick()
    {
        disablePreviewMode();
    }
}
