using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolVacuum : MonoBehaviour
{
    public GameObject frontDoorLeft;
    public GameObject frontDoorRight;
    public GameObject backDoorLeft;
    public GameObject backDoorRight;
    private string industrialArm;
    private string plcO;
    private string currentObject;
    void Start()
    {
        industrialArm = GetHighestParentGameObjectName(gameObject.transform);
        plcO = "ns=2;s=SmartFactory.controlPlc." + industrialArm + "." + gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PLCIOS.Instance.GetTagValueBool(plcO))
        {
            frontDoorLeft.SetActive(false);
            backDoorLeft.SetActive(false);
            frontDoorRight.SetActive(false);
            backDoorRight.SetActive(false);
        }
        //Tool check
        if (industrialArm == "IndustrialArm5")
        {
            if (currentObject == "front")
            {
                Debug.Log("setActive");
                frontDoorLeft.SetActive(PLCIOS.Instance.GetTagValueBool(plcO) ? true : false);
            }
            else if (currentObject == "back")
            {
                backDoorLeft.SetActive(PLCIOS.Instance.GetTagValueBool(plcO) ? true : false);
            }

        }
        if (industrialArm == "IndustrialArm7")
        {
            if (currentObject == "front")
            {
                Debug.Log("setActive");
                frontDoorRight.SetActive(PLCIOS.Instance.GetTagValueBool(plcO) ? true : false);
            }
            else if (currentObject == "back")
            {
                backDoorRight.SetActive(PLCIOS.Instance.GetTagValueBool(plcO) ? true : false);
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("leftFront") || other.CompareTag("rightFront"))
        {
            Debug.Log(industrialArm);
            currentObject = "front";
            Debug.Log("front");
        }
        else if (other.CompareTag("leftBack") || other.CompareTag("rightBack"))
        {
            currentObject = "back";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("leftFront") || other.CompareTag("rightFront") || other.CompareTag("leftBack") || other.CompareTag("rightBack"))
        {
            currentObject = "";
        }
    }
    string GetHighestParentGameObjectName(Transform child)
    {
        while (child.parent != null)
        {
            child = child.parent;
        }
        return child.gameObject.name;
    }
}
