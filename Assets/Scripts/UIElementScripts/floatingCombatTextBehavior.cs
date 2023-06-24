using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class floatingCombatTextBehavior : MonoBehaviour
{
    public GameObject floatingCombatText;
    public float timeToLive = 1.0f;
    private float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 50.0f);
        Destroy(this.gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        floatingCombatText.GetComponent<TextMeshProUGUI>().alpha 
            = Mathf.Lerp(1f, 0f, currentTime / timeToLive);
    }

    public void setFloatingCombatText(string newText)
    {
        floatingCombatText.GetComponent<TextMeshProUGUI>().SetText(newText);
    }
}
