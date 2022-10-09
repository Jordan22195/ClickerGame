using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonUIBehaviorScript : MonoBehaviour {

    public GameObject levelTextGameObject;
    public GameObject autoAdvanceToggle;
    public TextMeshProUGUI goldText;
    public GameObject backgroundA;
    public GameObject backgroundB;
    public static bool autoAdvanceLevel = false;

    public static float pixelsPerMeter = 500f;

    private Text levelText;

    Vector3 inFramePos;
    Vector3 outOfFramePos;

    // Use this for initialization
    void Start () {

        levelText = levelTextGameObject.GetComponent<Text>();
        inFramePos = backgroundA.transform.position;
        outOfFramePos = backgroundB.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        incrimentLevelText();
        goldText.text = "Gold: " + CombatManager.gold.ToString();

        if(CombatManager.currentDungeonState == CombatManager.DungeonState.RUNNING )
        {
            //Debug.Log("run");
            Vector3 movement = new Vector3(-1f, 0f, 0f);
            backgroundA.transform.position += movement * Time.deltaTime * (float)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) * pixelsPerMeter;
            backgroundB.transform.position += movement * Time.deltaTime * (float)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) * pixelsPerMeter;
        }

        if(backgroundA.transform.position.x < inFramePos.x && !backgroundA.GetComponent<SpriteRenderer>().isVisible)
        {
            backgroundA.transform.position = outOfFramePos;
        }
        if (backgroundB.transform.position.x < inFramePos.x && !backgroundB.GetComponent<SpriteRenderer>().isVisible)
        {
            backgroundB.transform.position = outOfFramePos;
        }

    }

    public void incrimentLevelText()
    {
        levelText.text = "Level: " + CombatManager.currentLevel.ToString();
    }

    public void onAutoAdvanceToggle()
    {
        autoAdvanceLevel = autoAdvanceToggle.GetComponent<Toggle>().isOn;
    }
}
