using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Sirenix.Utilities;

public class ImageHelper
{
    const string PROJECTFOLDER = "Astronosis"; 
    const string IMAGEFOLDER = "CameraScreenShot"; 
    
    public static void SaveImage(Texture2D texture)
    {
        string imagePath = GetImagePath();

        UpdateFileInDirectory();

        string savePath = $"{imagePath}0.png";

        string directory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);
    }

    public static void UpdateFileInDirectory()
    {
        string imagePath = GetImagePath();
        
        if (Directory.Exists(imagePath))
        {
            if(File.Exists($"{imagePath}9.png"))
            {
                File.Delete($"{imagePath}9.png");
            }

            string[] files = Directory.GetFiles(imagePath);
            files.Sort();

            for(int i = files.Length; i > 0; i--)
            {
                int fileName = i;
                File.Move(files[i-1],$"{imagePath}{fileName}.png");
            }
        }
    }

    public static string GetImagePath()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            return $"{Application.persistentDataPath}/{PROJECTFOLDER}/{IMAGEFOLDER}/";
        }
        else if(Application.isEditor)
        {
            return $"{Application.dataPath}/../../Documents/{PROJECTFOLDER}/{IMAGEFOLDER}/";
        }

        return string.Empty;
    }

    public static Texture2D LoadImageTexture(int index)
    {
        string[] files = GetAllFilePath();
        files.Sort();
        
        if (index < files.Length)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(files[index]);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return texture;
        }
        else
        {
            return null;
        }
    }

    public static void ClearAllImageInDirectory()
    {
        string[] files = GetAllFilePath();

        for(int i = 0; i < files.Length; i++)
        {
            if (File.Exists(files[i]))
                File.Delete(files[i]);
        }
    }

    public static int GetImageCount()
    {
        string[] files = GetAllFilePath();
        return files.Length;
    }

    public static string[] GetAllFilePath()
    {
        string imagePath = GetImagePath();
        string[] files = Directory.GetFiles(imagePath);
        return files;
    }
}
