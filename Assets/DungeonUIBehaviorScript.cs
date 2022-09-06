using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonUIBehaviorScript : MonoBehaviour {

    public GameObject levelTextGameObject;
    public GameObject autoAdvanceToggle;
    public TextMeshProUGUI goldText;

    public static bool autoAdvanceLevel = false;

    private Text levelText;

	// Use this for initialization
	void Start () {

        levelText = levelTextGameObject.GetComponent<Text>();



    }
	
	// Update is called once per frame
	void Update () {
        incrimentLevelText();
        goldText.text = "Gold: " + CombatManager.gold.ToString();
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
