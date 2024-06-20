using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggleManager : MonoBehaviour
{
    public GameObject ToggleGroup;
    public Toggle EasyToggle;
    public Toggle MediumToggle;
    public Toggle HardToggle;

    void Start()
    {
        EasyToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
        MediumToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
        HardToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
    }

    public void ActiveToggle()
    {
        if (EasyToggle.isOn)
        {
            Debug.Log("Easy");
            ToggleStateManager.EasyToggleOn = true;
            ToggleStateManager.MediumToggleOn = false;
            ToggleStateManager.HardToggleOn = false;
        }
        else if (MediumToggle.isOn)
        {
            Debug.Log("Medium");
            ToggleStateManager.EasyToggleOn = false;
            ToggleStateManager.MediumToggleOn = true;
            ToggleStateManager.HardToggleOn = false;
        }
        else if (HardToggle.isOn)
        {
            Debug.Log("Hard");
            ToggleStateManager.EasyToggleOn = false;
            ToggleStateManager.MediumToggleOn = false;
            ToggleStateManager.HardToggleOn = true;
        }
    }

    private void ToggleValueChanged()
    {
        ActiveToggle();
    }

}
