using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MonsterLove.StateMachine;
public class Enemy3_StateMachine : Enemy3
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
    
    // NORMAL STATE //
    void Normal_Enter(){
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
    void Normal_Update(){
        if(DistanceToPlayer() <= rangeBeforeAttack){
            fsm.ChangeState(States.Attacking);
        }
    }
    void Normal_Exit(){}

    // CHASE STATE //
    void Chasing_Enter(){
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
    }
    void Chasing_Update(){}
    void Chasing_Exit(){}

    // ATTACK STATE //
    void Attacking_Enter(){
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
    }
    void Attacking_Update(){
        Attack();

        if(DistanceToPlayer() >= rangeBeforeAttack){
            fsm.ChangeState(States.Normal);
        }
    }
    void Attacking_Exit(){}

    // FLEE STATE //
    void Fleeing_Enter(){
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.black;
    }
    void Fleeing_Update(){}
    void Fleeing_Exit(){}


}