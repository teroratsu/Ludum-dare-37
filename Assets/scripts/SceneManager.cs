using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public int currentSceneID;
    public List<GameObject> scenes;

    public GameObject pauseScene;
    public GameObject menuScene;
    public GameObject gameOverScene;
    public GameObject victoryScene;
    private GameManager gameManager;

    public AudioSource menuSnd;
    public AudioSource décorsSnd;
    public AudioSource startSnd;
    public AudioSource winSnd;
    public AudioSource musicBG;
    public AudioSource poulettAppearsSnd;

    public bool paused = false;
    private bool loading = false;

    private bool needToUnload = false;

    private float startTime;
    private float e_t;

    public float loadDelay = 4f;

    // Use this for initialization
    void Start () {
        currentSceneID = -1;
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            scenes.Add(transform.GetChild(i).gameObject);
            if (transform.GetChild(i).gameObject.GetComponent<SceneHandler>().isReady()) transform.GetChild(i).gameObject.GetComponent<SceneHandler>().unload();
            else needToUnload = true;
        }
        gameOverScene.GetComponent<AnimHandler>().activeElt(false);
        victoryScene.GetComponent<AnimHandler>().activeElt(false);
        menuSnd.Play();
    }

    public bool isloading()
    {
        return loading;
    }

    // Update is called once per frame
    void Update () {

        if(needToUnload)
        {
            needToUnload = false;
            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).gameObject.GetComponent<SceneHandler>().isReady()) transform.GetChild(i).gameObject.GetComponent<SceneHandler>().unload();
                else needToUnload = true;
            }
        }

        if(loading){
            e_t += Time.deltaTime;
            if (e_t - startTime > loadDelay)
            {
                if (paused)
                {
                    pauseScene.SetActive(false);
                    Time.timeScale = 1;
                    AudioListener.volume = 1F;
                    paused = false;
                    GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = false;
                }
                décorsSnd.Play();
                scenes[currentSceneID].GetComponent<SceneHandler>().unload();
                ++currentSceneID;
                scenes[currentSceneID].GetComponent<SceneHandler>().load();
                
                gameManager.setinMenu(false);
                loading = false;
            }
        }
        
    }

    public void loadNextScene()
    {
        if(!loading)
        {
            loading = true;
            startTime = Time.time;
            e_t = startTime;
        }
    }

    public void pause()
    {
        GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = true;
        Time.timeScale = 0;
        musicBG.Pause();
        menuSnd.UnPause();
        gameManager.setinMenu(true);
        pauseScene.SetActive(true);
        paused = true;
    }

    public void resumePause()
    {
        GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = false;
        Time.timeScale = 1;
        gameManager.setinMenu(false);
        pauseScene.SetActive(false);
        paused = false;
        musicBG.UnPause();
        menuSnd.Pause();
    }

    public void backToMenu()
    {
        musicBG.Stop();
        menuSnd.UnPause();
        scenes[currentSceneID].GetComponent<SceneHandler>().restore();
        scenes[currentSceneID].GetComponent<SceneHandler>().unload();
        gameManager.setinMenu(true);
        currentSceneID = 0;
        if (paused)
        {
            GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = false;
            pauseScene.SetActive(false);
            Time.timeScale = 1;
            AudioListener.volume = 1F;
            paused = false;
        }
        menuScene.GetComponent<SceneHandler>().load();
    }

    public void startGame()
    {
        gameManager.DisableGUI(false);
        gameManager.setinMenu(false);
        menuScene.GetComponent<SceneHandler>().unload();
        currentSceneID = 0;
        GameObject.Find("poule").transform.position = scenes[currentSceneID].GetComponent<SceneHandler>().startPos.transform.position;
        GameObject.Find("poule").GetComponent<PouleManager>().restore();
        scenes[currentSceneID].GetComponent<SceneHandler>().load();
        menuSnd.Pause();
        startSnd.Play();
        musicBG.Play();
        poulettAppearsSnd.Play();
    }

    public void gameOver()
    {
        gameManager.setinMenu(true);
        musicBG.Pause();
        menuSnd.UnPause();
        gameOverScene.GetComponent<AnimHandler>().activeElt(true);
    }

    public void victory()
    {
        gameManager.DisableGUI(true);
        victoryScene.SetActive(true);
        victoryScene.GetComponent<AnimHandler>().activeElt(true);
        
    }

    public void reloadCurrentScene()
    {
        scenes[currentSceneID].GetComponent<SceneHandler>().restore();
        musicBG.UnPause();
        menuSnd.Pause();
    }

    public void restore()
    {
        scenes[currentSceneID].GetComponent<SceneHandler>().unload();
        currentSceneID = 0;
        scenes[currentSceneID].GetComponent<SceneHandler>().load();
    }
}
