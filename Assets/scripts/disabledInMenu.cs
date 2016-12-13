using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disabledInMenu : MonoBehaviour
{

    GameManager gameManager;
    public Light lt;

    private float intensity;

    void Start()
    {
        lt = GetComponent<Light>();
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
        intensity = lt.intensity;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        lt.intensity = (gameManager.inMenu) ? 0 : intensity;
    }
}
