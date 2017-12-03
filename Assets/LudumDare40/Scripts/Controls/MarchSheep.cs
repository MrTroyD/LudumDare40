using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchSheep : MonoBehaviour {

    Sheep _sheep;
    Animal _animal;

	// Use this for initialization
	void Start ()
    {
        this._sheep = GetComponent<Sheep>();
        this._animal = GetComponent<Animal>();
    }

    private void OnEnable()
    {
        this._sheep = GetComponent<Sheep>();
        this._animal = GetComponent<Animal>(); 
    }

    // Update is called once per frame
    void Update () {
		if (this.transform.position.x < -11)
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * this._animal.movementSpeed);
        }
        else if (this.transform.position.x > 11)
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * this._animal.movementSpeed);

        }
        else
        {
            this.gameObject.AddComponent<WanderSheep>().SetNewDirection();
            Destroy(this);
        }
	}
}
