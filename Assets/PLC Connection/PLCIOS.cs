using UnityEngine;
using System.Collections.Generic;
using Opc.UaFx.Client;
using System;

public class PLCIOS : MonoBehaviour
{
    public OpcUaClientBehaviour OpcUaClientBehaviour;
    // Singleton instance
    public static PLCIOS Instance;

    // Dictionary to store OPC tag values dynamically
    public Dictionary<string, float> OpcTagValuesFloat = new Dictionary<string, float>();
    public Dictionary<string, bool> OpcTagValuesBool = new Dictionary<string, bool>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Handles data changes from OPC UA server.
    /// </summary>
    public void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
    {
        // Get the tag ID
        string tag = e.MonitoredItem.NodeId.ToString();

        // Check the type of the value and handle accordingly
        if (e.Item.Value.Value is bool boolValue)
        {
            // Store the boolean value in the dictionary
            OpcTagValuesBool[tag] = boolValue;
        }
        else if (e.Item.Value.Value is float floatValue)
        {
            // Store the float value in the dictionary
            OpcTagValuesFloat[tag] = floatValue;
            Debug.Log("HELLO"+tag + OpcTagValuesFloat[tag]);
        }
    }
    /// <summary>
    /// Retrieves the latest value for a given OPC tag.
    /// </summary>
    public bool GetTagValueBool(string tagNodeId)
    {
        if (OpcTagValuesBool.TryGetValue(tagNodeId, out bool value))
        {
            return value;
        }

        // If the tag is not found, return a default value or throw an exception
        Debug.LogWarning($"Tag {tagNodeId} not found in OpcTagValues dictionary");
        return false; // Default value if tag not found
    }
    public float GetTagValueFloat(string tagNodeId)
    {
        if (OpcTagValuesFloat.TryGetValue(tagNodeId, out float value))
        {
            return value;
        }

        // If the tag is not found, return a default value or throw an exception
        Debug.LogWarning($"Tag {tagNodeId} not found in OpcTagValues dictionary");
        return 0; // Default value if tag not found
    }
    /// <summary>
    /// Sets a new value to an OPC tag and writes it to the server.
    /// </summary>
    public void SetTagValueBool(string tagNodeId, bool newValue)
    {
        // Update dictionary value
        OpcTagValuesBool[tagNodeId] = newValue;
        OpcUaClientBehaviour.SetOpcTagValueBool(tagNodeId, newValue);
    }
    public void SetTagValueFloat(string tagNodeId, float newValue)
    {
        // Update dictionary value
        OpcTagValuesFloat[tagNodeId] = newValue;
        OpcUaClientBehaviour.SetOpcTagValueFloat(tagNodeId, newValue);
    }
}
