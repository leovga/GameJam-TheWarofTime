using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject sword;
    public Animator anim;

    public bool CanAttack;
    public bool isAttacking;
    public float AttackCooldown;
    public AudioClip SwordSlice;
    private AudioSource ac;
    
    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
        CanAttack = true;
        AttackCooldown = 1.0f;
        anim = sword.GetComponent<Animator>();
        Physics.IgnoreLayerCollision(8, 6);
        ac = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(CanAttack)
            {
                SwordAttack();
            }
        }
    }

    public void SwordAttack()
    {
        isAttacking = true;
        CanAttack = false;
        anim.SetTrigger("attacking");
        ac.PlayOneShot(SwordSlice);
        StartCoroutine(ResetAttackCooldown());
    }
    

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }
}
