using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamb : MonoBehaviour {

    public float _growthTime;
    
    public GameObject ramPrefab;
    public GameObject ewePrefab;

    private Sheep _sheep;

	// Use this for initialization
	void Start () {
        this._sheep = this.GetComponent<Sheep>();
        DecideSex();
    }

    private void OnEnable()
    {
        this._sheep = this.GetComponent<Sheep>();
    }

    // Update is called once per frame
    void Update () {
        this._growthTime -= Time.deltaTime;
        if (this._growthTime <= 0)
        {
            //No longer a lamb! Now an adult!
            

            //This needs to be replaced with a real algo
            if (this._sheep.sex == Sheep.Sex.Male)
            {
                Instantiate(ramPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
            }
            else
            {
                Instantiate(ewePrefab, this.transform.position, this.transform.rotation, this.transform.parent);

            }

            //Tell Sound manager to play sound of change
            Destroy(this.gameObject);
        }
		
	}

    void DecideSex()
    {
        if (Random.Range(0f, 1f) > .5f)
        {
            this._sheep.sex = Sheep.Sex.Male;
        }
        else
        {
            this._sheep.sex = Sheep.Sex.Female;
        }
    }
}
