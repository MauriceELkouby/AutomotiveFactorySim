using UnityEngine;
public class ServoDriveSim : MonoBehaviour
{
    private string industrialArm;
    public string plcO1;
    private string plcO2;
    private string plcI1;
    private string plcI2;
    private string armRotation;
    private bool lastPlcOState = false; // Stores previous plcO state
    private IndustrialArmController armController;
    private Coroutine pulseCoroutine;
    public bool pulse;
    public bool currentPlcOState = false;
    void Start()
    {
        
        industrialArm = GetHighestParentGameObjectName(gameObject.transform);
        plcO1 = "ns=2;s=SmartFactory.controlPlc." + industrialArm + "." + gameObject.name + "Pulse";  // OPC UA tag for PLC pulse
        plcO2 = "ns=2;s=SmartFactory.controlPlc." + industrialArm + "." + gameObject.name + "Sign";   // OPC UA tag for PLC Sign
        plcI1 = "ns=2;s=SmartFactory.controlPlc." + industrialArm + "." + gameObject.name + "A";      // OPC UA tag for PLC A
        plcI2 = "ns=2;s=SmartFactory.controlPlc." + industrialArm + "." + gameObject.name + "B";      // OPC UA tag for PLC B
        armRotation = gameObject.name + "Rotation";         // Industrial arm dictionary tag for rotation
        armController = GetComponentInParent<IndustrialArmController>();
    }
    private float pulseInterval = 0.1f;
    private float pulseTimer = 0f;
    private bool pulseInProgress = false;
    bool once = false;
    void FixedUpdate()
    {
        if (OpcUaClientBehaviour.connected == false) return;

        currentPlcOState = PLCIOS.Instance.GetTagValue(plcO1);

        if (!lastPlcOState && currentPlcOState && !pulseInProgress)
        {
            pulseInProgress = true;
            SendPulsesDirect(); // Call pulses immediately without coroutine
        }

        lastPlcOState = currentPlcOState;
    }
    void SendPulsesDirect()
    {
        Debug.Log("A");
        if (!PLCIOS.Instance.GetTagValue(plcO2))//forward
        {
            PLCIOS.Instance.SetTagValue(plcI1, true);
            PLCIOS.Instance.SetTagValue(plcI2, false);
        }
        else //backwards
        {
            PLCIOS.Instance.SetTagValue(plcI1, false);
            PLCIOS.Instance.SetTagValue(plcI2, true);
        }

        // Use Invoke or delayed execution to turn off pulses
        Invoke(nameof(ResetPulses), pulseInterval);
    }

    void ResetPulses()
    {
        Debug.Log("B");
        if (!PLCIOS.Instance.GetTagValue(plcO2))//forward
        {
            PLCIOS.Instance.SetTagValue(plcI1, false);
            PLCIOS.Instance.SetTagValue(plcI2, true);
        }
        else //backwards
        {
            PLCIOS.Instance.SetTagValue(plcI1, true);
            PLCIOS.Instance.SetTagValue(plcI2, false);
        }

        bool isForward = !PLCIOS.Instance.GetTagValue(plcO2);
        float incrementValue = isForward ? 5f : -5f;

        armController.UpdateRotation(armRotation, incrementValue);
        pulseInProgress = false;
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
