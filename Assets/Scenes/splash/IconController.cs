using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class IconController : MonoBehaviour
{   
    public Texture2D[] acceptButtons;
    public Texture2D[] cancelButtons;
    public int keyboardID = 0;
    public int psID = 1;
    public int genericID = 2;

    public Texture2D useAccept;
    public Texture2D useCancel;

    void Awake(){
        DontDestroyOnLoad(this);
        SaveDataLoaded data = GameObject.FindObjectOfType<SaveDataLoaded>();
        useAccept = acceptButtons[data.buttonIconID];
        useCancel = cancelButtons[data.buttonIconID];
    }

    public void UpdateIcon(){
        SaveDataLoaded data = GameObject.FindObjectOfType<SaveDataLoaded>();
        useAccept = acceptButtons[data.buttonIconID];
        useCancel = cancelButtons[data.buttonIconID];
    }
}
