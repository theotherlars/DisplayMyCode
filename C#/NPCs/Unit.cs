using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS THE BASE CLASS OF ALL ENEMIES
// It holds all the necessary functions and properties for the enemies, each new enemy should inherit this base class and run base.ini() at start

[RequireComponent(typeof(Rigidbody2D),(typeof(AudioSource)))]
public class Units : Health
{
    [Header("Normal:")]
    [SerializeField]public float normalMovementSpeed;
    
    [Header("Chasing:")]
    [Range(0,100)]
    [SerializeField]public float rangeBeforeChase;
    [SerializeField]public float chasingMovementSpeed;

    [Header("Attacking:")]
    [Tooltip("How close does the enemy need to be the player before attacking")]
    [Range(0,100)]
    [SerializeField]public float rangeBeforeAttack;
    [SerializeField]public float attackRange;
    [SerializeField]public float timeBetweenAttacks;
    [Tooltip("If false: Uses 'Min Attack Damage'. If true: Random between 'Min' and 'Max Attack Damage'")]
    [SerializeField]public bool randomAttackDamage;
    [SerializeField]public int minAttackDamage;
    [SerializeField]public int maxAttackDamage;
    
    [Header("Fleeing:")]
    [Tooltip("How long does the player need to be from the enemy before it flees back to start point")]
    [Range(0,100)]
    [SerializeField]public float rangeBeforeFleeing;

    private Transform moveTowards; // Called when you want the NPC to move towards a target;
    
    [Header("Technical stuff:")]
    public Transform playerTransform; 
    [SerializeField]public Transform groundCastPoint;
    [SerializeField]public Transform playerCastPoint;
    [SerializeField]public Transform boxCastPoint;
    [SerializeField]private float baseCastDist;
    [SerializeField]private bool canFallDownEdges;
    [SerializeField]public LayerMask playerMask;
    [SerializeField]LayerMask whatIsGround;
    [SerializeField]public Animator animator;
    [HideInInspector]
    public Transform startingPoint;
    AudioSource audioSource;
    int facingDirection = 1;

    public virtual void Init(){
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetMaxHealth();
        startingPoint = transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    public virtual float FacingDirection(){
        // return transform.rotation.y > 0 ? 1 : -1;
        return transform.right.x;
    }

    public virtual float DistanceToPlayer(){
        float val = 999;
        float castDist = rangeBeforeChase;
        Vector2 endPos = playerCastPoint.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(playerCastPoint.position,playerTransform.position,playerMask);
        Debug.DrawLine(playerCastPoint.position,playerTransform.position,Color.magenta);
        if(hit.collider != null){
            if(hit.collider.gameObject.CompareTag("Player")){
                val = hit.distance;
            }
        }
        return val;
    }

    public virtual bool IsHittingWall(){
        bool val = false;
        float castDist = baseCastDist * facingDirection; // direction of cast
        Vector3 targetPos = groundCastPoint.position;
        targetPos.x += castDist;
        val = Physics2D.BoxCast(boxCastPoint.position,new Vector2(0.5f,0.3f),0,new Vector2(facingDirection,0),baseCastDist,whatIsGround) ? true : false;
        return val;
    } 

    public virtual bool IsNearEdge(){
        if(canFallDownEdges){return false;}
        bool val = false;
        float castDist = baseCastDist ; // direction of cast
        Vector3 targetPos = groundCastPoint.position;
        targetPos.y -= castDist*2;
        val = Physics2D.Linecast(groundCastPoint.position, targetPos, whatIsGround) ? false : true;
        Debug.DrawLine(groundCastPoint.position,targetPos,Color.green,0.1f);
        return val;
    }

    public virtual void Flip(){
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public virtual void PlaySFX(soAudio soAudio){
        audioSource.pitch = soAudio.AudioPitch;
        audioSource.PlayOneShot(soAudio.AudioClip, soAudio.AudioVolume);
    }

    private void OnDrawGizmosSelected(){
        if(boxCastPoint){
            Gizmos.DrawWireCube(boxCastPoint.position, new Vector3(0.5f,0.3f));
        }
    }   
}