using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    private const float STUFFED_SPEED = 4.5f;
    private const float NORMAL_SPEED = 6;
    private const float HUNGRY_SPEED = 6.5f;

    public static Wolf instance;

    public bool bark;

    private Animal _animal;

    private bool _eating;
    private float _eatingTimer;
    
    [SerializeField]
    private float _hunger = 80f;

    private bool _gameStarted;
    
    public float hunger
    {
        get { return this._hunger; }
    }


	// Use this for initialization
	void Start () {
        this._animal = GetComponent<Animal>();
        Invoke("OnStartBark", 1);

        this._gameStarted = false;
	}

    void OnEnable()
    {
        this._animal = GetComponent<Animal>();
        Wolf.instance = this;
    }

    // Update is called once per frame
    void Update () {

        if (this._gameStarted)
        {
            float hungerDeplete = Time.deltaTime * .98f;
            this._hunger -= hungerDeplete;
        }

        if (this._eatingTimer > 0)
        {
            this._eatingTimer -= Time.deltaTime;
            if (this._eatingTimer < 0)
            {
                this._eating = false;
                this._eatingTimer = 0;
            }
        }

        if (this._hunger > 90)
        {
            this._animal.movementSpeed = Wolf.STUFFED_SPEED;
        }

        if (this._hunger <= 90 && this._hunger >= 30)
        {
            this._animal.movementSpeed = Wolf.NORMAL_SPEED;
        }

        if (this._hunger < 30)
        {
            this._animal.movementSpeed = Wolf.HUNGRY_SPEED;
        }
	}

    void OnStartBark()
    {
        this._gameStarted = true;
        this._animal.controlsActive = false;

        print("SFX of Bark Bark!");

        print("Text pop up of Bark! Bark!");


        Invoke("EnableControls", 2f);

    }

    public void OnBark()
    {

        if (this._eating) return;

        print("SFX of Bark Bark!");

        print("Text pop up of Bark! Bark!");
        this._animal.controlsActive = false;

        //This is temporary. We have to do this with a real timer not with Invoke
        Invoke("EnableControls", 2f);
    }

    public void EnableControls()
    {
        this._animal.controlsActive = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!this._eating)
        {
            this._eating = true;
            this._eatingTimer = 2f;

            if (collision.gameObject.tag == "Edible")
            {
                SheepManager.instance.OnEat(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Inedible")
            {
                print("Don't eat the lamb");
                SheepManager.instance.OnEat(collision.gameObject);

            }
        }
    }

    public void OnFeed(float hungerValue)
    {
        this._hunger += hungerValue;

        if (this._hunger > 100) this._hunger = 100;
    }

}
