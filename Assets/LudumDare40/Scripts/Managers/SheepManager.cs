using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour {
    public static SheepManager instance;

    public List<Ram> maleSheep;
    public List<Ewe> femaleSheep;
    public List<Lamb> lambSheep;

    public bool breedingEnabled = false;

    public GameObject ewe;
    public GameObject ram;
    
    public int sheepLevel = 0;

    [SerializeField]
    private float _levelAggression;

    // Use this for initialization
    void Awake() {
        SheepManager.instance = this;
    }

    private void OnEnable()
    {
        SheepManager.instance = this;
    }

    // Update is called once per frame
    void Update() {

        this._levelAggression = 0;
        foreach (Ram ram in this.maleSheep)
        {
            this._levelAggression += ram.GetComponent<Sheep>().aggression;

            if (femaleSheep.Count < maleSheep.Count + 2) ram.GetComponent<Sheep>().GetAngry(Time.deltaTime);
        }
        

        if(GameManager.instance.gameActive && (this.maleSheep.Count == 0 || this.femaleSheep.Count == 0))
        {
            IncreaseSheepLevel();
        }
    }

    public void IncreaseSheepLevel()
    {
        float startPoint = Random.Range(0, 1) < .5f ? -15 : 15;
        sheepLevel++;
        GameObject newSheep;
        for (int i = 0; i < sheepLevel + 4; i++)
        {
            newSheep = CreateSheep();

            Destroy(newSheep.GetComponent<WanderSheep>());
            newSheep.transform.position = new Vector3(startPoint - (((int)i/5) * 2),  newSheep.transform.position.y, 11 - ((i % 5) * 4));
            newSheep.AddComponent<MarchSheep>();
        }


    }

    public GameObject CreateSheep()
    {
        GameObject newSheep;
        if (this.maleSheep.Count < this.femaleSheep.Count)
        {
            newSheep = Instantiate(ram);            
        }
        else if (this.maleSheep.Count > this.femaleSheep.Count)
        {
            newSheep = Instantiate(ewe);
        }
        else
        {
            newSheep = Instantiate((Random.Range(0f, 1f) < .5f) ? ewe : ram);

        }


        AddSheep(newSheep);

        return newSheep;
    }

    public void CreateRandomSheep(int sheepCount)
    {
        GameObject newSheep;
        
        Vector3 destination;
        for (int i = 0; i < sheepCount; i++)
        {
            newSheep = CreateSheep();

            switch (i % 4)
            {
                
                case 1:
                    destination = new Vector3(Random.Range(-8, -4), .74f, Random.Range(-8, -4));
                    break;
                case 2:
                    destination = new Vector3(Random.Range(8, 4), .74f, Random.Range(-8, -4));
                    break;
                case 3:
                    destination = new Vector3(Random.Range(-8, -4), .74f, Random.Range(8, 4));
                    break;
                default:
                    destination = new Vector3(Random.Range(8, 4), .74f, Random.Range(8, 4));
                    break;
            }
            newSheep.transform.position = destination;
        }
    }

    public void ClearSheep()
    {
        for (int i = 0; i < this.maleSheep.Count; i++)
        {
            Destroy(this.maleSheep[i].gameObject);
        }

        for (int i = 0; i < this.femaleSheep.Count; i++)
        {
            Destroy(this.femaleSheep[i].gameObject);
        }

        for (int i = 0; i < this.lambSheep.Count; i ++)
        {
            Destroy(this.lambSheep[i].gameObject);
        }

        this.maleSheep.Clear();
        this.femaleSheep.Clear();
        this.lambSheep.Clear();
    }

    public void AddSheep(GameObject sheep)
    {
        if (sheep.GetComponent<Ewe>()) AddSheep(sheep.GetComponent<Ewe>());
        if (sheep.GetComponent<Ram>()) AddSheep(sheep.GetComponent<Ram>());
    }

    public void AddSheep(Ram ram)
    {
        if (!this.maleSheep.Contains(ram)) this.maleSheep.Add(ram);
    }

    public void AddSheep(Ewe ewe)
    {
        if (!this.femaleSheep.Contains(ewe)) this.femaleSheep.Add(ewe);
    }

    public void AddSheep(Lamb lamb)
    {
        this.lambSheep.Add(lamb);
    }

    public void EnableBreeding()
    {
        this.breedingEnabled = true;
    }

    public void DisableBreeding()
    {
        this.breedingEnabled = false;
    }

    public void OnEat(GameObject sheep)
    {
        GameManager.instance.sheepEaten++;
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
            RemoveLamb(sheep.GetComponent<Lamb>());
            Wolf.instance.OnFeed(5f);
            foreach (Ram ram in this.maleSheep)
            {
                ram.GetComponent<Sheep>().GetAngry(5);
            }
        }

        Destroy(sheep);
    }

    public void RemoveLamb(Lamb lamb)
    {
        if (this.lambSheep.Contains(lamb))
        {
            this.lambSheep.Remove(lamb);
        }
    }

    public void Frighten()
    {
            Sheep sheep;
        Vector3 wolfPosition = Wolf.instance.transform.position;

        foreach(Ram ram in this.maleSheep)
        {
            sheep = ram.GetComponent<Sheep>();
            if (sheep.aggression > 4.5 || Vector3.Distance(wolfPosition, ram.transform.position) > 3) continue;

            sheep.GetFrightened(.15f);

        }

        foreach(Ewe ewe in this.femaleSheep)
        {
            sheep = ewe.GetComponent<Sheep>();
            ewe.GetComponent<Sheep>().GetFrightened(.3f);
        }
    }
}
