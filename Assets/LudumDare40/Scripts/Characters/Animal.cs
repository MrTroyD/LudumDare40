using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

    public bool moveUp;
    public bool moveDown;
    public bool moveLeft;
    public bool moveRight;

    public bool controlsActive;

    public float movementSpeed;

    Vector3 movementVector;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!controlsActive) return;
		if (this.moveUp)
        {
            movementVector.z = movementSpeed;

        }
        else if (this.moveDown)
        {
            movementVector.z = -movementSpeed;
        }
        else
        {
            movementVector.z = 0;
        }

        if (this.moveRight)
        {
            movementVector.x = movementSpeed;
        }
        else if (this.moveLeft)
        {
            movementVector.x = -movementSpeed;
        }
        else
        {
            movementVector.x = 0;
        }


        if (movementVector.magnitude > movementSpeed)
        {
            movementVector *= .75f;
        }
        
        this.transform.Translate(movementVector * Time.deltaTime);
	}
}
