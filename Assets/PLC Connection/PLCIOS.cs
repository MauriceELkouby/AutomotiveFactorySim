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
    public Dictionary<string, bool> OpcTagValues = new Dictionary<string, bool>();

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
        // Convert the value to boolean
        bool value = Convert.ToBoolean(e.Item.Value.Value);

        // Get the tag ID
        string tag = e.MonitoredItem.NodeId.ToString();

        // Store value in dictionary (assuming the dictionary stores boolean values)
        OpcTagValues[tag] = value;

        // Optionally, log the change or perform additional actions_______________________________________________________________
        Debug.Log($"Tag {tag} changed to {value}");
    }

    /// <summary>
    /// Retrieves the latest value for a given OPC tag.
    /// </summary>
    public bool GetTagValue(string tagNodeId)
    {
        if (OpcTagValues.TryGetValue(tagNodeId, out bool value))
        {
            return value;
        }

        // If the tag is not found, return a default value or throw an exception
        Debug.LogWarning($"Tag {tagNodeId} not found in OpcTagValues dictionary");
        return false; // Default value if tag not found
    }
    /// <summary>
    /// Sets a new value to an OPC tag and writes it to the server.
    /// </summary>
    public void SetTagValue(string tagNodeId, bool newValue)
    {
        // Update dictionary value
        OpcTagValues[tagNodeId] = newValue;
        OpcUaClientBehaviour.SetOpcTagValue(tagNodeId, newValue);
    }
}
