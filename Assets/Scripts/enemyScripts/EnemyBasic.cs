using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/////////////////////////////////////////////////////////////////////////
//   Class:      EnemyBasic
//   Purpose:    This script will be used for the basics of enemies. 
//   The basics of enemies are health, attackDamage, defense, drops, 
//   and typing.
//   
//   Notes: Attach onto a any enemy. 
//   Contributors: RSF
/////////////////////////////////////////////////////////////////////////
public class EnemyBasic : MonoBehaviour {
    
    public int currentHealth;
    public int fullHealth;          //health
    public int attackDamage;    //damage from touching
    public float damagePopupSpread;
    public DamagePopup damageText;
    public string type;

    //item drops
    public List<Transform> item100 = new List<Transform>();
    public List<Transform> item50 = new List<Transform>();

    private SpriteRenderer healthBar;
    private Vector3 healthScale;
    private static GameObject canvas;

    // Use this for initialization
    void Awake () {
        currentHealth = fullHealth;
        healthBar = transform.FindChild("enemyHealthBar").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
        canvas = GameObject.Find("playerUI");       
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        //pop out the damage on the screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + Random.Range(-damagePopupSpread, damagePopupSpread), 
                                                                            transform.position.y + Random.Range(-damagePopupSpread, damagePopupSpread)));
        DamagePopup popupDamage = Instantiate(damageText);
        popupDamage.transform.SetParent(canvas.transform, false);
        popupDamage.transform.position = screenPosition;
        popupDamage.SetText(amount.ToString());
        

        //if (currentHP <= 45 && currentHP > 0)
        //{
        //    lerpto = (currentHP / (float)(startingHP * .45));
        //    Fill.color = Color.Lerp(minColor, maxColor, lerpto);
        //}

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        for (int i = 0; i <= item50.Count-1; i++)
        {
            if(Random.Range(0, 100) <= 50)
            {
                Transform tempItem = (Transform)Instantiate(item50[i], transform.position, transform.rotation);
                tempItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3f, 3f), (Random.Range(1f, 3f))), ForceMode2D.Force);
            }
        }

        for (int i = 0; i <= item100.Count - 1; i++)
        {
                Transform tempItem = (Transform)Instantiate(item100[i], transform.position, transform.rotation);
                tempItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4f, 4f), (Random.Range(1f, 3f))) , ForceMode2D.Force);
        }
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - currentHealth * 0.01f);

        healthBar.transform.localScale = new Vector3(healthScale.x * currentHealth *.01f, 1, 1);
    }

    public string getType()
    {
        return type;
    }
}
