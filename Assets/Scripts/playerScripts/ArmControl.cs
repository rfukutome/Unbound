using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      ArmControl
//   Purpose:    Controls how the arm moves on the Player.
//
//   Notes: Attach onto the arm portion of a player.
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class ArmControl : MonoBehaviour {
    
    //rotation of the arm.
    public float rotZ;
    private bool facingRight = true;

	// Update is called once per frame
	void Update () {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (facingRight)
        { 
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180 - rotZ);    
        }
	}

    //Called when the player is flipped.
    public void MirrorArm()
    {
        facingRight = !facingRight;
    }
}
