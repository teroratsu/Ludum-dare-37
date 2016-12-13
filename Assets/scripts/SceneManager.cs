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

    public bool paused = false;
    private bool loading = false;

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
            transform.GetChild(i).gameObject.GetComponent<SceneHandler>().unload();
        }
        gameOverScene.GetComponent<AnimHandler>().activeElt(false);
        victoryScene.GetComponent<AnimHandler>().activeElt(false);
    }

    public bool isloading()
    {
        return loading;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        scenes[currentSceneID].GetComponent<SceneHandler>().unload();
        ++currentSceneID;
        Debug.Log("you did it !");
        if (currentSceneID >= transform.childCount)
        {
            gameManager.victory();
        }
        else
        {
            scenes[currentSceneID].GetComponent<SceneHandler>().load();
            GameObject.Find("poule").transform.position = scenes[currentSceneID].GetComponent<SceneHandler>().startPos.transform.position;
            GameObject.Find("poule").GetComponent<PouleManager>().restore();
            GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update () {

        if(loading){
            e_t += Time.deltaTime;
            if(e_t - startTime > loadDelay)
            {
                if (paused)
                {
                    pauseScene.SetActive(false);
                    Time.timeScale = 1;
                    AudioListener.volume = 1F;
                    paused = false;
                }
                GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = true;
                scenes[currentSceneID].GetComponent<SceneHandler>().unload();
                ++currentSceneID;
                scenes[currentSceneID].GetComponent<SceneHandler>().load();
                GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = false;
                reloadCurrentScene();
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
        AudioListener.volume = 0.2F;
        gameManager.setinMenu(true);
        pauseScene.SetActive(true);
        paused = true;
    }

    public void resumePause()
    {
        GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = false;
        Time.timeScale = 1;
        AudioListener.volume = 1F;
        gameManager.setinMenu(false);
        pauseScene.SetActive(false);
        paused = false;
    }

    public void backToMenu()
    {
        scenes[currentSceneID].GetComponent<SceneHandler>().restore();
        scenes[currentSceneID].GetComponent<SceneHandler>().unload();
        gameManager.setinMenu(true);
        currentSceneID = 0;
        if (paused)
        {
            pauseScene.SetActive(false);
            Time.timeScale = 1;
            AudioListener.volume = 1F;
            paused = false;
        }
        menuScene.GetComponent<SceneHandler>().load();
    }

    public void startGame()
    {
        gameManager.setinMenu(false);
        menuScene.GetComponent<SceneHandler>().unload();
        currentSceneID = 0;
        GameObject.Find("poule").transform.position = scenes[currentSceneID].GetComponent<SceneHandler>().startPos.transform.position;
        GameObject.Find("poule").GetComponent<PouleManager>().restore();
        GameObject.Find("poule").GetComponent<Rigidbody2D>().isKinematic = true;
        scenes[currentSceneID].GetComponent<SceneHandler>().load();
    }

    public void gameOver()
    {
        gameManager.setinMenu(true);
        gameOverScene.GetComponent<AnimHandler>().activeElt(true);
    }

    public void victory()
    {
        gameManager.setinMenu(true);
        victoryScene.GetComponent<AnimHandler>().activeElt(true);
    }

    public void reloadCurrentScene()
    {
        scenes[currentSceneID].GetComponent<SceneHandler>().restore();
    }

    public void restore()
    {
        scenes[currentSceneID].GetComponent<SceneHandler>().unload();
        currentSceneID = 0;
        scenes[currentSceneID].GetComponent<SceneHandler>().load();
    }
}
