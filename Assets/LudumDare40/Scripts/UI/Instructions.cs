using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {

    public GameObject titleScreen;

    public void ShowInstructions()
    {
        titleScreen.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void OnInstructionsComplete()
    {
        titleScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnInstructionsComplete();
        }
    }
}
