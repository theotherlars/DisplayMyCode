using System.Collections;
using UnityEngine;

public class Enemy3 : Units{

    [Header("Attack Pulse Settings:")]
    [SerializeField]Transform attackCircle;

    [Tooltip("How long before the pulse disapears")]
    [SerializeField]float pulseTime;
    [SerializeField]Vector2 maxSize;
    Rigidbody2D rb;
    bool canAttack;
    [HideInInspector] public bool canDoDamage;
    

    public override void Init(){
        base.Init();
        rb = GetComponent<Rigidbody2D>();
        canAttack = true;
        canDoDamage = true;
    }

    public virtual void Chase(){

    }

    public virtual void Attack(){
        if(!canAttack){return;}
        StartCoroutine(PulseAttack());
    }

    IEnumerator PulseAttack(){
        canAttack = false;
        canDoDamage = true;
        float timer = 0;
        Vector3 startScaleSize = attackCircle.localScale;
        //TODO: Add animations, particles and sound during attack
        while(timer < pulseTime){
            timer += Time.deltaTime;
            attackCircle.localScale = Vector3.Lerp(startScaleSize, maxSize, timer / pulseTime);
            yield return null;
        }
        attackCircle.localScale = new Vector2(1,1);
        canDoDamage = false;
        yield return new WaitForSecondsRealtime(timeBetweenAttacks);
        canAttack = true;
    }

    public virtual void Flee(){

    }
}