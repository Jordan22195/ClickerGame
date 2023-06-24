﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scritps;

public class DungeonUIBehaviorScript : MonoBehaviour {

    public GameObject levelTextGameObject;
    public GameObject autoAdvanceToggle;
    public TextMeshProUGUI goldText;
    public GameObject cameraObj;
    public static bool autoAdvanceLevel = false;
    public List<GameObject> groundClutterSprites;
    public List<GameObject> Clouds;
    public GameObject grassBlock;

    public GameObject Tree;


    public int floorYPos;

    public int backgroundDensity;
    public static int floor = -800;

    public GameObject ascendButton;

    public static float pixelsPerMeter = 320f;

    public static int chunksLoaded=-3;

    public static Vector3 cameraStartPos;
    public static int distanceTraveledInPixels;
    public static int distanceTraveledInChunks;
    private Text levelText;

    Vector3 inFramePos;
    Vector3 outOfFramePos;

    public static int chunkSize = 1920;
    public static int blockSize = 1000;

    // Use this for initialization
    void Start () {

        ascendButton.SetActive(false);
        levelText = levelTextGameObject.GetComponent<Text>();
        cameraStartPos = cameraObj.transform.position;
        Globals.cameraObject = cameraObj;

    }
	
	// Update is called once per frame
	void Update () {
        incrimentLevelText();
        goldText.text = "Gold: " + CombatManager.gold.ToString();
        if(CombatManager.canAscend())
        {
            //ascendButton.SetActive(true);
        }

        if(distanceTraveledInChunks > chunksLoaded -15)
        {
            generateChunk();
        }

    }

    public void stopRun()
    {
        StopAllCoroutines();
    }

    public void runState()
    {
        StopAllCoroutines();

        StartCoroutine(runCoroutine());
    }
    IEnumerator runCoroutine()
    {
        float then = Time.fixedTime;
        float now = then;

        while (true)
        {
            now = Time.fixedTime;
            run(now - then);
            then = now;
            yield return null;
        }
    }
    void run(float elapsedTime)
    {
        Vector3 movement = new Vector3(1f, 0f, 0f);
        cameraObj.transform.position += 
            movement * 
            elapsedTime * 
            (float)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) *
            pixelsPerMeter;
        distanceTraveledInPixels = (int)(cameraObj.transform.position.x - cameraStartPos.x);
        distanceTraveledInChunks = distanceTraveledInPixels / 1920;
    }


    public void incrimentLevelText()
    {
        levelText.text = (int)(distanceTraveledInPixels/pixelsPerMeter) + "m";
    }

    public void onAutoAdvanceToggle()
    {
        autoAdvanceLevel = autoAdvanceToggle.GetComponent<Toggle>().isOn;
    }

    public void OpenDungeonScene()
    {
        SceneManager.LoadScene("AscentionScreen", LoadSceneMode.Single);
    }

    public void generateChunk()
    {
        int xLow = (chunksLoaded * 1920) + (int)cameraStartPos.x;
        int xHigh = xLow + chunkSize;

        for (int i = 0; i < 10; i ++)
        {
            int x = Random.Range(xLow, xHigh);
            int z = Random.Range(200, 15000);
            generateGroundObject(Tree,  z, x);
        }

        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, Clouds.Count - 1);
            int x = Random.Range(xLow, xHigh);
            int z = Random.Range(5000, 35000);
            int y = Random.Range(200, 20000);
            generateObject(Clouds[index], y, z, x);
        }


        SpriteRenderer spriteRenderer = grassBlock.GetComponent<SpriteRenderer>();
        Vector3 itemSize = spriteRenderer.bounds.size;

        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        itemSize.y *= pixelsPerUnit;
        itemSize.x *= pixelsPerUnit;
        itemSize.z *= pixelsPerUnit;

        for (int i = 0; i < 16; i++)
        {
            int x = xLow + i * (int)itemSize.x;
            for (int ii = 0; ii < 16; ii++)
            {
                generateObject(grassBlock, floor, 0 + ii * (int)itemSize.z, x);

            }
        }


        chunksLoaded++;

    }

    public static GameObject generateGroundObject(GameObject prefab, int z, int x )
    {

        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        Vector3 itemSize = spriteRenderer.bounds.size;

        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        itemSize.y *= pixelsPerUnit;
        itemSize.x *= pixelsPerUnit;

        Vector3 pos;
        pos.y = floor + (itemSize.x /2);
        pos.x = x;
        pos.z = z;
        GameObject clone = Instantiate(prefab, pos, prefab.transform.rotation);
        return clone;
    }

    public void generateObject(GameObject prefab, int y, int z, int x)
    {
        

        Vector3 pos;
        pos.y = y;
        pos.x = x;
        pos.z = z;
        GameObject o = Instantiate(prefab, pos, prefab.transform.rotation);

    }
}
