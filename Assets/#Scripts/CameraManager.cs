using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float AciveCamaera;


    public GameObject Cam1;
    public GameObject Cam2;
    public GameObject Cam3;
    public GameObject Cam4;

    void Start()
    {
        
    }

    
    void Update()
    {
        CamKeeper();
    }



    public void CamKeeper()
    {
        if(AciveCamaera == 1)
        {
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            Cam3.SetActive(false);
            Cam4.SetActive(false);
        }

        if (AciveCamaera == 2)
        {
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            Cam3.SetActive(false);
            Cam4.SetActive(false);
        }

        if (AciveCamaera == 3)
        {
            Cam1.SetActive(false);
            Cam2.SetActive(false);
            Cam3.SetActive(true);
            Cam4.SetActive(false);
        }

        if (AciveCamaera == 4)
        {
            Cam1.SetActive(false);
            Cam2.SetActive(false);
            Cam3.SetActive(false);
            Cam4.SetActive(true);
        }


        if (AciveCamaera == 5)
        {
            AciveCamaera = 1;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            AciveCamaera += 1;
        }
    }
}
