using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServoDriveSim : MonoBehaviour
{
    [DllImport("Dll1")]
    private static extern void ProcessServoInputs(
        int[] outputPulse, bool[] inputPulses, float[] inputsVoltage, int[] outputSigns, float[] lastPositions, int[] remainingPulses
    );

    private string industrialArm;
    public string[] plcO = new string[5];
    public string[] plcI = new string[5];
    private string[] armRotation = new string[5];
    private IndustrialArmController armController;
    private int[] outputPulse = new int[5];
    private bool[] inputPulses = new bool[5];
    private float[] inputsVoltage = new float[5];
    private int[] outputSigns = new int[5];
    public float[] lastPositions = new float[5];   // Store last known positions
    private int[] remainingPulses = new int[5];     // Store remaining pulses

    void Start()
    {
        industrialArm = GetHighestParentGameObjectName(transform);
        armController = GetComponentInParent<IndustrialArmController>();

        for (int i = 0; i < 5; i++)
        {
            plcO[i] = $"ns=2;s=SmartFactory.controlPlc.{industrialArm}.Joint{i + 1}OutputVolts";
            plcI[i] = $"ns=2;s=SmartFactory.controlPlc.{industrialArm}.Joint{i + 1}InputVolts";
            armRotation[i] = $"Joint{i + 1}Rotation";
            lastPositions[i] = 0f;
            remainingPulses[i] = 0;
        }
        StartCoroutine(ServoInputRoutine());
    }
    public float updateInterval = 0.02f; // Update every 20ms (50Hz)    
    IEnumerator ServoInputRoutine()
    {
        while (true)
        {
            // Wait for the specified interval before processing servo inputs again
            yield return new WaitForSeconds(updateInterval);

            if (!OpcUaClientBehaviour.connected) continue;
            
            // Read voltage inputs from the PLC
            for (int i = 0; i < 5; i++)
            {
                if (lastPositions[i] == inputsVoltage[i] && PLCIOS.Instance.GetTagValueFloat(plcI[i]) != inputsVoltage[i])
                {
                    PLCIOS.Instance.SetTagValueFloat(plcI[i], lastPositions[i]);
                }
                inputsVoltage[i] = PLCIOS.Instance.GetTagValueFloat(plcO[i]);
            }
            Debug.Log("Calling ProcessServoInputs...");
            ProcessServoInputs(outputPulse, inputPulses, inputsVoltage, outputSigns, lastPositions, remainingPulses);
            Debug.Log("ProcessServoInputs completed.");
            for (int i = 0; i < 5; i++)
            {
                Debug.Log($"outputPulse[{i}]: {outputPulse[i]}, outputSigns[{i}]: {outputSigns[i]}");
                inputPulses[i] = outputPulse[i] == 1 ? true : false;
                if (outputPulse[i] == 1)
                {
                    // Toggle pulses
                    lastPositions[i] += inputPulses[i] ? (outputSigns[i] == 0 ? (1f / 36f) : -(1f / 36f)) : 0f;
                    lastPositions[i] = Mathf.Round(lastPositions[i] * 1_000_000f) / 1_000_000f;
                    // Simulate encoder feedback
                    Debug.Log($"Calling UpdateServoPosition({i})...");
                    UpdateServoPosition(i);
                }
            }
            

            

        }
    }

    void UpdateServoPosition(int index)
    {
        int isForward = outputSigns[index];
        float incrementValue = isForward == 0 ? 1f : -1f;
        armController.UpdateRotation(armRotation[index], incrementValue);
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