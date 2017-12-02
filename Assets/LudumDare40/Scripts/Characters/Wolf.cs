using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    public static Wolf instance;

    public bool bark;

    private Animal _animal;

	// Use this for initialization
	void Start () {
        this._animal = GetComponent<Animal>();
        Invoke("OnBark", 1);
	}

    void OnEnable()
    {
        this._animal = GetComponent<Animal>();
        Wolf.instance = this;
    }

    // Update is called once per frame
    void Update () {
	            
	}

    public void OnBark()
    {
        print("SFX of Bark Bark!");

        print("Text pop up of Bark! Bark!");
        this._animal.controlsActive = false;

        //This is temporary. We have to do this with a real timer not with Invoke
        Invoke("EnableControls", 2f);
    }

    public void EnableControls()
    {
        this._animal.controlsActive = true;
    }


}
