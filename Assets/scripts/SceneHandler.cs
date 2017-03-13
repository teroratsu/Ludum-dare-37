using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour {

    //todo : tag mechanism, retore default, triggerAnim

    public bool seedCountInfinite = false;
    public int seedCount = 1;
    public List<GameObject> childs;
    public List<GameObject> mechanisms;
    public GameObject startPos; // spawnPos
    private GameManager gameManager;

    private bool ready = false;

	// Use this for initialization
	void Start () {
        for(int i =0; i< transform.childCount ; ++i)
        {
            childs.Add(transform.GetChild(i).gameObject);
            if(transform.GetChild(i).tag == "mechanism" || transform.GetChild(i).tag == "Floor")
                mechanisms.Add(transform.GetChild(i).gameObject);
        }
        gameManager =  GameObject.Find("GameController").GetComponent<GameManager>();
        ready = true;
    }

    public bool isReady()
    {
        return ready;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void load()
    {
        gameManager.gameObject.GetComponent<spawnMeALilStack>().prevent = true;
        foreach (GameObject child in childs)
        {
            if (child != null)
            child.GetComponent<AnimHandler>().activeElt(true);
        }
        foreach (GameObject mechanism in mechanisms)
        {
            triggerPlateform tr_t = mechanism.GetComponent<triggerPlateform>();
            movingBehaviour mv_t = mechanism.GetComponent<movingBehaviour>();
            if (tr_t) tr_t.restore();
            if (mv_t) mv_t.restore();
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("graines"))
        {
            DestroyImmediate(obj);
        }
        if (!gameManager.inMenu) gameManager.gameObject.GetComponent<spawnMeALilStack>().SetUp(seedCountInfinite, seedCount);
        gameManager.gameObject.GetComponent<spawnMeALilStack>().prevent = false;
    }

    public void unload()
    {
        if(gameObject.name == "Scene 4")
        {
            Debug.Log(gameObject.name + " - Bugs :\n");
            Debug.Log(childs.Count);
            foreach (GameObject child in childs)
            {
                Debug.Log(child.name);
                child.GetComponent<AnimHandler>().activeElt(false);
                movingBehaviour bhv = child.GetComponent<movingBehaviour>();
                if (bhv) bhv.disableMovement();
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("graines"))
            {
                DestroyImmediate(obj);
            }
        }
        else
        {
            foreach (GameObject child in childs)
            {
                child.GetComponent<AnimHandler>().activeElt(false);
                movingBehaviour bhv = child.GetComponent<movingBehaviour>();
                if (bhv) bhv.disableMovement();
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("graines"))
            {
                DestroyImmediate(obj);
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void restore()
    {
        gameManager.gameObject.GetComponent<spawnMeALilStack>().prevent = true;
        GameObject.Find("poule").GetComponent<PouleManager>().restore();
        foreach (GameObject mechanism in mechanisms)
        {
            triggerPlateform tr_t = mechanism.GetComponent<triggerPlateform>();
            movingBehaviour mv_t = mechanism.GetComponent<movingBehaviour>();
            if (tr_t) tr_t.restore();
            if (mv_t) mv_t.restore();
        }
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("graines"))
        {
            DestroyImmediate(obj);
        }
        gameManager.gameObject.GetComponent<spawnMeALilStack>().SetUp(seedCountInfinite, seedCount);
        GameObject.Find("poule").transform.position = startPos.transform.position;
        gameManager.gameObject.GetComponent<spawnMeALilStack>().prevent = false;
    }
}
