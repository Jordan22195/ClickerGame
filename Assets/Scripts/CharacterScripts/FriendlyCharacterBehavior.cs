using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scritps;



public class FriendlyCharacterBehavior : MonoBehaviour
{

    public enum playerStateEnum
    {
        RUN = 0,
        IDLE = 1,
        COMBAT = 2
    };

    public playerStateEnum playerState = playerStateEnum.RUN; 
    public int attackPower = 0;
    public float primaryAttackSpeed = 0.5f; // attacks per second
    public float secondaryAttackSpeed = .25f; // attacks per second
    public int clickDamage = 1;
    public float moveSpeed = 1; // meters per second. todo: make this number make sense
    public int passiveDamage = 0; //dps?
    public GameObject sprite;
    private Animator animator;

    public enum chartype { FRIENDLY, ENEMEY }

    public chartype CharacterType = chartype.ENEMEY;

    private GameObject SceneManagerRef;
    public GameObject statusText;
    private CombatManager manager;


    private Queue<string> animationQueue = new Queue<string>();



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

    public void Update()
    {
        if (animationQueue.Count > 0)
        {
            string anim = animationQueue.Dequeue();
            animator.SetTrigger(anim);
        }
    }


    public void OnEnable()
    {
        CharacterType = chartype.FRIENDLY;
    }

    public void endCombat()
    {
        Debug.Log("end Combat");
        if(playerState == playerStateEnum.COMBAT)
        {
            StopAllCoroutines();
            idleState();
        }
    }


    public void idleState()
    {
        Debug.Log("idle state");
        playerState = playerStateEnum.IDLE;
        animationQueue.Enqueue("idle");
    }

    public void runState()
    {
        Debug.Log("run state");
        playerState = playerStateEnum.RUN;
        animator.ResetTrigger("idle");
        animationQueue.Enqueue("run");

    }


    public void attackState()
    {
        Debug.Log("attack state");
        playerState = playerStateEnum.COMBAT;
        animationQueue.Enqueue("idle");

        StartCoroutine(attackCoroutine());
    }

    IEnumerator attackCoroutine()
    {

        while (playerState == playerStateEnum.COMBAT) 
        {
            yield return new WaitForSeconds(1/CombatManager.managerRef.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED));
            attack();
            yield return new WaitForSeconds(1/CombatManager.managerRef.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED)/2);
            attack();
            yield return new WaitForSeconds(1/CombatManager.managerRef.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_SPEED));
            animationQueue.Enqueue("idle");
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
            animationQueue.Enqueue("punch");

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
