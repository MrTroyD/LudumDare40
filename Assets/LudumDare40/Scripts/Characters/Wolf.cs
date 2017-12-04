using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    private const float STUFFED_SPEED = 4.5f;
    private const float NORMAL_SPEED = 6;
    private const float HUNGRY_SPEED = 6.5f;

    public static Wolf instance;

    public float barkCount;
    public Transform barkTransform;

    private Animal _animal;

    private bool _barkAction;

    private bool _eating;
    private float _eatingTimer;
    
    private bool _invincible;

    private bool _on = true;
    private bool _jumpToggle = false;

    [SerializeField]
    private float _health = 100;
    
    [SerializeField]
    private float _hunger = 100f;

    private bool _gameStarted;

    public SpriteRenderer wolfSprite;

    public Sprite normalSprite;
    public Sprite deathSprite;
    public Sprite mouthFullSprite;
    public Sprite barkSprite;

    public float eatingTimer
    {
        get { return this._eatingTimer; }
    }

    public bool eating
    {
        get { return this._eating; }
    }

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
     
        this._gameStarted = false;
	}

    void OnEnable()
    {
        this._animal = GetComponent<Animal>();
        wolfSprite.sprite = normalSprite;
        Wolf.instance = this;
    }

    // Update is called once per frame
    void Update () {
        if (!GameManager.instance.gameActive) return;

        if (this._animal.moveRight)
        {
            this.wolfSprite.flipX = false;
        }
        else if (this._animal.moveLeft)
        {
            this.wolfSprite.flipX = true;
        }

        if (this._gameStarted && !this._eating)
        { 
            float hungerDeplete = Time.deltaTime * 2;
            this._hunger -= hungerDeplete;
        }

        if (this._eatingTimer > 0)
        {
            this._eatingTimer -= Time.deltaTime;
            if (this._eatingTimer < 0)
            {
                this._eating = false;

                this.wolfSprite.sprite = this.normalSprite;
                this._eatingTimer = 0;
            }
        }

        /*
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
        */

        if (this._hunger < 0)
        {
            this._health += this._hunger;
            this._hunger = 0;
        }
	}

    public void ResetWolf()
    {
        CancelInvoke("Bark");
        CancelInvoke("Blink");
        CancelInvoke("EndBlinking");
        CancelInvoke("EnableControls");
        this._health = 100;
        this._hunger = 100;
        this._gameStarted = true;
        this._animal.controlsActive = false;
        this._invincible = false;
        this.wolfSprite.sprite = normalSprite;
        this.transform.position = new Vector3(0, 0.51f, 0);

        this.barkTransform.gameObject.SetActive(false);
    }

    public void OnStartBark()
    {
        ResetWolf();
        this._barkAction = true;
        this.barkCount = 3;
        InvokeRepeating("Bark", .15f, .15f);

        Invoke("EnableControls", 1.5f);

    }

    public void OnBark()
    {

        this._barkAction = true;
        if (this._eating) return;

        this.barkCount = 3;

        InvokeRepeating("Bark", .15f, .15f);
        

        this._animal.controlsActive = false;

        //This is temporary. We have to do this with a real timer not with Invoke
        Invoke("EnableControls", 1.5f);
    }

    void Bark()
    {
        SheepManager.instance.Frighten();

        this._jumpToggle = !this._jumpToggle;
        

        if (this._jumpToggle)
        {
            this.wolfSprite.transform.localPosition = new Vector3(0, .3f, 0);
            AudioManager.instance.PlaySound("Bark");
        this.barkTransform.gameObject.SetActive(true);
        this.barkTransform.localPosition = new Vector3(Random.Range(-.5f, .5f), Random.Range(1, 1.25f), 0);
            this.wolfSprite.sprite = this.barkSprite;
        }
        else
        {
            this.wolfSprite.transform.localPosition = new Vector3(0, .15f, 0);
            this.barkTransform.gameObject.SetActive(false);
            this.wolfSprite.sprite = this.normalSprite;
        }


    }

    public void EnableControls()
    {
        CancelInvoke("Bark");
        this.wolfSprite.sprite = this.normalSprite;
        this.wolfSprite.transform.localPosition = new Vector3(0, .15f, 0);
        this._jumpToggle = false;

        this.barkTransform.gameObject.SetActive(false);
        this._animal.controlsActive = true;
        this._barkAction = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!this._eating && !this._barkAction && GameManager.instance.gameActive)
        {
            this._eating = true;
            this._eatingTimer = 1.5f;

            this.wolfSprite.sprite = this.mouthFullSprite;

            if (collision.gameObject.tag == "Edible")
            {
                AudioManager.instance.PlaySound("Eat");

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

            if (this._eating)
            {
                Ram ram = collision.gameObject.GetComponent<Ram>();
                
                if (ram && GameManager.instance.gameActive)
                {
                        OnGetHit(ram);
                }
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
        if (this._invincible) return;

        ram.GetComponent<Sheep>().GetFrightened(4);
        ram.GetComponent<WanderSheep>().ClearBehaviour();

        this._health -= 25;
        this._invincible = true;

        AudioManager.instance.StopSound("Bark");
        AudioManager.instance.PlaySound("Whine");
        AudioManager.instance.PlaySound("Bonk");

        this._animal.moveUp = false;
        this._animal.moveDown = false;
        this._animal.moveLeft = false;
        this._animal.moveRight = false;

        this._animal.controlsActive = false;
        this._jumpToggle = false;
        this._barkAction = false;


        this.barkTransform.gameObject.SetActive(false);
        this.wolfSprite.transform.localPosition = new Vector3(0, .15f, 0);

        //Start blinking
        CancelInvoke("Bark");
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
        
        if (this._on)
        {
            if (this._eating)
            {
                this.wolfSprite.sprite = this.mouthFullSprite;
            }
            else
            {
                this.wolfSprite.sprite = this.normalSprite;
            }
        }
    }

    void EndBlinking()
    {
        this.wolfSprite.color = new Color(1, 1, 1);
        this._on = true;
 
       this.wolfSprite.enabled = true;
        this._invincible = false;
        CancelInvoke("Blink");
    }

    public void OnDied()
    {
        CancelInvoke("Blink");
        CancelInvoke("EndBlinking");
        CancelInvoke("EnableControls");
        this._on = true;
        this.wolfSprite.color = new Color(1, 1, 1);
        this._invincible = false;

        this._eating = false;
        this._animal.controlsActive = false;

        wolfSprite.sprite = deathSprite;
    }

}
