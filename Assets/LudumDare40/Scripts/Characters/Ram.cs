using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : MonoBehaviour {

    public float matingRecoveryTime = 3;
    Sheep _sheep;

    // Use this for initialization
    void Start()
    {

        this._sheep = GetComponent<Sheep>();
    }

    private void OnEnable()
    {
        this._sheep = GetComponent<Sheep>();
    }

    public void Mating()
    {
        this._sheep.OnMating(matingRecoveryTime);
    }
}
