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
    public float attackSpeed = 0.5f; // attacks per second
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
        attackSpeed = 0.5f; // attacks per second
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
    
        attackSpeed = 0.5f;
    }

    void run(float elapsedTime)
    {
        Vector3 movement = new Vector3(1f, 0f, 0f);
        this.gameObject.transform.position += movement * (float)elapsedTime * (float)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) * DungeonUIBehaviorScript.pixelsPerMeter;
    }

    public override void OnEnable()
    {
        CharacterType = chartype.FRIENDLY;
        base.OnEnable();
    }

    public override void performAction()
    {

    }

    public void idleState()
    {
        StopAllCoroutines();
        animator.SetInteger("state", (int)animationState.IDLE);
    }

    public void runState()
    {
        StopAllCoroutines();
        animator.SetInteger("state", (int)animationState.RUN);

        StartCoroutine(runCoroutine());
    }
    public void attackState()
    {
        StopAllCoroutines();

        StartCoroutine(attackCoroutine());
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

    IEnumerator attackCoroutine()
    {
        animator.SetInteger("state", (int)animationState.IDLE);
        yield return new WaitForSeconds(attackSpeed);
        while (true)
        {
            attack();
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    public override void attack()
    {
        Debug.Log("punch");
        animator.SetTrigger("punch");
        CharacterBehavior target = CombatManager.getTargetEnemy(this);
        if (target != null)
        {
            
            int damage = (int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER);
            CombatManager.updateDPS(damage);
            target.takeDamage(damage);
        }
    }


    public override void OnDestroy()
    {
        //saveChar();
        base.OnDestroy();
    }

    public override void die()
    {
        base.die();
        return;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void knockback()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
    }
}
