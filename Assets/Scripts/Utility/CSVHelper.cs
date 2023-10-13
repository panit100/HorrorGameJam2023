using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

public class CSVHelper
{
    /// <summary>
    /// Variable in class <T> must same as in CSV file. Parameter in CSV file and class must order in same position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T[] LoadCSVAsObject<T>(string fileName) 
    {
        var data = LoadTextCSV(fileName);
        if(data != string.Empty)
        {
            string[] lines = ConvertTextCSVToStringArray(data);

            string[] variableName = lines[0].Split(new[] {','});
            string[] variableType = lines[1].Split(new[] {','});

            T[] newObject = new T[lines.Length-2];

            for(int line = 0; line < lines.Length-2; line++)
            {
                newObject[line] = SetDataInClassT<T>(variableType,lines[line+2]);
            }

            return newObject;
        }

        return default(T[]);  
    }

    /// <summary>
    /// Load CSV data into text.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    static string LoadTextCSV(string fileName)
    {
        string filePath = $"{Application.streamingAssetsPath}/Data/CSVData/{fileName}.csv";

        if(File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            if(reader != null)
            {
                string data = reader.ReadToEnd();
                reader.Close();
                return data;
            }
        }
        else
        {
            UnityEngine.Debug.LogError($"File Name {fileName} is not exist.");
        }

        return "";
    }

    /// <summary>
    /// Convert CSV data into array of string.
    /// </summary>
    /// <param name="textCSV"></param>
    /// <returns></returns>
    static string[] ConvertTextCSVToStringArray(string textCSV)
    {
        string[] lines = textCSV.Split(new[] {'\n'});
        return lines;
    }

    /// <summary>
    /// Set data that loaded into a class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="variableType"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    static T SetDataInClassT<T>(string[] variableType,string data)
    {
        string[] dataArray = data.Split(new[] {','});

        T newT;

        Type type = typeof(T);  
        ConstructorInfo[] constructorInfos = type.GetConstructors();

        // CheckConstructor(constructorInfos[0],variableName);
        
        ConstructorInfo desiredConstructor = constructorInfos[0];

        if (desiredConstructor != null)
        {
            // Create an instance of the class using the constructor
            object instance = desiredConstructor.Invoke(null);

            // Cast the instance to the desired type
            newT = (T)instance;
        }
        else
        {
            throw new Exception("Constructor without specific parameter types not found.");
        }

        Type objectType = newT.GetType();
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Instance);

        for(int i = 0; i < variableType.Length; i++)
        {

            SetValueOfInstance<T>(fieldInfos[i],newT,variableType[i],dataArray[i]);
        }

        return newT;
    }

    /// <summary>
    /// Set value that loaded into a parameter in class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fieldInfo"></param>
    /// <param name="instance"></param>
    /// <param name="variableType"></param>
    /// <param name="value"></param>
    /// <exception cref="Exception"></exception>
    static void SetValueOfInstance<T>(FieldInfo fieldInfo,T instance,string variableType,string value) //TODO Add another variable to switch case
    {
        object _value = null;

        string type = variableType.Trim();

        switch(type.ToLower())
        {
            case "string":
                _value = ConvertToType<string>(value);
                break;
            case "int":
                _value = ConvertToType<int>(value);
                break;
            case "float":
                _value = ConvertToType<float>(value);
                break;
            case "bool":
                _value = ConvertToType<bool>(value);
                break;
            case "":
                throw new Exception($"Variable Type {variableType} can't convert.");
        }

        fieldInfo.SetValue(instance,_value);
    }

    /// <summary>
    /// Convert type from string to <T> Type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    static T ConvertToType<T>(string value)
    {
        try
        {
            // Attempt to convert the string to the specified type
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (FormatException)
        {
            // Handle the case where the string cannot be converted to the specified type
            throw new Exception($"Unable to convert '{value}' to type {typeof(T)}.");
        }
    }

    /// <summary>
    /// Export CSV to JSON file. Must check object that you want to export is array or not.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="exportObject"></param>
    static void ExportCSVToJSON(string fileName,object exportObject)
    {
        JsonHelper.SaveJSONAsObject(fileName, exportObject,true);
    }
}