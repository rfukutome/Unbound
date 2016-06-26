using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      DamageProperties
//   Purpose:    This class holds all the properties of a bullet.
//
//   Notes: Attach onto bullet prefab.
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class DamageProperties : MonoBehaviour {
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public string type;

    private float weak = 1.5f;
    private float resist = .5f;

    public float bulletTime;
    void Update()
    {
        Invoke("destroyBullet", bulletTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Enemy")
        {
            int damageDone = damage;
            EnemyBasic otherEnemyBasic = other.GetComponent<EnemyBasic>();
            string otherType = otherEnemyBasic.type;

            switch (type)
            {
                case "Normal":
                    damageDone = damage;
                    break;
                    
                case "Fire":
                    if(otherType == "Fire" || otherType == "Water")
                    {
                        damageDone = (int)(resist * damage);
                    }
                    else if(type == "Wood")
                    {
                        damageDone = (int)(weak * damage);
                    }
                    break;

                case "Water":
                    if (type == "Water" || type == "Wood")
                    {
                        damageDone = (int)(resist * damage);
                    }
                    else if (type == "Fire")
                    {
                        damageDone = (int)(weak * damage);
                    }
                    break;

                case "Wood":
                    if (type == "Wood" || type == "Fire")
                    {
                        damageDone = (int)(resist * damage);
                    }
                    else if (type == "Water")
                    {
                        damageDone = (int)(weak * damage);
                    }
                    break;

                case "Dark":
                        damageDone = 2*damage;
                    break;
            }
            
            otherEnemyBasic.TakeDamage(damageDone);
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "ground")
        {
            Destroy(gameObject);
        }
    }

    void destroyBullet()
    {
        Destroy(gameObject);
    }
}
