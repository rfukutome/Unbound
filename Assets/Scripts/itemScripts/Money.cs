using UnityEngine;
using System.Collections;

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
