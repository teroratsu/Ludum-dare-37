using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private SceneManager sceneManager;
    spawnMeALilStack spawnerController;

    public GameObject gui;
    private bool disableGUI = false;

    public bool inMenu = true;
    public bool unfortunate = false;

    public AudioSource winSeul;

    // Use this for initialization
    void Start () {
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        spawnerController = GetComponent<spawnMeALilStack>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Cancel") && !inMenu)
            sceneManager.pause();
	}

    public void DisableGUI(bool disable)
    {
        disableGUI = disable;
    }

    void FixedUpdate()
    {
        if(inMenu || disableGUI)
        {
            gui.SetActive(false);
            spawnerController.enabled = false;
        }
        else
        {
            if (!disableGUI)
            {
                gui.SetActive(true);
                spawnerController.enabled = true;
            }
            
        }

        if (spawnerController.seedCount == 0 && GameObject.FindGameObjectsWithTag("graines").Length == 0 && !inMenu && !sceneManager.isloading())
        {
            if(!unfortunate)
            {
                unfortunate = true;
                thatIsUnfortunate();
            }
            
        }
    }

    public void thatIsUnfortunate()
    {
        if (unfortunate && sceneManager && !inMenu)
        {
            sceneManager.gameOver();
            unfortunate = false;
        }
        if(inMenu && !sceneManager.paused && !sceneManager.isloading())
        {
            unfortunate = false;
        }
    }

    public void unfortunatePls()
    {
        unfortunate = true;
    }


    public void victory()
    {
        Debug.Log("you did it ! :D");
        winSeul.Play(1);
    }

    public void reTry()
    {
        if (inMenu) inMenu = false;
        sceneManager.reloadCurrentScene();
    }

    public void setinMenu(bool isinMenu)
    {
        inMenu = isinMenu;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
