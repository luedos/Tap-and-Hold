using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    

    public static GameSettings Instance
    { get
        {
            return instance;
        }
    }

    public SO_Settings mySettings;

    private static GameSettings instance;

    // Use this for initialization
    void Start () {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        if (instance != this)
            Destroy(this);
	}
	
}
