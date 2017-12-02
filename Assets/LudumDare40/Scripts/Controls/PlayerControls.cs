using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animal))]
public class PlayerControls : MonoBehaviour {

    Animal _animal;
    Wolf _wolf;

	// Use this for initialization
	void Start () {
        this._animal = GetComponent<Animal>();
        this._wolf = GetComponent<Wolf>();
	}

    private void OnEnable()
    {
        this._animal = GetComponent<Animal>();
        this._wolf = GetComponent<Wolf>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this._animal.controlsActive) return;

        this._animal.moveRight = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
        this._animal.moveLeft = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        this._animal.moveUp = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        this._animal.moveDown = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));

        if(Input.GetKey(KeyCode.Space))
        {
            this._wolf.OnBark();
        }

      


    }
}
