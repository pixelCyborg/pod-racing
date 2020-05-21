﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem
{
    [System.Serializable]
    public class SaveData
    {
        public string name;
        public int credits;
        public VehicleData vehicle;

        public SaveData(string _name)
        {
            name = _name;
            vehicle = new VehicleData();
            credits = 0;
        }
    }

    private static SaveData currentSave;

    public static void Save(string saveName = "")
    {
        //Update the current save data, creating a new one if there isn't one
        if (saveName == "") saveName = "player";
        if (currentSave == null) currentSave = new SaveData(saveName);
        currentSave.vehicle = Garage.data;
        currentSave.credits = CreditsTracker.Credits();
        Debug.Log("Saving " + currentSave.credits + " credits");

        //Start up the serializer and create a new save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(SavePath(currentSave.name));

        //Write the data and close the serializer
        bf.Serialize(file, currentSave);
        file.Close();

        Debug.Log("Game Saved");
    }

    public static void Load(string saveName = "")
    {
        if (saveName == "") saveName = "player";

        string filePath = SavePath(saveName);
        if (File.Exists(filePath))
        {
            Debug.Log("Loaded Save");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SavePath(saveName), FileMode.Open);
            currentSave = (SaveData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Loading " + currentSave.credits + " credits");
            Garage.data = currentSave.vehicle;
        }
        else
        {
            Debug.Log("No game saved!");
            currentSave = new SaveData(saveName);
        }
    }

    private static string SavePath(string name)
    {
        return Application.persistentDataPath + "/" + name + ".save";
    }

    public static void ClearSave(string saveName = "")
    {
        if (saveName == "") saveName = "player";

        string filePath = SavePath(saveName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static VehicleData PlayerVehicle()
    {
        if (currentSave == null) Load();
        return currentSave.vehicle;
    }

    public static int Credits()
    {
        if (currentSave == null) Load();
        return currentSave.credits;
    }
}
