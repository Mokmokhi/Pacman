using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUpdate : MonoBehaviour {

    public DataBaseManager firebasemanager;

    // Start is called before the first frame update
    public void UpdateScore(int score) {
        if (score > firebasemanager.profile.getScore()) {
            firebasemanager.profile.setScore(score);
            firebasemanager.SaveData();
        }
    }
    public int CalScore(int score, int lives, int param) {
        //TODO:
        // Score calculating Formula.
        return score * lives * param;
    }
}
