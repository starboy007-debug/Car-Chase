using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownScript : MonoBehaviour
{

    public GameObject Three;
    public GameObject Two;
    public GameObject One;
    public GameObject Go;

    public GameObject RaceUI;



    void Start()
    {
        StartCoroutine("StartRace");
    }



    IEnumerator StartRace()
    {
        yield return new WaitForSeconds(1);
        Three.SetActive(true);
        yield return new WaitForSeconds(1);
        Three.SetActive(false);
        Two.SetActive(true);       
        yield return new WaitForSeconds(1);
        Two.SetActive(false);
        One.SetActive(true);
        yield return new WaitForSeconds(1);
        Go.SetActive(true);
        One.SetActive(false);
        RaceUI.SetActive(true);
        Destroy(gameObject, 1);

    }

}
