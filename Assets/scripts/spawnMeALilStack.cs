using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnMeALilStack : MonoBehaviour {
    public GameObject stack;
    UnityEngine.UI.Text seedCounter;

    public int seedCount = 5;
    public bool infinite = true;
    public bool prevent = false;
    private bool preventNextClick = false;

    void Start()
    {
        seedCounter = GameObject.Find("seedCounter").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && seedCount > 0 && !prevent)
        {
            if(preventNextClick)
            {
                preventNextClick = false;
            }
            else
            {            
                Vector3 v3 = Input.mousePosition;
                v3.z = 0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                Instantiate(stack, v3, Quaternion.identity);
                if(!infinite) --seedCount;
            }
        }
    }
    void FixedUpdate()
    {
        seedCounter.text = (!infinite) ? seedCount.ToString() : "∞";
    }

    public void SetUp(bool inf, int count)
    {
        infinite = inf;
        seedCount = count;
    }

    public void preventNextClickPls()
    {
        preventNextClick = true;
    }
}
