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

     void Update()
    {
        if (!mateReady && mateRecoveryTime > 0)
        {
            mateRecoveryTime -= Time.deltaTime;

            if (mateRecoveryTime <= 0)
            {
                mateReady = true;
                mateRecoveryTime = 0;
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
        this.mateRecoveryTime = recoveryTime;
        mateReady = false;

    }
}
