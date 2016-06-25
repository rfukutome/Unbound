using UnityEngine;
using System.Collections;

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
