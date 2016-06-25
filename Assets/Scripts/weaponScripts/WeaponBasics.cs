using UnityEngine;
using System.Collections;

public class WeaponBasics : MonoBehaviour {

    public float fireRate = 0;
    public float range;
    public int damage = 10;
    public float bulletSpeed = 10f;

    public string type;

    public GameObject shotPrefab;

    public float effectSpawnRate = 7;
    float timeToFire = 0;
    Transform firePoint;

    bool facingRight;
    // Use this for initialization
	void Awake () {
        facingRight = true;

        firePoint = transform.FindChild("firePoint");
        if(firePoint == null)
        {
            Debug.LogError("No FirePoint");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    //===============================================================================
    //Function: Shoot()
    //Purpose: Function that instantiates bullets to shoot out of a gun. This function
    //  gives the bullet its' damage value, type(from the gun) and velocity.
    //===============================================================================
    void Shoot()
    {
        GameObject bullet;
        Vector2 difference = firePoint.right;
        float bulletTime = range / bulletSpeed;
        if (!facingRight)
        {
            difference.x *= -1;
            bullet = (GameObject)Instantiate(shotPrefab, firePoint.position, 
                                             Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, 180- firePoint.eulerAngles.z));
        }
        else
        {
            bullet = (GameObject)Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
        }
        bullet.GetComponent<DamageProperties>().damage = damage;
        bullet.GetComponent<DamageProperties>().type = type;
        bullet.GetComponent<DamageProperties>().bulletTime = bulletTime;

        
        bullet.GetComponent<Rigidbody2D>().AddForce((difference)*bulletSpeed , ForceMode2D.Force);
    }

    public void setFacingRight(bool argFacingRight)
    {
        facingRight = argFacingRight;
    }
}
