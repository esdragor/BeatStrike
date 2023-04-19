using System.Collections.Generic;
using UnityEngine;
public class MelodySO : ScriptableObject
{
    public int seed;
    
    public List<PatternSO> patterns = new List<PatternSO>();
    public List<PatternSO> selectedPatterns = new List<PatternSO>();


    public void GenerateSeed()
    {
        seed = System.DateTime.Now.GetHashCode();
        SetSeed();
    }

    public void SetSeed()
    {
        Random.InitState(seed);
        SetRandomPatterns();
    }

    public void SetRandomPatterns()
    {
        selectedPatterns.Clear();
        for (int i = 0; i < 18; i++)
        {
            selectedPatterns.Add(patterns[Random.Range(0, patterns.Count)]);
        }
    }

    public string GetSeedPatternsPreview()
    {
        string seedID = "";
        string insert = "";

        for (int i = 0; i < patterns.Count; i++)
        {
            if (selectedPatterns.Contains(patterns[i]))
            {
                insert = insert + $"{i}";
            }
        }
        
        seedID = insert;
        return seedID;
    }
}
