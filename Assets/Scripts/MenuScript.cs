using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Slider slider;
    //public int abc;
    public MouseScript mouse;

    public void Start()
    {
        slider.value = AudioListener.volume;
    }

    public void Continue()
    {
        mouse.OpenMenu();
    }
    public void AppQuit()
    {
        Application.Quit();
    }

    public void ValueChangeCheck()
    {
        AudioListener.volume = slider.value;
    }
}
