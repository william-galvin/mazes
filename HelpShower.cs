using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpShower : MonoBehaviour
{
    public GameObject img;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        img.SetActive(active);
    }

    public void Change()
    {
        img.SetActive(!active);
        active = !active;
    }
}
