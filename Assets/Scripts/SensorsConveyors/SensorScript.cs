using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    private string plcI;
    public bool touched;
    void Start()
    {
        plcI = "ns=2;s=SmartFactory.controlPlc.Misc." + gameObject.name;      // OPC UA tag for PLC A
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "Object" || other.gameObject.tag == "leftFront" || other.gameObject.tag == "leftBack" || other.gameObject.tag == "rightFront" || other.gameObject.tag == "rightBack")
        {
            touched = true;
            PLCIOS.Instance.SetTagValueBool(plcI, true);
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "Object" || other.gameObject.tag == "leftFront" || other.gameObject.tag == "leftBack" || other.gameObject.tag == "rightFront" || other.gameObject.tag == "rightBack")
        {
            touched = false;
            PLCIOS.Instance.SetTagValueBool(plcI, false);
        }
    }

}
