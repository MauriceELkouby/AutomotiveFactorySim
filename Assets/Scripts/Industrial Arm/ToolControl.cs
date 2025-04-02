using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControl : MonoBehaviour
{
    public Animator anim;
    public GameObject tire;
    private string industrialArm;
    private string plcO;
    void Start()
    {
        industrialArm = GetHighestParentGameObjectName(gameObject.transform);
        plcO = "ns=2;s=SmartFactory.controlPlc." + industrialArm + "." + gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        //Tool check
        tire.SetActive(PLCIOS.Instance.GetTagValueBool(plcO) ? true : false);
        anim.SetBool("Tool", PLCIOS.Instance.GetTagValueBool(plcO) ? true:false);
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
