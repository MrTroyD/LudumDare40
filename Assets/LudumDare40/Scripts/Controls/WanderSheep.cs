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
        Stand

    }

    public float maxWidth = 11;

    [SerializeField]
    private float _behaviourChangeTimer;

    [SerializeField]
    private Behaviour _currentBevaiour;

    [SerializeField]
    private Transform _target;

    private Sheep _sheep;
    private Animal _animal;

    [SerializeField]
    private List<WhiskerNode> _destinations;
        
	// Use this for initialization
	void Start () {
        this._animal = this.GetComponent<Animal>();
        this._sheep = this.GetComponent<Sheep>();
        this._destinations = new List<WhiskerNode>(this.GetComponentsInChildren<WhiskerNode>()); 
        for (int i = 0, n = this._destinations.Count; i < n; i++)
        {
            this._destinations[i].rotation = ((float)i / (float)n) * 360;
            this._destinations[i].rayLength = maxWidth;
            this._destinations[i].UpdateNode();
        }
    }

    void OnEnable()
    {
        this._animal = this.GetComponent<Animal>();
        this._sheep = this.GetComponent<Sheep>();

        if (GetComponent<Lamb>()) this._currentBevaiour = Behaviour.Frolick;
    }

    // Update is called once per frame
    void Update () {
        
        if (this._currentBevaiour != Behaviour.Stand)
        {
            this.transform.Translate(this.transform.forward * Time.deltaTime);

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

        if (_behaviourChangeTimer > 0)
        {
            this._behaviourChangeTimer -= Time.deltaTime;

            if (this._behaviourChangeTimer <= 0)
            {
                this._behaviourChangeTimer = 0;

                switch (this._currentBevaiour)
                {
                    case Behaviour.Frolick:
                        this._behaviourChangeTimer = 2;
                        this._currentBevaiour = Behaviour.Stand;
                        break;

                    case Behaviour.Wander:
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
                        if (this._target != null)
                        {
                            this._behaviourChangeTimer = .25f;
                            FaceTarget();
                        }
                        else
                        {
                            this._behaviourChangeTimer = 2f;
                            DoMateSearch();
                            this._currentBevaiour = Behaviour.Stand;
                        }
                        break;

                }
            }


        }
         
        
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

    void SetNewDirection()
    {
        this._animal.moveUp = false;
        this._animal.moveDown = false;
        this._animal.moveLeft = false;
        this._animal.moveRight = false;

        for (int i = 0; i < this._destinations.Count; i++)
        {
            this._destinations[i].UpdateNode();
        }


        this.transform.LookAt(this._destinations[Random.Range(0, this._destinations.Count)].transform);
        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        
    }

    public void Mating()
    {
        this._target = null;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this._currentBevaiour = Behaviour.Stand;
        this._behaviourChangeTimer = 1;
    }
}
