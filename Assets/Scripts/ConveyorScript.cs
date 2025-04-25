using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    private string plcO;
    void Start()
    {
        plcO = "ns=2;s=SmartFactory.controlPlc.Misc." + gameObject.name;      // OPC UA tag for PLC A
    }
    void Update()
    {
        if (!PLCIOS.Instance.GetTagValueBool(plcO))
        {
            transform.position = new Vector3(transform.position.x, 50,transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }

}
