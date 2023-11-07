using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionDetector : MonoBehaviour
{
    public WeaponController wc;
    public EnemyScript enemy;
    public GameObject HitParticle;
    public int damage = 10;
    public int knockback = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && wc.isAttacking)
        {
            Debug.Log(other.name);
            enemy = other.GetComponent<EnemyScript>();
            enemy.health -= damage;
            enemy.anim.SetTrigger("hurt");
            enemy.Hit();
            enemy.rb.AddForce(transform.up * knockback, ForceMode.Impulse);
        }

    }
}
