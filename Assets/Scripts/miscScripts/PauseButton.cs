using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      PauseButton
//   Purpose:    Simple script to pause the game. 
//
//   Notes: Attach onto any object in the game. Idealy the GameManager.
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class PauseButton : MonoBehaviour {

    private bool paused = false;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if(paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
	}
}
