using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskerNode : MonoBehaviour {
    public float rotation = 0;
    public float weight = 1;

    private float _rayLength = 11;

    public float rayLength
    {
        set { this._rayLength = value; }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateNode()
    {
        this.weight = 1;
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.rotation = Quaternion.Euler(0, this.rotation, 0);
        this.transform.Translate(Vector3.forward * this._rayLength);

        float xPos = this.transform.position.x;
        float yPos = this.transform.position.z;
        

        this.transform.position = new Vector3(xPos, .5f, yPos);
    }
}
