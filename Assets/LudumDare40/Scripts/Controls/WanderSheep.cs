using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSheep : MonoBehaviour {

    public float maxWidth = 11;

    private Animal _animal;
    private bool _adult;
    
	// Use this for initialization
	void Start () {
        this._animal = this.GetComponent<Animal>();
	}

    void OnEnable()
    {
        this._animal = this.GetComponent<Animal>();
        this._adult = (GetComponent<Lamb>() == null);
    }

    // Update is called once per frame
    void Update () {

         

        if (!this._adult) return;
	}
}
