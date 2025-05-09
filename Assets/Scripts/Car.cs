using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] GameObject wheelLeftFrount;
    [SerializeField] GameObject wheelLeftBack;
    [SerializeField] GameObject wheelRightFrount;
    [SerializeField] GameObject wheelRightBack;
    [SerializeField] GameObject doorLeftFrount;
    [SerializeField] GameObject doorLeftBack;
    [SerializeField] GameObject doorRightFrount;
    [SerializeField] GameObject doorRightBack;
    string arm1Tool = "ns=2;s=SmartFactory.controlPlc.IndustrialArm1.Tool";
    string arm3Tool = "ns=2;s=SmartFactory.controlPlc.IndustrialArm3.Tool";
    string arm5Tool = "ns=2;s=SmartFactory.controlPlc.IndustrialArm1.Tool";
    string arm7Tool = "ns=2;s=SmartFactory.controlPlc.IndustrialArm3.Tool";
    [SerializeField] GameObject colliderFront;
    [SerializeField] GameObject colliderBack;
    void Start()
    {
        SetAllPartsActive(false);
    }

    void SetAllPartsActive(bool state)
    {
        wheelLeftFrount.SetActive(state);
        wheelLeftBack.SetActive(state);
        wheelRightFrount.SetActive(state);
        wheelRightBack.SetActive(state);
        doorLeftFrount.SetActive(state);
        doorLeftBack.SetActive(state);
        doorRightFrount.SetActive(state);
        doorRightBack.SetActive(state);
    }
    IEnumerator DelayedActivation(string rootName, bool isFront)
    {
        yield return new WaitForSeconds(3f); // Delay in seconds
        ActivatePartBasedOnRoot(rootName, isFront);
    }
    void ActivatePartBasedOnRoot(string rootName, bool isFront)
    {
        switch (rootName)
        {
            case "IndustrialArm1":
                if (isFront) wheelLeftFrount.SetActive(true);
                else wheelLeftBack.SetActive(true);
                break;
            case "IndustrialArm3":
                if (isFront) wheelRightFrount.SetActive(true);
                else wheelRightBack.SetActive(true);
                break;
            case "IndustrialArm5":
                if (PLCIOS.Instance.GetTagValueBool("ns=2;s=SmartFactory.controlPlc.IndustrialArm5.Tool"))
                {
                    if (isFront) doorLeftFrount.SetActive(true);
                    else doorLeftBack.SetActive(true);
                }
                break;
            case "IndustrialArm7":
                if (PLCIOS.Instance.GetTagValueBool("ns=2;s=SmartFactory.controlPlc.IndustrialArm7.Tool"))
                {
                    if (isFront) doorRightFrount.SetActive(true);
                    else doorRightBack.SetActive(true);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor") == false) return;

        Debug.Log("Car sensed object");

        // Find the sensor (trigger) that was hit by the object
        Collider thisCollider = other; // This is the incoming collider (robot arm)
        Collider triggeredBy = null;

        // Go through all the car's own colliders and check if this one was hit
        Collider[] myColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in myColliders)
        {
            if (col.isTrigger && col.bounds.Intersects(thisCollider.bounds))
            {
                triggeredBy = col;
                break;
            }
        }

        if (triggeredBy == null) return;

        // Determine if it's front or back
        bool isFront = triggeredBy.gameObject == colliderFront;
        bool isBack = triggeredBy.gameObject == colliderBack;

        // Traverse to top parent of the object entering the trigger
        Transform root = other.transform;
        while (root.parent != null)
        {
            root = root.parent;
        }

        string rootName = root.name;
        Debug.Log("Root: " + rootName);

        if (isFront)
        {
            Debug.Log("Front sensor triggered");
            StartCoroutine(DelayedActivation(rootName, true));
        }
        else if (isBack)
        {
            Debug.Log("Back sensor triggered");
            StartCoroutine(DelayedActivation(rootName, false));
        }
    }

}

