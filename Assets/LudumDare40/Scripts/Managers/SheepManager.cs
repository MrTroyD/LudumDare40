using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour {
    public static SheepManager instance;

    public List<Ram> maleSheep;
    public List<Ewe> femaleSheep;

    [SerializeField]
    private float _levelAggression;

	// Use this for initialization
	void Awake () {
        SheepManager.instance = this;
	}

    private void OnEnable()
    {
        SheepManager.instance = this;
    }

    // Update is called once per frame
    void Update () {

        this._levelAggression = 0;
        foreach(Ram ram in this.maleSheep)
        {
            this._levelAggression += ram.GetComponent<Sheep>().aggression;
        }

        //If the ram aggression is over a certain amount. ALL Ram get the highest level of aggression
        if (this._levelAggression > 15)
        {
            foreach (Ram ram in this.maleSheep)
            {
                ram.GetComponent<Sheep>().GetAngry(999);
            }
        }
	}

    public void AddSheep(Ram ram)
    {
        this.maleSheep.Add(ram);
    }

    public void AddSheep(Ewe ewe)
    {
        this.femaleSheep.Add(ewe);
    }
}
