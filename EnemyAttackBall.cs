using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBall : MonoBehaviour
{
    public PControl player;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            player = other.GetComponent<PControl>();
            player.Damage(damage);
            Destroy(gameObject);
        }
        if(other.tag == "ground")
        {
            Destroy(gameObject);
        }
    }   
} 
