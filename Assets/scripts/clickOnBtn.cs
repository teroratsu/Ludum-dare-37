using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class clickOnBtn : MonoBehaviour {

    public UnityEvent methods;
    public GameManager manager;

    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
    }

    void OnMouseDown()
    {
        if (GameObject.Find("Scene Manager").GetComponent<SceneManager>().paused)
            manager.gameObject.GetComponent<spawnMeALilStack>().preventNextClickPls();
        methods.Invoke();
    }
}
