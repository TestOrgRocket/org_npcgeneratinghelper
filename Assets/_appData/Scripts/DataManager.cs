using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static List<NPC> npcs = new List<NPC>();
    public static List<NPC> village = new List<NPC>();
    static string npcsDataFileName = "npcsData.json";
    static string villageFileName = "villageData.json";

    public static void SaveVillageData(List<NPC> newVillage)
    {
        NPCList wrapper = new NPCList();
        wrapper.npcList = newVillage.ToArray();
        string jsonData = JsonUtility.ToJson(wrapper, true);
        string filePath = Path.Combine(Application.persistentDataPath, villageFileName);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Village data saved to: " + filePath);
    }
    public static List<NPC> LoadVillageData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, villageFileName);
        village = new List<NPC>();
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            NPCList wrapper = JsonUtility.FromJson<NPCList>(jsonData);
            village = new List<NPC>(wrapper.npcList);
            Debug.Log("Village data loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No Village data file found at: " + filePath);
        }
        return village;
    }
    public static void DeleteExistingVillage()
    {
        string filePath = Path.Combine(Application.persistentDataPath, villageFileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Village data deleted from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No Village data file found at: " + filePath);
        }
        village = new List<NPC>();
    }
    public static void DeleteNPCData(NPC npcToDelete)
    {
        npcs.Remove(npcToDelete);
        SaveNpcData();
    }
    public static void SaveNpcData()
    {
        NPCList wrapper = new NPCList();
        wrapper.npcList = npcs.ToArray();
        string jsonData = JsonUtility.ToJson(wrapper, true);
        string filePath = Path.Combine(Application.persistentDataPath, npcsDataFileName);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("NPC data saved to: " + filePath);
    }
    public static void LoadNpcData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, npcsDataFileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            NPCList wrapper = JsonUtility.FromJson<NPCList>(jsonData);
            npcs = new List<NPC>(wrapper.npcList);
            Debug.Log("NPC data loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No NPC data file found at: " + filePath);
        }

    }
    public static void CreateNPC(NPC newNpc)
    {
        npcs.Add(newNpc);
        SaveNpcData();
    }
}