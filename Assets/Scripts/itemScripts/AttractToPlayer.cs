using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      AttractToPlayer
//   Purpose:    Items that drop from enemies or chests, will be attracted
//   to the player. The player will automatically pick up anything this 
//   script is attached to.
//   
//   Notes: Attach onto item drops. 
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class AttractToPlayer : MonoBehaviour
{
    public float smooth = .5f;
    public float delay = 2;
    float delayStart;

    Transform player;
	// Use this for initialization

	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        delayStart = Time.time + delay;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time > delayStart)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * smooth);
            smooth += .05f;
        }
	}
}
