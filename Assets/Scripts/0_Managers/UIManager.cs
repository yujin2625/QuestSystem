using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    [SerializeField] private List<ButtonDictionary> Buttons = new List<ButtonDictionary>();
    public Dictionary<string,Button> ButtonDictionary = new Dictionary<string, Button>();
    

    private void Start()
    {
        foreach(var button in Buttons)
        {
            ButtonDictionary.Add(button.key, button.value);
        }
    }


}

[Serializable]
public class ButtonDictionary
{
    public string key;
    public Button value;
}