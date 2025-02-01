using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ButtonIcon : MonoBehaviour
{
    public UseButtonIcon buttonIs;
    void Start()
    {
        IconController icon = GameObject.FindObjectOfType<IconController>();
        if(buttonIs == UseButtonIcon.Accept) GetComponent<RawImage>().texture = icon.useAccept;
        if(buttonIs == UseButtonIcon.Cancel) GetComponent<RawImage>().texture = icon.useCancel;
    }
}
public enum UseButtonIcon {
    Accept,
    Cancel
}
