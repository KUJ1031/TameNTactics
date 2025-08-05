using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class KeyValue
{
    public string key;
    public bool value;
}

[System.Serializable]
public class RayTriggerSaveData
{
    public List<KeyValue> npcRayStates = new List<KeyValue>();
}
public static class DialogueSaveSystem
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "ray_trigger_save.json");
    private static RayTriggerSaveData dataCache;

    private static Dictionary<string, bool> _npcRayStatesDict = new Dictionary<string, bool>();

    static DialogueSaveSystem()
    {
        LoadAll();
    }

    public static void SaveRayTriggerState(string id, bool triggered)
    {
        _npcRayStatesDict[id] = triggered;
        SaveAll();
    }

    public static bool LoadRayTriggerState(string id)
    {
        if (_npcRayStatesDict.TryGetValue(id, out bool triggered))
            return triggered;
        return false;
    }

    private static void SaveAll()
    {
        // Dictionary를 List로 변환하여 저장
        dataCache.npcRayStates.Clear();
        foreach (var kvp in _npcRayStatesDict)
        {
            dataCache.npcRayStates.Add(new KeyValue { key = kvp.Key, value = kvp.Value });
        }

        string json = JsonUtility.ToJson(dataCache, true);
        File.WriteAllText(savePath, json);
    }

    private static void LoadAll()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            dataCache = JsonUtility.FromJson<RayTriggerSaveData>(json);

            // List를 Dictionary로 변환
            _npcRayStatesDict.Clear();
            if (dataCache != null && dataCache.npcRayStates != null)
            {
                foreach (var kv in dataCache.npcRayStates)
                {
                    _npcRayStatesDict[kv.key] = kv.value;
                }
            }
        }
        else
        {
            dataCache = new RayTriggerSaveData();
            _npcRayStatesDict.Clear();
        }
    }
}

