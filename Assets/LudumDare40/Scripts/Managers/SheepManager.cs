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

            if (femaleSheep.Count < maleSheep.Count + 2) ram.GetComponent<Sheep>().GetAngry(Time.deltaTime); 
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

    public void OnEat(GameObject sheep)
    {
        if (sheep.GetComponent<Ewe>())
        {
            Wolf.instance.OnFeed(20f);
            Ewe ewe = sheep.GetComponent<Ewe>();
            if (this.femaleSheep.Contains(ewe))
            {
                this.femaleSheep.Remove(ewe);
            }


            //This may agro any Rams in the area
            foreach (Ram ram in this.maleSheep)
            {
                WanderSheep ramWanderSheepScript = ram.GetComponent<WanderSheep>();
                if (ramWanderSheepScript.target == sheep.transform)
                {
                    ramWanderSheepScript.StartLookingForMate();
                }
            }
        }
        else if (sheep.GetComponent<Ram>())
        {
            Wolf.instance.OnFeed(20f);
            Ram ram = sheep.GetComponent<Ram>();
            if(this.maleSheep.Contains(ram))
            {
                this.maleSheep.Remove(ram);
            }

            //Add to score
        }
        else if (sheep.GetComponent<Lamb>())
        {
            Wolf.instance.OnFeed(5f);
            foreach (Ram ram in this.maleSheep)
            {
                ram.GetComponent<Sheep>().GetAngry(5);
            }
        }

        Destroy(sheep);
    }

    public void Frighten()
    {
            Sheep sheep;
        foreach(Ram ram in this.maleSheep)
        {
            sheep = ram.GetComponent<Sheep>();
            if (sheep.aggression > 3) continue;

            sheep.GetFrightened(.3f);

        }

        foreach(Ewe ewe in this.femaleSheep)
        {
            sheep = ewe.GetComponent<Sheep>();
            ewe.GetComponent<Sheep>().GetFrightened(.3f);
        }
    }
}
