using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : MonoBehaviour {

    public float matingRecoveryTime = 3;
    Sheep _sheep;

    public bool mateReady;
    
    // Use this for initialization
    void Start()
    {
        this._sheep = GetComponent<Sheep>();
        AddToManager();

        this.name = this.name.Replace("(Clone)", "");
    }

    private void AddToManager()
    {
        if (SheepManager.instance)
        {
            SheepManager.instance.AddSheep(this);
        }
        else
        {
            Invoke("AddToManager", .15f);
        }
    }

    private void OnEnable()
    {
        this._sheep = GetComponent<Sheep>();
    }

    private void Update()
    {
        
    }

    public void Mating()
    {
        //Tell behaviour to drop aggression by half
        //this.aggression *= .5f;
        this._sheep.OnMating(matingRecoveryTime);
               
    }
}
