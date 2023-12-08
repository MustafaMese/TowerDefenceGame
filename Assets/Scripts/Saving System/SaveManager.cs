using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static readonly string SavePath = Application.persistentDataPath + "/save.dat";

    public static void SetIsLastGameDefeat(bool isDefeat)
    {
        PlayerPrefs.SetInt("lastGameDefeat", Convert.ToInt16(isDefeat));
    }
    
    public static bool GetIsLastGameDefeat()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt("lastGameDefeat", 0));
    }
    
    public static void SaveGameState(GameState data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(SavePath, FileMode.Create);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static GameState LoadGameState()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(SavePath, FileMode.Open);

            try
            {
                GameState data = (GameState)formatter.Deserialize(fileStream);
                fileStream.Close();
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading player data: " + e.Message);
                fileStream.Close();
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No saved data found.");
            return null;
        }
    }
}
