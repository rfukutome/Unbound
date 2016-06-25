using UnityEngine;
using System.Collections;

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
