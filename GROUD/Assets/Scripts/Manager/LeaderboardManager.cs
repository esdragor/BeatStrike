using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LeaderboardManager : MonoBehaviour
{
    private static LeaderboardManager instance;

    [SerializeField] private TMP_Text totalScore;
    [SerializeField] private TMP_Text RankText;
    [SerializeField] private string[] leaderboardNames;

    private List<string> leaderboardtoPrint;

    private Dictionary<string, int> COLLECTION = new ();

    private int scoreTotal;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        scoreTotal = 0;
        AddScore(PlayerPrefs.HasKey("ScoreTotal") ? PlayerPrefs.GetInt("ScoreTotal") : 0);
    }

    public static void AddScore(int score)
    {
        instance.scoreTotal += score;
        instance.totalScore.text = "Total score : " + instance.scoreTotal;
        PlayerPrefs.SetInt("ScoreTotal", instance.scoreTotal);


        instance.COLLECTION.Clear();
        instance.COLLECTION.Add("YOU", instance.scoreTotal);

        for (int i = 0; i < instance.leaderboardNames.Length; i++)
        {
            int newScore = (PlayerPrefs.HasKey(instance.leaderboardNames[i]))
                ? PlayerPrefs.GetInt(instance.leaderboardNames[i])
                : 0;

            newScore += Random.Range(0, 15000);
            PlayerPrefs.SetInt(instance.leaderboardNames[i], newScore);

            instance.COLLECTION.Add(instance.leaderboardNames[i], newScore);
        }

        instance.leaderboardtoPrint = instance.Sort();

        instance.RankText.text = "";
        for (int i = 0; i < instance.leaderboardtoPrint.Count; i++)
        {
            instance.RankText.text += instance.leaderboardtoPrint[i] + "\n\n\n\n";
        }
    }

    public List<string> Sort()
    {
        var value = new List<string>();
        int rank = 0;
        foreach (var VARIABLE in COLLECTION.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key))
        {
            rank++;
            value.Add(rank + ": " + VARIABLE + " : " + COLLECTION[VARIABLE]);
        }

        int index = -1;
        for (int i = 0; i < value.Count; i++)
        {
            if (value[i].Contains("YOU"))
            {
                index = i;
                break;
            }
        }

        var value2 = new List<string>();
        if (index - 2 >= 0)
        {
            value2.Add(value[index - 2]);
        }

        if (index - 1 >= 0)
        {
            value2.Add(value[index - 1]);
        }

        if (index >= 0)
            value2.Add(value[index]);
        if (index + 1 < value.Count)
        {
            value2.Add(value[index + 1]);
        }

        if (index + 2 < value.Count)
        {
            value2.Add(value[index + 2]);
        }

        return value2;
    }
}