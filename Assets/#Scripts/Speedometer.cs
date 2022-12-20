using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;


public class Speedometer : MonoBehaviour
{
    public Text MPHText;
    public GameObject Needle;
    private float startPosistion = 220f, endPosition = -41f;
    private float desiredPostion;

    public float vehicleSpeed;

    public CarControllerr Car;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var speed = Car.CurrentSpeed;

        MPHText.text = speed.ToString("f0");
        vehicleSpeed = speed;
        UpdateNeedle();
    }


    public void UpdateNeedle()
    {
        desiredPostion = startPosistion - endPosition;
        float temp = vehicleSpeed / 180;
        Needle.transform.eulerAngles = new Vector3(0,0,(startPosistion - temp * desiredPostion));
    }
}
