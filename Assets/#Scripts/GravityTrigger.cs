using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;


public class GravityTrigger : MonoBehaviour
{
    public string ExitOrEnter;


    private void OnTriggerEnter(Collider other)
    {
        if(ExitOrEnter == "Enter")
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (ExitOrEnter == "Exit")
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Rigidbody>().useGravity = true;
            }
        }

    }


}
