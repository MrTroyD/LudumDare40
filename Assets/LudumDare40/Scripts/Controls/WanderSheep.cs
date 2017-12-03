using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSheep : MonoBehaviour {

    private enum Behaviour
    {
        Wander,
        SearchForMate,
        TargetWolf,
        Frolick,
        Flee,
        Stand

    }


    public float maxWidth = 11;
    public SpriteRenderer sprite;

    Color _angryColor = new Color(1, .5f, .5f);//FFA4A4FF;
    
    Color _normalColor = new Color(1, 1, 1);

    [SerializeField]
    private float _behaviourChangeTimer;

    [SerializeField]
    private Behaviour _currentBevaiour;
    
    private Transform _target;

    private Sheep _sheep;
    private Animal _animal;

    private List<WhiskerNode> _destinations;


        
    public Transform target
    {
        get { return this._target; }
    }

	// Use this for initialization
	void Start () {
        this._animal = this.GetComponent<Animal>();
        this._sheep = this.GetComponent<Sheep>();
        if (this._destinations == null)
        {
            this._destinations = new List<WhiskerNode>(this.GetComponentsInChildren<WhiskerNode>());
            for (int i = 0, n = this._destinations.Count; i < n; i++)
            {
                this._destinations[i].rotation = ((float)i / (float)n) * 360;
                this._destinations[i].rayLength = maxWidth;
                this._destinations[i].UpdateNode();
            }
        }
        this.sprite = GetComponentInChildren<SpriteRenderer>();

        if (GetComponent<Lamb>()) this._currentBevaiour = Behaviour.Frolick;
    }

    void OnEnable()
    {
        this._animal = this.GetComponent<Animal>();
        this._sheep = this.GetComponent<Sheep>();

        if (GetComponent<Lamb>()) this._currentBevaiour = Behaviour.Frolick;
    }

    // Update is called once per frame
    void Update () {
        

        this.sprite.color = this._sheep.aggression > 3 ? this._angryColor : this._normalColor;

        if (this._currentBevaiour != Behaviour.Stand)
        {
            this.transform.Translate(this.transform.forward * Time.deltaTime * this._animal.movementSpeed);

            float xPos = this.transform.position.x;
            float yPos = this.transform.position.z;
            if (xPos > 11)
            {
                xPos = 11;
                this._behaviourChangeTimer *= .5f;
            }
            else if (xPos < -11)
            {
                xPos = -11;
                this._behaviourChangeTimer *= .5f;
            }

            if (yPos > 11)
            {
                yPos = 11;
                this._behaviourChangeTimer *= .5f;
            }
            else if (yPos < -11)
            {
                yPos = -11;
                this._behaviourChangeTimer *= .5f;
            }

            this.transform.position = new Vector3(xPos, this.transform.position.y, yPos);
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }



        if (_behaviourChangeTimer > 0)
        {
            this._behaviourChangeTimer -= Time.deltaTime;


            //Check to see if Wolf is around
            if (Wolf.instance && Vector3.Distance(this.transform.position, Wolf.instance.transform.position) < 5 && this._sheep.aggression < 1f && this._currentBevaiour != Behaviour.Flee)
            {
//                print("Wolf too close");
                this._behaviourChangeTimer = .5f;
                this.transform.LookAt(Wolf.instance.transform, Vector3.up);
                this.transform.rotation = Quaternion.Euler(0, -this.transform.rotation.eulerAngles.y, 0);
                this._currentBevaiour = Behaviour.Flee;

            }

            if (this._behaviourChangeTimer <= 0)
            {
                this._behaviourChangeTimer = 0;


                switch (this._currentBevaiour)
                {
                    case Behaviour.Frolick:
                        SetNewDirection();
                        this._behaviourChangeTimer = 1;
                        this._currentBevaiour = Behaviour.Stand;


                        break;

                    case Behaviour.Wander:

                        if (this._sheep.aggression > 3)
                        {
                            this._currentBevaiour = Behaviour.TargetWolf;
                            return;
                        }
                        SetNewDirection();

                        this._behaviourChangeTimer = 4;
                        break;

                    case Behaviour.Stand:

                        if (GetComponent<Lamb>())
                        {
                            SetNewDirection();
                            this._currentBevaiour = Behaviour.Frolick;
                            return;
                        }

                        if (this._sheep.mateRecoveryTime > 0)
                        {
                            this._currentBevaiour = Behaviour.Wander;
                            SetNewDirection();
                        }
                        else if (Random.Range(0, 1f) > .75f)
                        {
                            DoMateSearch();
                            this._currentBevaiour = Behaviour.SearchForMate;
                        }
                        this._behaviourChangeTimer = 3;

                        break;
                    case Behaviour.TargetWolf:
                        if (this._target != null)
                        {
                            this._behaviourChangeTimer = .25f;
                            FaceTarget();

                            if (this._sheep.aggression < 2.5f)
                            {
                                //Better relax before you get eaten
                                this._currentBevaiour = Behaviour.Stand;
                            }
                        }
                        break;
                    case Behaviour.SearchForMate:

                        if (this.GetComponent<Lamb>())
                        {
                            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            SetNewDirection();
                            print("How lamb!?");
                            this._behaviourChangeTimer = 1f;
                            this._currentBevaiour = Behaviour.Frolick;
                            return;
                        }

                        if (this._target != null)
                        {
                            this._behaviourChangeTimer = .25f;
                            FaceTarget();
                        }
                        else
                        {
                            this._behaviourChangeTimer = 2f;
                            this._currentBevaiour = Behaviour.Stand;
                            DoMateSearch();
                        }
                        break;

                    case Behaviour.Flee:
                        this._currentBevaiour = Behaviour.Stand;
                        this._behaviourChangeTimer = .15f;
                        break;

                }
            }


        }
         
        
        if (this._behaviourChangeTimer <= 0)
        {
            ClearBehaviour();
        }
	}

    public void StopLookingForMate()
    {
        this._behaviourChangeTimer *= .5f;
        this._currentBevaiour = Behaviour.Stand;
    }

    public void StartLookingForMate()
    {
        this._behaviourChangeTimer *= .5f;
        this._currentBevaiour = Behaviour.SearchForMate;
    }

    void DoMateSearch()
    {
        List<Sheep> mateList = new List<Sheep>();

        Sheep possiblePartner;
        if (this._sheep.sex == Sheep.Sex.Female)
        {
            foreach(Ram ram in SheepManager.instance.maleSheep)
            {
                possiblePartner = ram.GetComponent<Sheep>();
                if (possiblePartner.mateReady && Random.Range(0, 1f) < .9f)
                {
                    mateList.Add(possiblePartner);
                }
            }

        }
        else
        {
            foreach (Ewe ewe in SheepManager.instance.femaleSheep)
            {
                possiblePartner = ewe.GetComponent<Sheep>();
                if (possiblePartner.mateReady)
                {
                    mateList.Add(possiblePartner);
                }

            }
        }

        if (mateList.Count > 0)
        {
            float distance = 9999f;
            foreach(Sheep possibleMate in mateList)
            {

                if (this._target == null || Vector3.Distance(this.transform.position, possibleMate.transform.position) < distance)
                {
                    this._target = possibleMate.transform;
                }
            }

        }
        else
        {
            if (this._sheep.sex == Sheep.Sex.Male)
            {
                this._sheep.GetAngry(.25f);
            }
            this._currentBevaiour = Behaviour.SearchForMate;
        }
    }

    public void CheckAggression()
    {
        if (this._sheep.aggression > 3f)
        {
            this._target = Wolf.instance.transform;

        }
    }

    void FaceTarget()
    {
        transform.LookAt(this._target);
        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
    }

    public void SetNewDirection()
    {
        this._animal.moveUp = false;
        this._animal.moveDown = false;
        this._animal.moveLeft = false;
        this._animal.moveRight = false;

        if (this._destinations == null)
        {
            this._destinations = new List<WhiskerNode>(this.GetComponentsInChildren<WhiskerNode>());
            for (int i = 0, n = this._destinations.Count; i < n; i++)
            {
                this._destinations[i].rotation = ((float)i / (float)n) * 360;
                this._destinations[i].rayLength = maxWidth;
                this._destinations[i].UpdateNode();
            }
        }

        for (int i = 0; i < this._destinations.Count; i++)
        {
            this._destinations[i].UpdateNode();
        }


        this.transform.LookAt(this._destinations[Random.Range(0, this._destinations.Count)].transform);
        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        
    }

    public void ClearBehaviour()
    {
        this._currentBevaiour = Behaviour.Stand;
        this._behaviourChangeTimer = 2f;
    }

    public void Mating()
    {
        this._target = null;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this._currentBevaiour = Behaviour.Stand;
        this._behaviourChangeTimer = 1;
    }
}
