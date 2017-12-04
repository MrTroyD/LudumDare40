using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : MonoBehaviour {

    public float matingRecoveryTime = 3;
    Sheep _sheep;

    public bool mateReady;

    private SpriteRenderer _spriteRenderer;

    public Sprite normalSprite;
    public Sprite angrySprite;
    
    // Use this for initialization
    void Start()
    {
        this._sheep = GetComponent<Sheep>();
        this._spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        if (this._sheep.aggression > 3)
        {
            this._spriteRenderer.sprite = angrySprite;
        }
        else
        {
            this._spriteRenderer.sprite = normalSprite;
        }

        if (Wolf.instance && Wolf.instance.eating && Vector3.Distance(Wolf.instance.transform.position, this.transform.position) < 2)
        {
            this._sheep.GetAngry(Time.deltaTime * .1f);
        }
    }

    public void Mating()
    {
        //Tell behaviour to drop aggression by half
        //this.aggression *= .5f;
        this._sheep.OnMating(matingRecoveryTime);
               
    }
}
