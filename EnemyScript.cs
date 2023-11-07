using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public int health = 100;
    public int CorpseRemovalTime = 1;
    public Animator anim;
    public AudioClip hurt;
    public AudioClip thud;
    public AudioSource ac;
    public Rigidbody rb;
    public PControl pcon;
    private BoxCollider bc;

    public NavMeshAgent agent;
    public Transform Player;
    public LayerMask whatisground, whatisplayer;
    
    public Vector3 walkpoint;
    bool walkpointset;
    public float walkpointrange;
    
    public float timebetweenattacks = 3;
    bool alreadyattacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update
    private void Awake()
    {
        Player= GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        rb = GetComponent<Rigidbody>();
        ac = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider>();
        pcon = Player.GetComponent<PControl>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisplayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatisplayer);

        if(!playerInSightRange && !playerInAttackRange) 
        {
            Patroling();
        }
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

        Die();
    }

    private void Patroling()
    {
        if(!walkpointset) SearchWalkPoint();

        if(walkpointset)
        {
            agent.SetDestination(walkpoint);
            anim.SetBool("walking", true);
            anim.SetBool("running", false);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkpointset = false;
        }

    }
    private void SearchWalkPoint()
    {
        float randz = Random.Range(-walkpointrange, walkpointrange);
        float randx = Random.Range(-walkpointrange, walkpointrange);

        walkpoint = new Vector3(transform.position.x + randx, transform.position.y, transform.position.z + randz);
        if(Physics.Raycast(walkpoint, -transform.up, 2f, whatisground))
        {
            walkpointset = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(Player.position);
        anim.SetBool("running", true);
        anim.SetBool("walking", false);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        if(!alreadyattacked)
        {
            
            anim.SetTrigger("attack");
            alreadyattacked = true;
            
            pcon.Damage(10);
            Invoke(nameof(ResetAttack), timebetweenattacks);
        }
    }
    private void ResetAttack()
    {
        alreadyattacked = false;
        
    }

    public void Hit()
    {
        ac.PlayOneShot(hurt);
        ac.PlayOneShot(thud);
    }

    void Die()
    {
        if(health <= 0)
        {
            anim.SetTrigger("dead");
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
            StartCoroutine(Death());
        }
    }

     IEnumerator Death()
    {
        yield return new WaitForSeconds(CorpseRemovalTime);
        Destroy(gameObject);
    }
}
