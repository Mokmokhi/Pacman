using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    
    public const int SKINCOUNT = 3;
    // SkinShop is a class to store the cost of each skin.
    public class SkinShop {
        public int[] skinCost;
        public SkinShop(int[] skinlist) {
            skinCost = skinlist;
        }
    }
    public SkinShop shop;
  
    // Load the skin data from the database before the first frame.
    void Start()
    {   
        int[] skinlist = new int[SKINCOUNT];
        shop = new SkinShop(skinlist);
        LoadSkinShop();
    }

    // function CalCoins to calculate the coins received after 1 game.
    public int CalCoins(int score, int lives) {
        float param = 1 + 0.5f * (lives - 1);
        return (int)(score * lives);
    }
    // function SaveCoins to save player's coin to that database.
    public void SaveCoins(int coin) {
        DataBaseManager.Instance.profile.Coins += coin;
        DataBaseManager.Instance.SaveData();
    }
    // function SaveSkinShop is not useful.
    // public void SaveSkinShop() {
    //     var shopjson = JsonUtility.ToJson(shop);
    //     print(shopjson.ToString());
    //     DataBaseManager.Instance.GetReference().Child("SkinShop").SetRawJsonValueAsync(shopjson).ContinueWithOnMainThread(task => {
    //         if (task.IsCompletedSuccessfully) {
    //             print("List stored.");
    //         } else print("List store failed.");
    //     });
    // }
    
    //dunction LoadSkinShop to load the skin data from database to the local.
    public void LoadSkinShop() {
        DataBaseManager.Instance.GetReference().Child("SkinShop").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully) {
                var val = task.Result.GetRawJsonValue();
                print(val);
                shop = JsonUtility.FromJson<SkinShop>(val);
                print("Loaded shop");
                print(shop);
                PrintDict();
            } else print("Failed to load skin shop");
        });
    }

    // function PrintDict to print the skin cost list. Just for dev.
    public void PrintDict() {
        print(shop.skinCost);
    }
    // function CheckHasSkin to check if the player has the skin input.
    public bool CheckHasSkin(int item) {
        int val = DataBaseManager.Instance.profile.HasSkin >> (int)item;
        val = val & 1;
        print("checkhasSkin " + val.ToString());
        if (val == 1)
            return true;
        else return false;
    }
    // function BuySkin to let player buy the skin input.
    public void BuySkin(int item) {
        if (DataBaseManager.Instance.profile.Coins >= shop.skinCost[item]) {
            DataBaseManager.Instance.profile.Coins -= shop.skinCost[item];
            DataBaseManager.Instance.profile.HasSkin += 1 << item;
            DataBaseManager.Instance.SaveData();
        } else print("Not enough Coins! " + DataBaseManager.Instance.profile.Coins);
    }

    // function GetPowerCost to calculate how many coins are needed to upgrade the powerpellet.
    public int GetPowerCost() {
        int cost = (int)Math.Round(Math.Pow(1.1, DataBaseManager.Instance.profile.PowerLevel) * 1000);
        return cost;
    }
    // function BuyPower to upgrade the powerpellet.
    public void BuyPower() {
        int cost = GetPowerCost();
        if (DataBaseManager.Instance.profile.Coins >= cost) {
            DataBaseManager.Instance.profile.Coins -= cost;
            DataBaseManager.Instance.profile.PowerLevel++;
            DataBaseManager.Instance.SaveData();
        }
    }
}
