using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    public RectTransform transitionScreen;
    public Image titleScreenImage;
    public GameObject playButton;

    private bool _transitionIn;
    private bool _transitionOut;

    [SerializeField]
    private float _transitionDuration = 3;
    private float _timeStart;
    private float _fadeTimeStart;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
        if (this._transitionOut)
        {
            transitionScreen.localPosition = Vector2.Lerp(Vector2.zero, new Vector2(0, 540), (Time.time - this._timeStart) / this._transitionDuration);

            if ((Time.time - this._timeStart) / this._transitionDuration > 1)
            {
                this._transitionIn = false;
                this._transitionOut = false;
                transitionScreen.localPosition = new Vector2(0, -540);
            }
        }
        else if (this._transitionIn)
        {
            transitionScreen.localPosition = Vector2.Lerp(new Vector2(0, -540), Vector2.zero, (Time.time - this._timeStart) / this._transitionDuration);

            if ((Time.time - this._timeStart)/ this._transitionDuration > 1)
            {
                this._transitionIn = false;
                this._transitionOut = true;


                GameManager.instance.ShowLevel();

                this._timeStart = Time.time;
            }
        }

        if (this._transitionIn || this._transitionOut )
        {
            titleScreenImage.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (Time.time - this._fadeTimeStart)/ (this._transitionDuration * 2) );
        }
	}

    public void ShowTitleScreen()
    {
        titleScreenImage.color = Color.white;
        playButton.SetActive(true);
    }

    public void StartTransition()
    {
        titleScreenImage.color = Color.white;
        transitionScreen.localPosition = new Vector2(0, -540);
        this._timeStart = Time.time;
        this._fadeTimeStart = Time.time;
        this._transitionIn = true;

        playButton.SetActive(false);
    }
}
