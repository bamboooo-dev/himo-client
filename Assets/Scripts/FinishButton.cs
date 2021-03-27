﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinishButton : MonoBehaviour
{

    public void OnClick() {
        RoomStatus.finished = true;
        SceneManager.LoadScene("Home");
    }
    
}
