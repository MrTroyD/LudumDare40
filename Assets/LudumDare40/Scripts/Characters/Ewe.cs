using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ewe : MonoBehaviour {

    public GameObject lamb;

    public float matingRecoveryTime = 10;
    public float pregnantTime = 5;

    Sheep _sheep;

    private float _pregnantTimeRemaining = 0;
    private bool _pregnant = false;

	// Use this for initialization
	void Start () {

        this._sheep = GetComponent<Sheep>();
	}

    private void OnEnable()
    {
        this._sheep = GetComponent<Sheep>();
    }

    // Update is called once per frame
    void Update () {
		if (this._pregnant)
        {
            this._pregnantTimeRemaining -= Time.deltaTime;

            if (this._pregnantTimeRemaining <= 0)
            {
                this._pregnant = false;

                //maybe add some crazy twins and triplets algo here

                Instantiate(lamb, this.transform.position, this.transform.rotation, this.transform.parent);

            }

        }
	}

    public void Mating()
    {
        this._sheep.OnMating(matingRecoveryTime);

        //Reduce speed

        //
        this._pregnant = true;
        this._pregnantTimeRemaining = pregnantTime;
    }
}
