using System.IO;
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

                    _player.m_SkinUsing = Game.ResourceManager.Instance.ShopInfo.m_PrefabOutfits[1].name;
                    _player.m_SkinColor = 2;
                    _player.m_SkinBoughts = new System.Collections.Generic.List<ItemBought>();
                    ItemBought _skin = new ItemBought();
                    _skin.id = _player.m_SkinUsing;
                    _skin.colors.Add(_player.m_SkinColor);
                    _player.m_SkinBoughts.Add(_skin);

                    _player.m_HairUsing = Game.ResourceManager.Instance.ShopInfo.m_PrefabHairs[2].name;
                    _player.m_HairBoughts = new System.Collections.Generic.List<ItemBought>();
                    _player.m_HairColorUsing = 1;
                    ItemBought _hair = new ItemBought();
                    _hair.id = _player.m_HairUsing;
                    _hair.colors.Add(_player.m_HairColorUsing);
                    _player.m_HairBoughts.Add(_hair);

                    _player.m_HatUsing = "None";
                    _player.m_HatBoughts = new System.Collections.Generic.List<ItemBought>();
                    ItemBought _hat = new ItemBought();
                    _hat.id = _player.m_HatUsing;
                    _player.m_HatBoughts.Add(_hat);

                    _player.m_FaceUsing = Game.ResourceManager.Instance.ShopInfo.m_PrefabsFaces[0].name;

                    _player.m_UtilityUsing = "None";
                    _player.m_UtilityBoughts = new System.Collections.Generic.List<string>();
                    _player.m_UtilityBoughts.Add("None");
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
