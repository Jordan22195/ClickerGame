using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyCharacterBehavior : CharacterBehavior {

    enum animationState
    {
        RUN = 0,
        IDLE = 1,
        PUNCH = 2
    };


    public int attackPower = 0;
    public float primaryAttackSpeed = 0.5f; // attacks per second
    public float secondaryAttackSpeed = .25f; // attacks per second
    public int clickDamage = 1;
    public double moveSpeed = 1; // meters per second. todo: make this number make sense
    public int passiveDamage = 0; //dps?
    public GameObject sprite;
    private Animator animator;

    void OnMouseDown()
    {
        knockback();
        Debug.Log("knockback");

    }

    // Use this for initialization
    public override void Start () {
        attackPower = 1;
        primaryAttackSpeed = 0.5f; // attacks per second
        clickDamage = 1;
        moveSpeed = 1; // meters per second. todo: make this number make sense
        passiveDamage = 0; //dps?
        //CharacterType = chartype.FRIENDLY;
        animator = sprite.GetComponent<Animator>();
        runState();
        base.Start();


    }
	
	// Update is called once per frame
	public override void Update () {
        //statusText.GetComponent<Text>().text = "HP: " + me.HP.ToString();
        base.Update();
    
        primaryAttackSpeed = 0.5f;
    }

    void run(float elapsedTime)
    {
        Vector3 movement = new Vector3(1f, 0f, 0f);
        this.gameObject.transform.position += movement * (float)elapsedTime * (float)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) * DungeonUIBehaviorScript.pixelsPerMeter;
    }

    public void OnEnable()
    {
        CharacterType = chartype.FRIENDLY;
    }


    public void idleState()
    {
        StopAllCoroutines();
        animator.SetTrigger("idle");
    }

    public void runState()
    {
        StopAllCoroutines();
        animator.SetTrigger("run");

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

    public void attackState()
    {
        StopAllCoroutines();
        animator.SetTrigger("idle");

        StartCoroutine(attackCoroutine());
    }

    IEnumerator attackCoroutine()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(primaryAttackSpeed);
            attack();
            yield return new WaitForSeconds(secondaryAttackSpeed);
            attack();
            yield return new WaitForSeconds(primaryAttackSpeed);
            animator.SetTrigger("idle");
        }
    }

    public override void attack()
    {
        Debug.Log("punch");
        EnemyBehavior target = CombatManager.getTargetEnemy();
        if (target != null)
        {
            
            int damage = (int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER);
            CombatManager.updateDPS(damage);
            target.takeDamage(damage);
            animator.SetTrigger("punch");

        }
    }




    private void knockback()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
    }
}
