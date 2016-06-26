using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      Experience
//   Purpose:    This class is used to handle the value of experience 
//   dropped by enemies.
//   
//   Notes: Attach onto experience.
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class Experience : MonoBehaviour {
    public float value;

    void OnTriggerEnter2D(Collider2D other)
    {
        //If you run into money;
        //
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerItemInteraction>().addExperience(value);
            Destroy(gameObject);
        }
    }
}
