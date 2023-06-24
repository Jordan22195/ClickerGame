using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIScript : MonoBehaviour
{
    public CharacterStats stats = new CharacterStats();
    public GameObject statsScreen;
    public GameObject statusText;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        //Camera c = CombatManager.cameraObj.GetComponent<Camera>();
        //canvas.GetComponent<Canvas > ().worldCamera = c;
    }

    public void initUI()
    {
        loadChar();
        statsScreen.GetComponent<statScreenBehavior>().loadStats(stats);
        //statsScreen.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

        //setStatusText(stats.name + " \r\n" + "Level: " + stats.level);
    }

    public void setStatusText(string newtext)
    {
        statusText.GetComponent<Text>().text = newtext;
    }

    public void loadChar(CharacterStats s)
    {
        this.stats = s;
    }

    public void loadChar()
    {
        if(this.gameObject.GetComponent<FriendlyCharacterBehavior>() != null)
        {
            this.stats = this.gameObject.gameObject.GetComponent<FriendlyCharacterBehavior>().stats;

        }
        else if (this.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            this.stats = this.gameObject.gameObject.GetComponent<EnemyBehavior>().stats;
        }
        return;

    }

    private void OnMouseDown()
    {
        Debug.Log(this.gameObject.name + " clicked");
        statsScreen.GetComponent<statScreenBehavior>().loadStats(stats);
       // statsScreen.SetActive(!statsScreen.activeSelf);
    }
}
