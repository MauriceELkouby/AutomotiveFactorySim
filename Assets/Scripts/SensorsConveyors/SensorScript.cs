using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    private string plcI;
    void Start()
    {
        plcI = "ns=2;s=SmartFactory.controlPlc." + gameObject.name;      // OPC UA tag for PLC A
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wheel" || other.gameObject.tag == "Car")
        {
            PLCIOS.Instance.SetTagValueBool(plcI, true);
        }
        else { PLCIOS.Instance.SetTagValueBool(plcI, false); }
        
    }
    void OnTriggerExit(Collider other)
    {
        PLCIOS.Instance.SetTagValueBool(plcI, false);
    }

}
