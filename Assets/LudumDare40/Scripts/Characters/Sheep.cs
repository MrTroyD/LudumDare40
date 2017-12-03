using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    public enum Sex
    {
        Male,
        Female
    }

    public Sex sex;

    public bool mateReady;
    public float mateRecoveryTime = 5;

    [SerializeField]
    private float _aggression = 0;

    [SerializeField]
    private float _matingDesire = 0;

    public float aggression
    {
        get { return this._aggression; }
    }

     void Update()
    {

        if (this._aggression > 0.5f)
        {
            this._aggression -= (Time.deltaTime * .5f);
        }
        else if (this._aggression < -0.5f)
        {
            this._aggression += (Time.deltaTime * .5f);
        }
        else
        {
            this._aggression *= .75f;
        }


        if (!mateReady && mateRecoveryTime > 0)
        {
            mateRecoveryTime -= Time.deltaTime;

            if (mateRecoveryTime <= 0)
            {
                mateReady = true;
                mateRecoveryTime = 0;
            }
        }
        else
        {
            this._matingDesire += Time.deltaTime;

            if (this._matingDesire >= 7f)
            {
                if (Random.Range(0, 1f) < .9f)
                {
                    this.GetComponent<WanderSheep>().StartLookingForMate();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!mateReady) return;

        if (collision.gameObject.tag == "Edible")
        {
            Sheep sheep = collision.gameObject.GetComponent<Sheep>();

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (sheep.sex != this.sex)
            {
                // It's mating time!!
                this.BroadcastMessage("Mating");
            }

        }
    }

    public void OnMating (float recoveryTime)
    {
        this._matingDesire = -4f;
        this.mateRecoveryTime = recoveryTime;
        mateReady = false;

    }

    public void GetAngry(float aggressionAdjustment)
    {
        this._aggression += aggressionAdjustment;
        if (this._aggression > 5)
        {
            this._aggression = 5;
        }

        this.GetComponent<WanderSheep>().CheckAggression();
    }

    public void GetFrightened(float aggressionAdjustment)
    {
        this._aggression -= aggressionAdjustment;

        if (this._aggression < -3)
        {
            this._aggression = -3;
        }

        this.GetComponent<WanderSheep>().CheckAggression();
    }
}
