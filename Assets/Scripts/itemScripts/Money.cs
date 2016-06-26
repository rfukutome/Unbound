using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      Money
//   Purpose:    This class is used to handle the value of money or URC of the
//   character.
//   
//   Notes: Attach onto money
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class Money : MonoBehaviour {
    public int value;

    void OnTriggerEnter2D(Collider2D other)
    {
        //If you run into money;
        //
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerItemInteraction>().addMoney(value);
            Destroy(gameObject);
        }
    }
}
