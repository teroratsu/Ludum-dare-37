using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class clickOnBtn : MonoBehaviour {

    public UnityEvent methods;
    public GameManager manager;
    public static AudioSource mouseover;

    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        mouseover = GameObject.Find("Over_Button").GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (GameObject.Find("Scene Manager").GetComponent<SceneManager>().paused)
            manager.gameObject.GetComponent<spawnMeALilStack>().preventNextClickPls();
        methods.Invoke();
    }

    void OnMouseEnter()
    {
        mouseover.Play();
    }
}
