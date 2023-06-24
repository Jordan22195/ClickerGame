using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scritps;
using UnityEngine.SceneManagement;

public class AscensionGUIScript : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI moreInfoText;
    // Start is called before the first frame update
    void Start()
    {
        Globals.ascensionLevel++;
        titleText.text = "ASCENSTION " + Globals.AscensionLevelToString(Globals.ascensionLevel-1) + " to " +
             Globals.AscensionLevelToString(Globals.ascensionLevel);

        moreInfoText.text = "Base Damage Increased to " + Globals.getBaseDamage().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDungeonScene()
    {
        SceneManager.LoadScene("dungeon1", LoadSceneMode.Single);
    }
}
