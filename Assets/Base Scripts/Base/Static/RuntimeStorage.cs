﻿using System.IO;
using UnityEngine;

public static class RuntimeStorageData
{
    public enum DATATYPE
    {
        NULL,
        SOUND,
        PLAYER
    }

    public static SoundSerializable SOUND;
    public static PlayerSerializable PLAYER;

    private static string _dataSound = OptimizeComponent.GetStringOptimize(Application.persistentDataPath, "/", HashLib.GetHashStringAndDeviceID(StaticVariable.DATA_SOUND));
    private static string _dataPlayer = OptimizeComponent.GetStringOptimize(Application.persistentDataPath, "/", HashLib.GetHashStringAndDeviceID(StaticVariable.DATA_PLAYER));

    public static bool IsReady
    {
        get
        {
            if (SOUND == null || PLAYER == null)
                return false;
            return true;
        }
    }

    public static void ReadAllData()
    {
        SOUND = ReadData<SoundSerializable>(DATATYPE.SOUND) as SoundSerializable;
        PLAYER = ReadData<PlayerSerializable>(DATATYPE.PLAYER) as PlayerSerializable;
        LogSystem.LogSuccess("Load all data in game");
    }

    public static void ReadNewData()
    {
        SOUND = ReadNew<SoundSerializable>(DATATYPE.SOUND) as SoundSerializable;
        PLAYER = ReadNew<PlayerSerializable>(DATATYPE.PLAYER) as PlayerSerializable;
        LogSystem.LogSuccess("Load all data in game");
    }

    public static void SaveAllData()
    {
        SaveData(_dataSound, SOUND);
        SaveData(_dataPlayer, PLAYER);
        LogSystem.LogSuccess("Save all data in game");
    }

    public static T ReadData<T>(DATATYPE dataType) where T : class, new()
    {
        var dataPath = GetPath(dataType);

        if (File.Exists(dataPath))
        {
            var data = ReadDataExist<T>(dataPath);
            return data;
        }
        else
        {
            var data = GetDataDefault<T>(dataType);
            return data;
        }
    }

    public static T ReadNew<T>(DATATYPE dataType) where T : class, new()
    {
        var data = GetDataDefault<T>(dataType);
        return data;
    }

    public static void DeleteData(DATATYPE dataType)
    {
        var dataPath = GetPath(dataType);
        LogSystem.LogWarning(dataPath);

        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            LogSystem.LogSuccess($"Delete {dataPath} success!");
        }
        else
        {
            LogSystem.LogError($"Can't delete {dataPath} because it's not found!");
        }
    }

    private static T GetDataDefault<T>(DATATYPE dataType) where T : class
    {
        try
        {
            switch (dataType)
            {
                case DATATYPE.SOUND:
                    var _sound = new SoundSerializable();
                    _sound.isMusic = true;
                    _sound.isSound = true;
                    _sound.isVibrate = true;
                    return _sound as T;
                case DATATYPE.PLAYER:
                    var _player = new PlayerSerializable();
                    _player.id = SystemInfo.deviceUniqueIdentifier;
                    _player.level = 1;
                    _player.coin = 0;
                    _player.character_using = Game.ResourceManager.Instance.ShopInfo.m_PrefabOutfits[1].name;
                    _player.characters_bought = new System.Collections.Generic.List<string>();
                    _player.characters_bought.Add(_player.character_using);
                    _player.character_color_using = 2;
                    _player.hair_using = Game.ResourceManager.Instance.ShopInfo.m_PrefabHairs[2].name;
                    _player.hairs_bought = new System.Collections.Generic.List<string>();
                    _player.hairs_bought.Add(_player.hair_using);
                    _player.hairs_bought.Add("None");
                    _player.hair_color_using = 1;
                    _player.hat_using = "None";
                    _player.hats_bought = new System.Collections.Generic.List<string>();
                    _player.hats_bought.Add(_player.hat_using);
                    _player.utility_using = Game.ResourceManager.Instance.ShopInfo.m_PrefabsFaces[0].name;
                    _player.utility_bought = new System.Collections.Generic.List<string>();
                    _player.utility_bought.Add(_player.utility_using);
                    return _player as T;
                default:
                    return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
        return null;
    }

    private static void SaveData<T>(string path, T data)
    {
        if (data == null) return;
        string _data = JsonUtility.ToJson(data);
        if (_data == null || _data == "" || _data == "{}") return;

        Debug.Log(_data);

        _data = HashLib.Base64Encode(_data);
        var encodeMD5 = HashLib.EncryptAndDeviceID(_data);
        File.WriteAllText(path, encodeMD5);
    }

    private static T ReadDataExist<T>(string path) where T : class
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string _data = sr.ReadToEnd();
            var decodeMD5 = HashLib.DecryptAndDeviceID(_data);
            _data = HashLib.Base64Decode(decodeMD5);

            var data = JsonUtility.FromJson<T>(_data);
            fs.Flush();
            fs.Close();
            return data;
        }
        catch (System.Exception ex)
        {

            Debug.Log(ex.Message);

        }
        return null;
    }

    private static string GetPath(DATATYPE dataType)
    {
        string dataPath = "";

        switch (dataType)
        {
            case DATATYPE.SOUND:
                dataPath = _dataSound;
                break;
            case DATATYPE.PLAYER:
                dataPath = _dataPlayer;
                break;
            default:
                break;
        }

        LogSystem.LogSuccess(OptimizeComponent.GetStringOptimize("Load ", dataPath));

        return dataPath;
    }
}
