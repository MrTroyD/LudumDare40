using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public bool gameActive;
    public bool gameOver;

    public Image gameOverOverlay;
    public GameOverTip toolTip;
    public GameObject ui;

    public GameObject eatenOverlay;
    public Text eatenOverlayValue;

    private int _sheepEaten;

    [SerializeField]
    private GameObject _wolfObject;

    public int sheepEaten
    {
        get { return this._sheepEaten; }
        set {
            this._sheepEaten = value;
            this.eatenOverlayValue.text = this._sheepEaten.ToString();
        }
    }

	// Use this for initialization
	void Start () {

        if (GameManager.instance != null)
        {
            Debug.LogError("Error! GameManager already exits!");
            Destroy(this.gameObject);
            return;
        }

        GameManager.instance = this;

        DontDestroyOnLoad(this.gameObject);

        this.ui.SetActive(false);

        //Invoke("StartLevel", 1f);
	}

    public void ShowLevel()
    {
        SheepManager.instance.ClearSheep();
        this._wolfObject.SetActive(true);

        SheepManager.instance.CreateRandomSheep(4);
        Invoke("StartLevel", 1f);


    }

    private void StartLevel()
    {
        this.gameOverOverlay.gameObject.SetActive(false);
        this.gameOver = false;
        this.gameActive = true;

        AudioManager.instance.StopSoundtrack();

        AudioManager.instance.PlaySound("BackgroundTrack");
        Wolf.instance.OnStartBark();

        this.sheepEaten = 0;


        this.ui.SetActive(false);
    }


	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            //put up quit dialog box

            //for now just quit
            Application.Quit();
        }

        if (!gameOver && gameActive)
        {
            
            bool playerDied = false;
            float tipTimer = 0;
            if (Wolf.instance.health <= 0 && Wolf.instance.hunger > 0)
            {
                playerDied = true;
                AudioManager.instance.StopSoundtrack();
                AudioManager.instance.PlaySound("GameOver A");

                toolTip.SetHealthTips();

                tipTimer = 5f;
            }
            else if (Wolf.instance.health <= 0 && Wolf.instance.hunger <= 0)
            {
                playerDied = true;
                AudioManager.instance.StopSoundtrack();

                AudioManager.instance.PlaySound("GameOver B");

                toolTip.SetHungerTips();

                tipTimer = 9f;
            }

            if (playerDied)
            {
                this.ui.SetActive(false);

                gameOverOverlay.gameObject.SetActive(true);

                Invoke("ShowTip", tipTimer);

                Wolf.instance.OnDied();
                print("Player died!");
                gameOver = true;
                gameActive = false;
            }

        }
	}

    void ShowTip()
    {
        SheepManager.instance.DisableBreeding();
        toolTip.ShowTip();
    }
}
