using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerBar : MonoBehaviour {


    public Wolf wolfReference;
    private RectTransform _rectTransform;

    // Use this for initialization
    void Start()
    {
        this._rectTransform = GetComponent<RectTransform>();
        
    }

    private void OnEnable()
    {
        this._rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        this._rectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x, (this.wolfReference.hunger / 100) * 200);

    }
}
