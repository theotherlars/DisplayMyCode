using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Units{

    Rigidbody2D rb;
    [SerializeField]public Transform attackPoint;
    [SerializeField]public Transform arm;
    Enemy1_StateMachine stateMachine;
    [SerializeField]soAudio attackSFX;
    [SerializeField]soAudio attackHitSFX;
    
    public override void Init(){
        base.Init();
        loseHealthEvent.AddListener(TakeDamage);
        stateMachine = GetComponent<Enemy1_StateMachine>();
        dieEvent.AddListener(Died);
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnDisable() {
        loseHealthEvent.RemoveListener(TakeDamage);
        dieEvent.RemoveListener(Died);
    }

    public virtual void Move(){
        if(stateMachine.fsm.State != Enemy1_StateMachine.States.Chasing){
            if(IsHittingWall() || IsNearEdge()){
                Flip();
            }
        }
        rb.velocity = new Vector2(FacingDirection() * normalMovementSpeed , rb.velocity.y);
        //TODO: Add walking animations
        if(rb.velocity.x != 0){
            animator.SetBool("Walking",true);
        }
        else{
            animator.SetBool("Walking",false);
        }
    }

    public virtual void Chase(){
        if(new Vector2(playerTransform.position.x - transform.position.x,0).normalized.x != FacingDirection()){
            Flip();
        }

        Vector2 direction = new Vector2(new Vector2(playerTransform.position.x - transform.position.x,0).normalized.x * chasingMovementSpeed, rb.velocity.y);
        //TODO: Add chasing animations
        rb.velocity = direction;
        if(rb.velocity.x != 0){
            animator.SetBool("Walking",true);
        }
        else{
            animator.SetBool("Walking",false);
        }
    }

    public virtual void Attack(){
        // TODO: Add attack animation
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerMask);
        int damage = (int)minAttackDamage;
        if(hit){   
            if(hit.TryGetComponent(out IHealth playerHealth)){
                if(randomAttackDamage){
                    damage = Random.Range((int)minAttackDamage,(int)maxAttackDamage);
                    playerHealth.LoseHealth(damage);
                    print(gameObject.name + " just hit player with " + damage + " damage");
                }
                else{
                    playerHealth.LoseHealth(damage);
                    print(gameObject.name + " just hit player with " + damage + " damage");
                }
                PlaySFX(attackHitSFX);
            }   
        }
        else{
            PlaySFX(attackSFX);
        }
    }

    private void TakeDamage(){
        if(CurrentHealth != 0){
            animator.SetTrigger("Hit");
        }
    }

    private void Died(){
        stateMachine.fsm.ChangeState(Enemy1_StateMachine.States.Die);
        rb.isKinematic = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetTrigger("Die");
    }
    public void Kill(){ // TRIGGERED AT THE END OF DIE ANIMATION VIA EVENT
        Destroy(gameObject);
    }

    public virtual void Flee(){
        // TODO: Add flee animation
        Vector2 direction = new Vector2(startingPoint.position.x - transform.position.x, rb.velocity.y);
        rb.velocity = direction.normalized * normalMovementSpeed;
        print("Trying to flee from player");
    }

    public IEnumerator Waiter(float seconds){
        yield return new WaitForSeconds(seconds);
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint){Gizmos.DrawWireSphere(attackPoint.position,attackRange);}
        if(boxCastPoint){Gizmos.DrawWireCube(boxCastPoint.position, new Vector2(0.5f,0.3f));}
    }
}
