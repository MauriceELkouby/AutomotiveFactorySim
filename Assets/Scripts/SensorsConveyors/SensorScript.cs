using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    private string plcI;
    public bool touched;
    void Start()
    {
        plcI = "ns=2;s=SmartFactory.controlPlc." + gameObject.name;      // OPC UA tag for PLC A
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Selectable")
        {
            touched = true;
            PLCIOS.Instance.SetTagValueBool(plcI, true);
        }
        else { touched = false; PLCIOS.Instance.SetTagValueBool(plcI, false); }
        
    }
    void OnTriggerExit(Collider other)
    {
        touched = false;
        PLCIOS.Instance.SetTagValueBool(plcI, false);
    }

}
