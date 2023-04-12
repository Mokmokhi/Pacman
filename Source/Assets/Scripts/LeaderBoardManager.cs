using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public DataBaseManager firebasemanager;
    [SerializeField]
    public TMP_InputField testinput;

    public Board leaderBoard;
    public class Entries {
        public string UserName;
        public int score;
        public Entries() {
            UserName = "User";
            score = 0;
        }
    }
    public class Board {
        public Entries[] entries;
        public Board() {
            entries = new Entries[5];
        }
        public void printBoard() {
            foreach (var entry in entries) {
                print(entry.UserName+ " " + entry.score);
            }
        }
    }

    void Start() {
        leaderBoard = new Board();
    }
 
    public void PrintBoard() {
        firebasemanager.GetReference().Child("LeaderBoard").OrderByChild("score").LimitToFirst(5).GetValueAsync().ContinueWith(task => {
            if (task.IsCompletedSuccessfully) {
                    DataSnapshot val = task.Result;
                    print(val.ChildrenCount.ToString());
                    int i = 4;
                    Debug.Log("start reading entry.");
                    foreach (var entry in val.Children) {
                        Debug.Log("entry: " + i.ToString());
                        var json = entry.GetRawJsonValue();
                        Debug.Log("Transfered to json");
                        leaderBoard.entries[i] = JsonUtility.FromJson<Entries>(json);
                        Debug.Log("added to borad.");
                        i--;
                    }
                    leaderBoard.printBoard();
                } else print("Print Failed.");
        });
    }
    public void UpdateScore(int score) {
        if (score > firebasemanager.profile.HighestScore) {
            firebasemanager.profile.HighestScore = score;
            firebasemanager.SaveData();
            AddScoreToLeaders(score);
        }
    }
    public int CalScore(int score, int lives, int param) {
        //TODO:
        // Score calculating Formula.
        return score * lives * param;
    }

    public void TestAdding() {
        AddScoreToLeaders(int.Parse(testinput.text));
    }
    public void AddScoreToLeaders(int score) {

    firebasemanager.GetReference().Child("LeaderBoard").RunTransaction(mutableData => {
        List<object> leaders = mutableData.Value as List<object>;

        if (leaders == null) {
            leaders = new List<object>();
        } else if (mutableData.ChildrenCount >= 5) {
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
        newScoreMap["UserName"] = firebasemanager.profile.UserName;
        leaders.Add(newScoreMap);
        mutableData.Value = leaders;
        return TransactionResult.Success(mutableData);
        });
    }
}
