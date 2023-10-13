using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class JsonHelper
{
    const string PROJECTFOLDER = "OwnEngine"; //TODO: Remove this if want this script to another project or edit project folder name //TODO: Fix it later to not use this variable

#region SaveJSON
    /// <summary>
    /// For save class data to json file. Can select to save it in StreamingAssets folder or not.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="saveObject"></param>
    /// <param name="streaming"></param>
    public static void SaveJSONAsObject(string fileName,object saveObject,bool streaming = false)
    {
        string data = JsonConvert.SerializeObject(saveObject,Formatting.Indented);

        if(streaming)
            CreateStreamingJSON(fileName,data); // if save it to StreamingAssets folder. it should be game data
        else
            CreateUserJSON(fileName,data);  // if not. it should be a user data. ex. save game data.
    }   
    
    /// <summary>
    /// Create a json file in StreamingAssets folder.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="data"></param>
    static void CreateStreamingJSON(string fileName,string data)
    {
        if(Application.isEditor)
        {
            StreamWriter writer;
            FileInfo t = new FileInfo($"{Application.streamingAssetsPath}/Data/JSONData/{fileName}.json");
            if(!t.Exists)
                t.Directory.Create();
            else
                t.Delete();

            writer = t.CreateText();
            writer.Write(data);
            writer.Close();
        }
    }

    /// <summary>
    /// Create a json file which is a save game data in user computer. but not in StreamingAssets folder.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="data"></param>
    static void CreateUserJSON(string fileName,string data)
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            StreamWriter writer;
            FileInfo t = new FileInfo(Application.persistentDataPath + "/" + PROJECTFOLDER + "/" + fileName + ".json");
            
            if(!t.Exists)
                t.Directory.Create();
            else
                t.Delete();

            writer = t.CreateText();
            writer.Write(data);
            writer.Close();
        }
        else if(Application.isEditor)
        {
            StreamWriter writer;
            FileInfo t = new FileInfo(Application.dataPath + "/../../Documents/" + PROJECTFOLDER + "/" + fileName + ".json");
            
            if(!t.Exists) 
                t.Directory.Create();
            else
                t.Delete();

            writer = t.CreateText();
            writer.Write(data);
            writer.Close();
        }
    }

    /// <summary>
    /// For Delete user data in user computer. ex. Save game
    /// </summary>
    /// <param name="fileName"></param>
    static void DeleteUserJSON(string fileName)
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            FileInfo t = new FileInfo(Application.persistentDataPath + "/" + PROJECTFOLDER + "/" + fileName + ".json");
            
            if(t.Exists)
                t.Delete();
        }
        else if(Application.isEditor)
        {
            FileInfo t = new FileInfo(Application.dataPath + "/../../Documents/" + PROJECTFOLDER + "/" + fileName + ".json");
            
            if(t.Exists)
                t.Delete();
        }
    }
#endregion

#region LoadJSON
    /// <summary>
    /// Load json file as object and that file should in StreamingAssets folder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T LoadJSONAsObject<T>(string fileName)
    {
        var data = LoadTextAppData(fileName);

        return DeserializeTextToObject<T>(data);
    }
    
    /// <summary>
    /// Load json file as object and that file should in StreamingAssets folder. But you can select JsonConverter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T LoadJSONAsObject<T>(string fileName,Newtonsoft.Json.JsonConverter converter) 
    {
        var data = LoadTextAppData(fileName);

        return DeserializeTextToObject<T>(data,converter);
    }

    /// <summary>
    /// For Load user data into Object.
    /// Ex. Save game data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T LoadUserJSONAsObject<T>(string fileName)
    {
        var data = LoadTextUserData(fileName);

        return LoadUserJSONAsObject<T>(data, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
    }

    /// <summary>
    /// For Load user data into Text.
    /// Ex. Save game data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    static string LoadTextUserData(string fileName)
    {
        string filePath;

        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            filePath = Application.persistentDataPath + "/" + PROJECTFOLDER + "/" + fileName + ".json";

            if(File.Exists(filePath))
            {
                StreamReader reader = File.OpenText(filePath);

                if(reader != null)
                {
                    string data = reader.ReadToEnd();
                    reader.Close();
                    return data;
                }
            }
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            filePath = Application.dataPath + "/../../Documents/" + PROJECTFOLDER +"/" + fileName + ".json"; //Folder to save  use data when play in editor

            if(File.Exists(filePath))
            {
                StreamReader reader = File.OpenText(filePath);

                if(reader != null)
                {
                    string data = reader.ReadToEnd();
                    reader.Close();
                    return data;
                }
            }
        }
        
        return "";
    }

    /// <summary>
    /// For Load game data into Text.
    /// Ex. Item data in game.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    static string LoadTextAppData(string fileName)
    {
        string filePath;

        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            filePath = $"{Application.streamingAssetsPath}/{fileName}.json";
            
            if(File.Exists(filePath))
            {
                StreamReader reader = File.OpenText(filePath);

                if(reader != null)
                {
                    string data = reader.ReadToEnd();
                    reader.Close();
                    return data;
                }
            }
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            filePath = $"{Application.streamingAssetsPath}/Data/JSONData/{fileName}.json";
            
            if(File.Exists(filePath))
            {
                StreamReader reader = File.OpenText(filePath);

                if(reader != null)
                {
                    string data = reader.ReadToEnd();
                    reader.Close();
                    return data;
                }
            }
        }
        
        return "";
    }

    public static T DeserializeTextToObject<T>(string data)
    {
        if(data != string.Empty)
        {
            try 
            {
                return (T)JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default(T);
            }
        }
        else
            return default(T);
    }

    public static T DeserializeTextToObject<T>(string data,Newtonsoft.Json.JsonConverter converter) 
    {
        if(data != string.Empty)
        {
            try 
            {
                return (T)JsonConvert.DeserializeObject<T>(data,converter);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default(T);
            }
        }
        else
            return default(T);
    }

    public static T LoadUserJSONAsObject<T>(string data,JsonSerializerSettings settings)
    {
        if(data != string.Empty)
        {
            try 
            {
                return (T)JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default(T);
            }
        }
        else
            return default(T);
    }
#endregion
}
