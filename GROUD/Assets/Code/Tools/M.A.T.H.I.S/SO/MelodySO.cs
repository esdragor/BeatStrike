using System.Collections.Generic;
using UnityEngine;
public class MelodySO : ScriptableObject
{
    public int seed;
    
    public List<Pattern> patterns;
    public List<Pattern> selectedPatterns;


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
            for (int j = 0; j < selectedPatterns.Count; j++)
            {
                if (selectedPatterns[j] == patterns[i])
                {
                    insert = seedID.Insert(seedID.Length, $"{i}");
                }
            }
        }
        
        seedID = insert;
        return seedID;
    }
}
