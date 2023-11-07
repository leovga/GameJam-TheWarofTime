using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PControl : MonoBehaviour
{
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public GameObject Player;
    public GameObject Heart;
    public Animator anim;
    public AudioSource ac;
    public AudioClip phurt;
    

    public int health = 100;

    Rigidbody rb;

    public float playerSpeed = 4f;
    public float jumpHeight;
    public float sprintMult;
    public float yRot;
    float x;
    float z;


    Vector3 velocity;
    bool isGrounded;
    bool isSprinting;
    bool canHeal = true;

    void Start() {
        rb = GetComponent<Rigidbody>();
        ac = GetComponent<AudioSource>();
        anim = Heart.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground collision detection
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        MyInput();
        SpeedControl();
        HealthAnim();

    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput() // start
    {
        //Player movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        //Player jump
        if (Input.GetButtonDown("Jump") && isGrounded) {
            Jump();
        }
        
        //Player sprint
        if (Input.GetKey(KeyCode.LeftShift)) {
            isSprinting = true;

        } else {
            isSprinting = false;
        }
        if(Input.GetKey(KeyCode.E))
        {
            RotateControl();
        }
        if(Input.GetKey(KeyCode.F) && canHeal)
        {
            PlayerHeal();
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        } 

    }
    void MovePlayer()
    {
        Vector3 move = transform.right * x + transform.forward * z;
        move.Normalize();
        if(!isSprinting){
            rb.AddForce(move * playerSpeed * 10 , ForceMode.Force);
        }
        if(isSprinting) {
            rb.AddForce(move * playerSpeed * sprintMult * 10, ForceMode.Force);
        }
        
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > playerSpeed)
        {
            if(!isSprinting) {
            Vector3 limitedVel = flatVel.normalized * playerSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
            if(isSprinting) {
            Vector3 limitedVel = flatVel.normalized * playerSpeed * sprintMult;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void RotateControl()
    {
        yRot = transform.rotation.y;
        Quaternion rotate = Quaternion.Euler(0, yRot, 0);
        transform.rotation = rotate;
        
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    public void Damage(int amount)
    {
        health -= amount;
        ac.PlayOneShot(phurt);
        if(health < 0)
        {
            health = 0;
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if(health < 100)
        {
            health = 100;
        }
    }

    public void PlayerHeal()
    {
        health += 20;
        canHeal = false;
        StartCoroutine(PlayerHealCD());

    }

    IEnumerator PlayerHealCD()
    {
        yield return new WaitForSeconds(20);
        canHeal = true;
    }

    private void HealthAnim()
    {
        if(health == 100)
        {
            anim.SetInteger("healthState", 0);
        }
        if(health < 100 && health > 75)
        {
            anim.SetInteger("healthState", 1);
        }
        if(health <= 75 && health > 50)
        {
            anim.SetInteger("healthState", 2);
        }
        if(health <= 50 && health > 25)
        {
            anim.SetInteger("healthState", 3);
        }
        if(health <= 25 && health > 0)
        {
            anim.SetInteger("healthState", 4);
        }
    }
}
