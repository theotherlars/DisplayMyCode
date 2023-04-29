using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy2 : Units{

    [Header("Charge Settings:")]
    [Tooltip("The time this enemy charges before it launches itself at the player")]
    public float chargeTime;
    public bool chargeDone;
    public float chargeForce;
    [SerializeField]Vector3 chargeScale;

    [Header("Audio Stuff:")]
    [SerializeField] soAudio leapSound;
    [SerializeField] soAudio screechSound;

    [Header("")]
    public Vector2 ceilingPoint;
    public float jumpForce;
    [SerializeField]private float explosionRadius;
    float gravityScaleStorage;
    Vector3 startScale;
    Rigidbody2D rb;
    SpriteRenderer art;
    bool doOnce;
    float timeSinceAttack;
    [HideInInspector]public bool canDoDamage;


    public override void Init(){
        base.Init();
        rb = GetComponent<Rigidbody2D>();
        art = GetComponentInChildren<SpriteRenderer>();
        dieEvent.AddListener(Died);
        gravityScaleStorage = rb.gravityScale;
        startScale = art.transform.localScale;
        rb.gravityScale = 0;
        canDoDamage = false;
    }

    private void OnDisable() {
        dieEvent.RemoveListener(Died);
    }

    private void Update() {
        timeSinceAttack += Time.deltaTime;
    }

    public virtual void JumpToCeiling(){
        if(ceilingPoint != Vector2.zero && Vector2.Distance(transform.position,ceilingPoint) > 0.01f){
            // TODO: Jump to ceiling animations
            PlaySFX(leapSound);
            rb.velocity = Vector2.up * jumpForce;
            art.flipY = true;
        }        
    }

    public virtual void Chase(){
        // rb.gravityScale = gravityScaleStorage;
        // Vector2 direction = new Vector2(new Vector2(playerTransform.position.x - transform.position.x,0).normalized.x * chasingMovementSpeed, rb.velocity.y);
        // rb.velocity = direction;
    }
    public IEnumerator Charge(){
        PlaySFX(screechSound);
        art.transform.localScale = startScale;
        chargeDone = false;
        float timer = 0;
        Vector3 scaleDifference = chargeScale - startScale;
        while(timer < chargeTime){
            art.transform.localScale += scaleDifference / chargeTime * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        chargeDone = true;
    }

    public virtual void Attack(){
        art.flipY = false;
        chargeDone = false;
        animator.Play("Attack",0);
        PlaySFX(leapSound);
        rb.gravityScale = gravityScaleStorage;
        Vector2 direction = playerTransform.position - transform.position;
        Vector2 forceMultiplier =  CeilingAbove() == true ?  new Vector2(1.5f,1) : new Vector2(1.5f,3); 
        rb.AddForce(direction.normalized * forceMultiplier * chargeForce);
        rb.velocity = new Vector2(0.0f,rb.velocity.y);
        timeSinceAttack = 0;
        doOnce = true;
    }

    IEnumerator Explode(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerCastPoint.position,explosionRadius, playerMask.value);
        if(hits.Length > 0){
            for (var i = 0; i < hits.Length; i++){
                if(hits[i].TryGetComponent(out IHealth health)){
                    int damage = Random.Range(minAttackDamage,maxAttackDamage);
                    damage = randomAttackDamage ? damage : minAttackDamage;
                    health.LoseHealth(damage);
                }
            }
        }
        //TODO: Play animation & sound
        yield return null;
        Die();
    }

    public virtual void Flee(){
        art.transform.localScale = startScale;
        ceilingPoint = Vector2.zero;
        if(CeilingAbove()){
            rb.gravityScale = 0;
            if(doOnce){
                doOnce = false;
                JumpToCeiling();
            }
        }
    }

    void Died(){
        Destroy(gameObject,0.5f);
    }

    public virtual bool CeilingAbove(){
        ceilingPoint = Vector2.zero;
        bool val = false;
        float castDist = 10.0f;
        Vector2 targetPos = playerCastPoint.position;
        targetPos.y += castDist;
        RaycastHit2D hit = Physics2D.Linecast(playerCastPoint.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(playerCastPoint.position, targetPos,Color.red,2.0f);
        if(hit.collider != null){
            val = true;
            ceilingPoint = hit.point;
        }
        return val;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player") && canDoDamage){
            if(other.gameObject.TryGetComponent(out IHealth health)){
                canDoDamage = false;
                int damage = Random.Range(minAttackDamage,maxAttackDamage);
                damage = randomAttackDamage ? damage : minAttackDamage;
                health.LoseHealth(damage);
            }
        }
        canDoDamage = false;
    }
}