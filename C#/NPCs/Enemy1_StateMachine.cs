using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public class Enemy1_StateMachine : Enemy1
{
    [SerializeField]private bool debugStates = false;
    private float timer;
    float nextAttack;

    public StateMachine<States, Driver> fsm;
    public enum States{
        Normal,
        Chasing,
        Attacking,
        Fleeing,
        Die
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
        if(debugStates){   
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }
    void Normal_Update(){
        Move();
        if(DistanceToPlayer() <= rangeBeforeChase){
            fsm.ChangeState(States.Chasing);
        }
    }
    void Normal_Exit(){}
    void Chasing_Enter(){
        if(debugStates){  
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        }
    }
    void Chasing_Update(){
        
        Chase(); // Chasing player!

        if(DistanceToPlayer() <= rangeBeforeAttack){
            
            fsm.ChangeState(States.Attacking);
        }

        if(DistanceToPlayer() >= rangeBeforeFleeing){
            fsm.ChangeState(States.Fleeing);
        }
    }
    void Chasing_Exit(){}
    void Attacking_Enter(){
        animator.SetBool("Walking",false);
        if(debugStates){  
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        }
        nextAttack = Time.time + timeBetweenAttacks;
    }
    void Attacking_Update(){
        if(Time.time >= nextAttack){
            animator.SetTrigger("Attacking");
            // StartCoroutine(Waiter(1.0f));
            // Attack();
            nextAttack = Time.time + timeBetweenAttacks;
        }
        
        if(DistanceToPlayer() >= rangeBeforeAttack){
            fsm.ChangeState(States.Chasing);
        }
    }
    void Attacking_Exit(){}
    void Fleeing_Enter(){
        if(debugStates){  
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
    }
    void Fleeing_Update(){
        Flee();
        if(Vector2.Distance(transform.position,startingPoint.position) <= 0.01f){
            fsm.ChangeState(States.Normal);
        }
    }
    void Fleeing_Exit(){

    }

    void Die_Enter(){}
    void Die_Update(){}
    void Die_Exit(){}
}