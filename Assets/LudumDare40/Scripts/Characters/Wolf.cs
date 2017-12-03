using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    private const float STUFFED_SPEED = 4.5f;
    private const float NORMAL_SPEED = 6;
    private const float HUNGRY_SPEED = 6.5f;

    public static Wolf instance;

    public bool barkAction;
    public float barkCount;
    public Transform barkTransform;

    private Animal _animal;

    private bool _eating;
    private float _eatingTimer;

    private bool _on = true;
    private float _health = 100;
    
    [SerializeField]
    private float _hunger = 80f;

    private bool _gameStarted;

    public SpriteRenderer wolfSprite;

    public float hunger
    {
        get { return this._hunger; }
    }

    public float health
    {
        get { return this._health; }
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
            float hungerDeplete = Time.deltaTime * .8f;
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
        this.barkCount = 3;
        InvokeRepeating("Bark", .15f, .5f);

        Invoke("EnableControls", 2f);

    }

    public void OnBark()
    {

        if (this._eating) return;

        print("SFX of Bark Bark!");
        this.barkCount = 3;
        print("Text pop up of Bark! Bark!");
        InvokeRepeating("Bark", .15f, .25f);

        this._animal.controlsActive = false;

        //This is temporary. We have to do this with a real timer not with Invoke
        Invoke("EnableControls", 2f);
    }

    void Bark()
    {
        SheepManager.instance.Frighten();

        this.barkTransform.gameObject.SetActive(true);
        this.barkTransform.localPosition = new Vector3(Random.Range(-.5f, .5f), Random.Range(1, 1.25f), 0);

    }

    public void EnableControls()
    {
        CancelInvoke("Bark");

        this.barkTransform.gameObject.SetActive(false);
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

                Ram ram = collision.gameObject.GetComponent<Ram>();
                if (ram && ram.GetComponent<Sheep>().aggression > 3f)
                {
                    OnGetHit(ram);
                }
                else
                {
                    SheepManager.instance.OnEat(collision.gameObject);
                }
            }
            else if (collision.gameObject.tag == "Inedible")
            {
                print("Don't eat the lamb");
                SheepManager.instance.OnEat(collision.gameObject);

            }
        }
        else
        {
            Ram ram = collision.gameObject.GetComponent<Ram>();
            if (collision.gameObject.tag == "Edible" && ram)
            {
                    OnGetHit(ram);
            }
                
        }
    }

    public void OnFeed(float hungerValue)
    {
        this._hunger += hungerValue;

        if (this._hunger > 100) this._hunger = 100;
    }

    public void OnGetHit(Ram ram)
    {
        ram.GetComponent<Sheep>().GetFrightened(4);
        ram.GetComponent<WanderSheep>().ClearBehaviour();

        this._health -= 10;

        this._animal.moveUp = false;
        this._animal.moveDown = false;
        this._animal.moveLeft = false;
        this._animal.moveRight = false;

        this._animal.controlsActive = false;


        //Start blinking

        CancelInvoke("EndBlinking");
        CancelInvoke("EnableControls");
        Invoke("EnableControls", 2f);
        Invoke("EndBlinking", 4f);
        InvokeRepeating("Blink", .15f, .15f);
    }

    void Blink()
    {
        this._on = !this._on;
        this.wolfSprite.color = this._on ? new Color(1, 1, 1) : new Color(1, 1, 1, .5f);
        
    }

    void EndBlinking()
    {
        this.wolfSprite.color = new Color(1, 1, 1);
        this._on = true;
        this.wolfSprite.enabled = true;
             
        CancelInvoke("Blink");
    }

}
