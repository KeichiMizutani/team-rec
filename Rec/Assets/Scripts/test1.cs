﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SystemSoundManager testSE1 = GetComponent<SystemSoundManager>();
        //testSE1.PlaySystemSound("zunou");
        MusicManager.Instance.PlayBGM("ashita");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MusicManager.Instance.PlayBGM("tugi");
        }
    }
}
