using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;


public class Boosters : MonoBehaviour
{

    public float BoostActive;

    public float BoostFuel;

    public Slider BoostSlider;

    public float thrust = 1.0f;
    public Rigidbody rb;

    public GameObject Booster;

    public Camera MainCam;

    void Start()
    {

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(transform.forward * thrust * Time.deltaTime);
        }

    }


    void Update()
    {

        BoostSlider.value = BoostFuel;


        if(BoostActive == 0)
        {
            BoostFuel += 5 * Time.deltaTime;


        }


        if (BoostActive == 1)
        {
            BoostFuel -= 10 * Time.deltaTime;
            MainCam.fieldOfView += Time.deltaTime;
        }

        if (BoostFuel > 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                BoostActive = 1;
                Booster.SetActive(true);

            }
        }


        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            BoostActive = 0;
            Booster.SetActive(false);
            MainCam.fieldOfView = 70;
        }
    }
}