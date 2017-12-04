using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerIcon : MonoBehaviour {

    public Wolf wolf;
    RectTransform rt;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (wolf.eatingTimer > 0 )
        {
            this.transform.localScale = Vector3.Lerp(Vector3.one * .75f, Vector3.one * .5f, (1.5f - wolf.eatingTimer) / 1.5f); 
                
        }
                
	}
}
