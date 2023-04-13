using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Board leaderBoard;
    [SerializeField]
    public const int BOARDLENGTH = 5;
    [SerializeField]
    public TMP_Text outputScore;
    [SerializeField]
    public TMP_Text outputName;
    [SerializeField]
    public TMP_Text outputRank;
    public class Entries {
        public string UserName;
        public int score;
        public Entries() {
            UserName = "User";
            score = 0;
            //Debug.Log("Entries");
        }
    }
    public class Board {
        public Entries[] entries;
        public Board() {
            entries = new Entries[BOARDLENGTH];
            for (int i = 0; i < entries.Length; i++) {
                entries[i] = new Entries();
            }

        }
        public void printEntires() {

            foreach (Entries entry in entries) {
                print(entry.UserName + " " + entry.score);
            }
        }
    }

    void Start() {
        leaderBoard = new Board();
    }

    public void PrintBoard() {
        outputRank.text = "Rank\n1\n2\n3\n4\n5\nYou";
        outputName.text = "Name\n";
        outputScore.text = "Score\n";
        for (int i = 0; i < leaderBoard.entries.Length ; i++) {
            outputName.text += leaderBoard.entries[i].UserName + "\n";
            outputScore.text += leaderBoard.entries[i].score.ToString() + "\n";
        }
        outputName.text += DataBaseManager.Instance.profile.UserName;
        outputScore.text += DataBaseManager.Instance.profile.HighestScore.ToString();
    }

    public void StoreBoard() {
        DataBaseManager.Instance.GetReference().Child("LeaderBoard").OrderByChild("score").LimitToFirst(BOARDLENGTH).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully) {
                    DataSnapshot val = task.Result;
                    print(val.ChildrenCount.ToString());
                    int i = 4;
                    Debug.Log("start reading entry.");
                    foreach (var entry in val.Children) {
                        //Debug.Log("entry: " + i.ToString());
                        var json = entry.GetRawJsonValue();
                        //Debug.Log("Transfered to json");
                        leaderBoard.entries[i] = JsonUtility.FromJson<Entries>(json);
                        //Debug.Log("added to borad.");
                        i--;
                    }
                    PrintBoard();
                } else print("Print Failed.");
        });

        leaderBoard.printEntires();
    }
    public void UpdateScore(int score) {
        if (score > DataBaseManager.Instance.profile.HighestScore) {
            DataBaseManager.Instance.profile.HighestScore = score;
            DataBaseManager.Instance.SaveData();
            AddScoreToLeaders(score);
        }
    }

    public void ClearScore() {
        DataBaseManager.Instance.profile.HighestScore = 0;
        DataBaseManager.Instance.SaveData();
    }

    public void AddScoreToLeaders(int score) {

    DataBaseManager.Instance.GetReference().Child("LeaderBoard").RunTransaction(mutableData => {
        List<object> leaders = mutableData.Value as List<object>;

        if (leaders == null) {
            leaders = new List<object>();
        } else if (mutableData.ChildrenCount >= BOARDLENGTH) {
            long minScore = long.MaxValue;
            object minVal = null;
            foreach (var child in leaders) {
            if (!(child is Dictionary<string, object>)) continue;
            long childScore = (long)((Dictionary<string, object>)child)["score"];
            if (childScore < minScore) {
                minScore = childScore;
                minVal = child;
            }
            }
            if (minScore > score) {
            // The new score is lower than the existing 5 scores, abort.
            return TransactionResult.Abort();
            }

            // Remove the lowest score.
            leaders.Remove(minVal);
        }
        // Add the new high score.
        Dictionary<string, object> newScoreMap =
                        new Dictionary<string, object>();
        newScoreMap["score"] = score;
        newScoreMap["UserName"] = DataBaseManager.Instance.profile.UserName;
        leaders.Add(newScoreMap);
        mutableData.Value = leaders;
        return TransactionResult.Success(mutableData);
        });
    }
}
