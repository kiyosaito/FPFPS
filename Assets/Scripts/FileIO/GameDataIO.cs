using System.IO;
using UnityEngine;

public class GameDataWriter
{
    private BinaryWriter _writer = null;

    public static void SaveData(string filename, PersistableData data)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);

        // The using (something) {} format will dispose of the something after the code block even if an exception occurs
        // Basically it's shorthand for a try catch block, only works with IDisposable types
        using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            data.Save(new GameDataWriter(writer));
        }
    }

    private GameDataWriter(BinaryWriter writer)
    {
        _writer = writer;
    }

    public void Write(int data)
    {
        _writer.Write(data);
    }

    public void Write(float data)
    {
        _writer.Write(data);
    }

    public void Write(Vector3 data)
    {
        _writer.Write(data.x);
        _writer.Write(data.y);
        _writer.Write(data.z);
    }

    public void Write(Quaternion data)
    {
        _writer.Write(data.x);
        _writer.Write(data.y);
        _writer.Write(data.z);
        _writer.Write(data.w);
    }

    public void Write(Color data)
    {
        _writer.Write(data.r);
        _writer.Write(data.g);
        _writer.Write(data.b);
        _writer.Write(data.a);
    }

    public void Write(Random.State value)
    {
        _writer.Write(JsonUtility.ToJson(value));
    }
}

public class GameDataReader
{
    private BinaryReader _reader = null;

    public static T LoadData<T>(string filename) where T : PersistableData, new()
    {
        string path = Path.Combine(Application.persistentDataPath, filename);

        T data = null;

        // The using (something) {} format will dispose of the something after the code block even if an exception occurs
        // Basically it's shorthand for a try catch block, only works with IDisposable types
        if (File.Exists(path))
        {
            using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                data = PersistableData.Load<T>(new GameDataReader(reader));
            }
        }

        return data;
    }

    private GameDataReader(BinaryReader reader)
    {
        _reader = reader;
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public float ReadFloat()
    {
        return _reader.ReadSingle();
    }

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Quaternion ReadQuaternion()
    {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Color ReadColor()
    {
        return new Color(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Random.State ReadRandomState()
    {
        return JsonUtility.FromJson<Random.State>(_reader.ReadString());
    }
}