using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneM : MonoBehaviour
{
    public DropDown dd;

    public void LoadPlayScene()
    {
        if (dd.value == 0) 
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (dd.value == 1) 
        {
            SceneManager.LoadScene("AI");
        }

    }
}
