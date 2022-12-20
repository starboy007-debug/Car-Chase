using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class MPHText : MonoBehaviour
{
    public CarControllerr Car;
    public Text MPH;
 
    private void Update()
    {
        MPH.text = Car.CurrentSpeed.ToString("0") + "mph";
    }
}
