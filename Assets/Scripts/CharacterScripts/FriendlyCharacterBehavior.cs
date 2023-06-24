using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scritps;



public class FriendlyCharacterBehavior : MonoBehaviour
{

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

    public enum chartype { FRIENDLY, ENEMEY }

    public chartype CharacterType = chartype.ENEMEY;

    private GameObject SceneManagerRef;
    public GameObject statusText;
    private CombatManager manager;




    private CharacterStats _stats = new CharacterStats();

    public virtual CharacterStats stats
    {
        get
        {
            return _stats;
        }
        set
        {
            _stats = value;
        }
    }


    void OnMouseDown()
    {
        knockback();
        Debug.Log("knockback");

    }

    // Use this for initialization
    public void Start () {
        stats.currentHP = stats.maxHP;
        attackPower = 1;
        primaryAttackSpeed = 0.5f; // attacks per second
        clickDamage = 1;
        moveSpeed = 1; // meters per second. todo: make this number make sense
        passiveDamage = 0; //dps?
        //CharacterType = chartype.FRIENDLY;
        animator = sprite.GetComponent<Animator>();
        runState();


    }
	

    void run(float elapsedTime)
    {
        Vector3 movement = new Vector3(1f, 0f, 0f);
        this.gameObject.transform.position += 
            movement * 
            (float)elapsedTime * 
            (float)CombatManager.managerRef.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) * 
            Globals.pixelsPerMeter;
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

    public  void attack()
    {
        EnemyBehavior target = CombatManager.managerRef.getTargetEnemy();
        if (target != null)
        {
            
            int damage = (int)CombatManager.managerRef.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER);
            CombatManager.managerRef.updateDPS(damage);
            target.takeDamage(damage);
            animator.SetTrigger("punch");

        }
    }




    public void setStatusText(string status)
    {
        statusText.GetComponent<Text>().text = status;
    }




    public void applyXP(int xpValue)
    {
        stats.xp += xpValue;
    }



    private void knockback()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
    }
}
