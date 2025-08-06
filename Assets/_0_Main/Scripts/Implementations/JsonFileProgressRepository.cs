using UnityEngine;
using System.IO;

public class JsonFileProgressRepository : IProgressRepository
{
    readonly string _path = Path.Combine(Application.persistentDataPath, "save.json");
    public void Save(GameProgress p)
    {
        File.WriteAllText(_path, JsonUtility.ToJson(p));
    }
    public GameProgress Load()
    {
        if (!File.Exists(_path)) return new GameProgress();
        return JsonUtility.FromJson<GameProgress>(File.ReadAllText(_path));
    }
}
