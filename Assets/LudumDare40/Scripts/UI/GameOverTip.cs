using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTip : MonoBehaviour {

    public string[] hungerTips;
    public string[] healthTips;

    [SerializeField]
    private UnityEngine.UI.Text _uiText;


    Color _lerpColor = new Color(0, 0, 0, 0);
    float _lerpStartTime;
    float _alphaLerp;
    bool _animating;

    public string text
    {
        get { return this._uiText.text; }
        set { this._uiText.text = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (this._animating)
        {
            float currentLerpTime = (Time.time - this._lerpStartTime) / this._lerpStartTime + 1;
            if (currentLerpTime > 1)
            {
                currentLerpTime = 1;
                this._animating = false;
            }

            this.transform.localPosition = Vector2.Lerp(new Vector2(0,-64.0f), new Vector2(0, -96.79f), currentLerpTime);
            this._uiText.color = Color.Lerp( this._lerpColor, Color.black, currentLerpTime);

        }
	}

    public void SetHungerTips()
    {
        this._uiText.color = this._lerpColor;
        this.text = this.hungerTips[Random.Range(0, this.hungerTips.Length)];
    }

    public void SetHealthTips()
    {
        this._uiText.color = this._lerpColor;
        this.text = this.healthTips[Random.Range(0, this.healthTips.Length)];
    }

    public void ShowTip()
    {
        this._lerpStartTime = Time.time;
        this._animating = true;
    }
}
