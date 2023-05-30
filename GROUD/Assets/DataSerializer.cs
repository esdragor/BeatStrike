using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NaughtyAttributes;
using UnityEngine;

[XmlRoot("Data")]
public class DataSerializer : MonoBehaviour
{
    [SerializeField, ReadOnly] private string path;
    
    Encoding encoding = Encoding.UTF8;
    public static DataSerializer instance;

    private void Awake()
    {
        instance = this;
    }

    public void SaveDataOnMainDirectory<T>(T data, string pathExtension)
    {
        path = Application.persistentDataPath + "/" + pathExtension + ".xml";
        
        SaveData(data);
    }

    private void SaveData<T>(T data)
    {
        StreamWriter streamWriter = new(path, false, encoding);
        XmlSerializer dataSerializer = new XmlSerializer(typeof(T));
        
        dataSerializer.Serialize(streamWriter, data);
        streamWriter.Close();
        
        Debug.Log("Save " + typeof(T).Name);
    }

    public T LoadDataFromDirectory<T>(string pathExtension)
    {
        path = Application.persistentDataPath + "/" + pathExtension + ".xml";
        return LoadData<T>();
    }

    private T LoadData<T>()
    {
        if (File.Exists(path))
        {
            FileStream fileStream = new(path, FileMode.Open);
            XmlSerializer dataSerializer = new XmlSerializer(typeof(T));

            T data = (T)dataSerializer.Deserialize(fileStream);
            fileStream.Close();

            Debug.Log("Success Load " + typeof(T).Name);
            return data;
        }
        throw new NullReferenceException("Path not exists");
    }
    
    public bool CheckFileExists(string pathExtension = "")
    {
        path = Application.persistentDataPath + "/" + pathExtension + ".xml";
        return File.Exists(path);
    }

    public void DeleteFile(string pathExtension)
    {
        path = Application.persistentDataPath + "/" + pathExtension + ".xml";
        File.Delete(path);
    }
}