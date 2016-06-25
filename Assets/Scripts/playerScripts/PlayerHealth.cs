using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    public int health = 100;
    public float damagePeriod = 2f;
    public float damagePushForce = 10f;

    private float lastHitTime;
    private SpriteRenderer healthBar;
    private Vector3 healthScale;
    private Animator anim;

    private Slider healthSlider;
    private Slider shieldSlider;
    private Text healthText;
    void Awake()
    {
        //healthBar = GameObject.Find("healthBar").GetComponent<SpriteRenderer>();
        healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
        healthText = GameObject.Find("healthText").GetComponent<Text>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        healthText.text = health + "";
        anim = GetComponentInChildren<Animator>();
        //healthScale = healthBar.transform.localScale;
    }

    //===============================================================================
    //Function: OnCollisionEnter2D(Collision2D col)
    //Purpose: Automatically called when colliding with another object.
    //===============================================================================
    void OnCollisionEnter2D (Collision2D col)
    {
        //Collision is an enemy
        //
        EnemyBasic enemyBasics = col.gameObject.GetComponent<EnemyBasic>();
        if (col.gameObject.tag == "Enemy")
        {
            //If ample time has passed to be hit again;
            if (Time.time>lastHitTime + damagePeriod)
            {
                if(health > 0f)
                {
                    TakeDamage(col.transform, enemyBasics.attackDamage);
                    lastHitTime = Time.time;
                }
                else
                {
                    health = 0;
                    Death(); 
                }
            }
        }
    }

    //===============================================================================
    //Function: TakeDamage()
    //Purpose: Logic used for when the player takes damage.
    //===============================================================================
    void TakeDamage(Transform enemy, int attackDamage)
    {
        //playerControl.jump = false;
        //Vector3 damageVector = transform.position - enemy.position;

        health -= attackDamage;

        if (health <= 0)
        {
            health = 0;
            Death();
        }

        UpdateHealthBar();

    }
    //===============================================================================
    //Function: Death()
    //Purpose: Called when the player dies
    //===============================================================================
    void Death()
    {
        //Player is dead, disable movement/gun -> respawn
        anim.SetTrigger("Death");
        GetComponent<Player>().enabled = false;
    }

    //===============================================================================
    //Function: UpdateHealthBar()
    //Purpose: Updates both UI health bar and floating health bar
    //===============================================================================
    public void UpdateHealthBar()
    {
        //healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
        //healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
        healthSlider.value = health;
        healthText.text = health + "";
    }
}
