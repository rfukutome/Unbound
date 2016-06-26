using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      HealthHover
//   Purpose:    This script is used to keep the health bar hovering above
//   the player.
//
//   Notes: Attach onto the health bar to hover.
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class HealthHover : MonoBehaviour
{

    public Vector3 offset;

    private Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = player.position + offset;
	}
}
