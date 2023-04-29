using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MonsterLove.StateMachine;
public class Enemy2_StateMachine : Enemy2
{
    [SerializeField]private bool debugStates = false;
    private float timer;

    public StateMachine<States, Driver> fsm;
    public enum States{
        Normal,
        Chasing,
        Attacking,
        Fleeing
    }
    public class Driver{
        public StateEvent Update;
    }
    private void Awake() {
        Init();
        fsm = new StateMachine<States, Driver>(this);
        fsm.ChangeState(States.Normal);
    }

    void Update(){
        fsm.Driver.Update.Invoke();
        if(debugStates){
            print(fsm.State);
        }
    }
    void Normal_Enter(){
        // gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
    void Normal_Update(){
        
        if(DistanceToPlayer() <= rangeBeforeChase){
            fsm.ChangeState(States.Chasing);
        }
    }
    void Normal_Exit(){}
    void Chasing_Enter(){
        // gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        animator.Play("Charge",0);
        StartCoroutine(Charge());
        timer = 0;
    }
    void Chasing_Update(){
        if(chargeDone){
            fsm.ChangeState(States.Attacking);
        }

        if(DistanceToPlayer() >= rangeBeforeFleeing){
            StopCoroutine(Charge());
            // fsm.ChangeState(States.Normal);
        }
    }
    void Chasing_Exit(){}
    void Attacking_Enter(){
        // gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        canDoDamage = true;
        Attack();
    }
    void Attacking_Update(){
        fsm.ChangeState(States.Fleeing);
    }
    void Attacking_Exit(){
    }
    void Fleeing_Enter(){
        // gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        timer = 0;
    }
    void Fleeing_Update(){
        if(timer > 2){
            Flee();
        
            if(Vector2.Distance(transform.position,ceilingPoint) <= 1f || !CeilingAbove()){
                fsm.ChangeState(States.Normal);
            }
        }

        timer += Time.deltaTime;
    }
    void Fleeing_Exit(){}


}