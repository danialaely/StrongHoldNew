using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuToggleManager : MonoBehaviour
{
    public GameObject ToggleGroup;
    public Toggle EasyToggle;
    public Toggle MediumToggle;
    public Toggle HardToggle;

    public TMP_InputField userNameText;
    public GameObject PlayerNamePanel;
    public GameObject WelcomePanel;

    void Start()
    {
        EasyToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
        MediumToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
        HardToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });

        ActivateMyPanel(PlayerNamePanel.name);
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

    public void OnLoginClick()
    {
        string name = userNameText.text;
        if (!string.IsNullOrEmpty(name))
        {
            //Debug.Log(name);
            ActivateMyPanel(WelcomePanel.name);
        }
        else 
        {
            Debug.Log("Name field is empty");
        }
    }

    public void ActivateMyPanel(string panelName) 
    {
        PlayerNamePanel.SetActive(panelName.Equals(PlayerNamePanel.name));
        WelcomePanel.SetActive(panelName.Equals(WelcomePanel.name));
    }

    public string GetUserName() 
    {
        return userNameText.text;
    }

}
