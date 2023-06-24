using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarBehavaiorScript : MonoBehaviour
{

    public GameObject powerbarBackground;
    public GameObject powerbarFill;

    public float fill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float p = CombatManager.managerRef.getPowerBarPercent();
        updateBar(p);
    }

    //0.00 to 1.00
    public void updateBar(float percentFill)
    {
        if( percentFill > 1.0)
        {
            percentFill = 1.0f;

        }
        if (percentFill < 0)
        {
            percentFill = 0;
        }
        Vector3 barscale = powerbarBackground.transform.localScale;
        Vector3 barposition = powerbarBackground.transform.localPosition;
        float barwidth = powerbarBackground.GetComponent<SpriteRenderer>().size.x * barscale.x;
        barscale.x = barscale.x * percentFill;
        barposition.x = powerbarBackground.transform.localPosition.x - ((1 - percentFill) * barwidth / 2);
        powerbarFill.transform.localScale = barscale;
        powerbarFill.transform.localPosition = barposition;
    }
}
