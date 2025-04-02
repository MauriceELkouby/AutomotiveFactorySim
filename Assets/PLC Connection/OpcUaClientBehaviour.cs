// Copyright (c) Traeger Industry Components GmbH. All Rights Reserved.
using System;
using UnityEngine;
using UnityEngine.UI;
using Opc.UaFx;
using Opc.UaFx.Client;
using System.Collections.Generic;

public class OpcUaClientBehaviour : MonoBehaviour
{
    static public bool connected = false;
    private OpcClient client;
    public Text statusText;
    public Text ipText;
    /// <summary>
    /// This sample demonstrates how to implement a primitive OPC UA client in Unity.
    /// </summary>
    /// <remarks>Start is called before the first frame update.</remarks>
    private List<string> tagNodeIds = new List<string>();
    private string targetChannel = "SmartFactory";
    public void connect()
    {
        this.statusText.text = "Connecting...";

        try
        {
            // If already connected, unsubscribe from all current subscriptions
            if (client != null)
            {
                client.Disconnect();
                tagNodeIds.Clear(); // Clear the stored tag list
            }
            //opc.tcp://localhost:49320/
            //endpoint
            //this.client = new OpcClient(ipText.text);
            this.client = new OpcClient("opc.tcp://localhost:49320/");
            //connect to the server
            this.client.Connect();
            this.statusText.text = "Connected!";
            connected = true;
            // Auto-discover tags from Kepware
            BrowseServer();

            // Subscribe dynamically
            foreach (var tagNodeId in tagNodeIds)
            {
                client.SubscribeDataChange(tagNodeId, PLCIOS.Instance.HandleDataChanged);
            }

            statusText.text = $"Subscribed to {tagNodeIds.Count-3} tags from {targetChannel}!";
            Invoke("connected1", 5);
        }
        catch (Exception ex)
        {
            if (ex is TypeInitializationException tiex)
                ex = tiex.InnerException;

            this.statusText.text += Environment.NewLine
                    + ex.GetType().Name
                    + ": " + ex.Message
                    + Environment.NewLine
                    + ex.StackTrace;
        }
    }
    void connected1()
    {
        connected = true;
        Debug.Log("connection");
    }
    private void BrowseServer()
    {
        try
        {
            var rootNode = client.BrowseNode(OpcObjectTypes.ObjectsFolder);
            foreach (var node in rootNode.Children())
            {
                ExploreNode(node);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error browsing OPC UA server: {ex.Message}");
        }
    }
    private void ExploreNode(OpcNodeInfo node)
    {
        string nodePath = node.NodeId.ToString();

        // Allow only direct children of "SmartFactory.controlPlc"
        if (nodePath.StartsWith("ns=2;s=SmartFactory.controlPlc.") &&
            !nodePath.Contains("_Statistics") &&
            !nodePath.Contains("_System") &&
            node is OpcVariableNodeInfo)
        {
            tagNodeIds.Add(nodePath);
            Debug.Log($"Subscribed to tag: {nodePath}");
        }

        // Continue exploring only if the node is NOT a restricted folder
        if (!nodePath.Contains("_Statistics") && !nodePath.Contains("_System"))
        {
            foreach (var child in node.Children())
            {
                ExploreNode(child);
            }
        }
    }
    /// <summary>
    /// Sets a new value to an OPC tag and writes it to the server.
    /// </summary>
    public void SetOpcTagValueBool(string tagNodeId, bool newValue)
    {
        // Write the new value to the OPC server
        try
        {
            client.WriteNode(tagNodeId, newValue);
            client.ReadNode(tagNodeId);
            Debug.Log($"Tag {tagNodeId} set to {newValue}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write to OPC tag {tagNodeId}: {ex.Message}");
        }
    }
    public void SetOpcTagValueFloat(string tagNodeId, float newValue)
    {
        // Write the new value to the OPC server
        try
        {
            client.WriteNode(tagNodeId, newValue);
            client.ReadNode(tagNodeId);
            Debug.Log($"Tag {tagNodeId} set to {newValue}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write to OPC tag {tagNodeId}: {ex.Message}");
        }
    }
}



