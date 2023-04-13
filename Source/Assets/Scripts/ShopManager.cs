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

    // Start is called before the first frame update
    public const int SKINCOUNT = 3;

    [Serializable]
    public class SkinShop {
        public int[] skinCost;
        public SkinShop(int[] skinlist) {
            skinCost = skinlist;
        }
    }
    public SkinShop shop;
  

    void Start()
    {   
        int[] skinlist = new int[SKINCOUNT];
        shop = new SkinShop(skinlist);
        LoadSkinShop();
    }
    public int CalCoins(int score, int lives) {
        //TODO:
        // Score calculating Formula.
        float param = 1 + 0.5f * (lives - 1);
        return (int)(score * lives);
    }
    public void SaveCoins(int coin) {
        DataBaseManager.Instance.profile.Coins += coin;
        DataBaseManager.Instance.SaveData();
    }
    public void SaveSkinShop() {
        var shopjson = JsonUtility.ToJson(shop);
        print(shopjson.ToString());
        DataBaseManager.Instance.GetReference().Child("SkinShop").SetRawJsonValueAsync(shopjson).ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully) {
                print("List stored.");
            } else print("List store failed.");
        });
    }
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

    public void PrintDict() {
        print(shop.skinCost);
    }
    public bool CheckHasSkin(int item) {
        int val = DataBaseManager.Instance.profile.HasSkin >> (int)item;
        val = val & 1;
        print("checkhasSkin " + val.ToString());
        if (val == 1)
            return true;
        else return false;
    }
    public void BuySkin(int item) {
        if (DataBaseManager.Instance.profile.Coins >= shop.skinCost[item]) {
            DataBaseManager.Instance.profile.Coins -= shop.skinCost[item];
            DataBaseManager.Instance.profile.HasSkin += 1 << item;
            DataBaseManager.Instance.SaveData();
        } else print("Not enough Coins! " + DataBaseManager.Instance.profile.Coins);
    }

    public int GetPowerCost() {
        int cost = (int)Math.Round(Math.Pow(1.1, DataBaseManager.Instance.profile.PowerLevel) * 1000);
        return cost;
    }
    public void BuyPower() {
        int cost = GetPowerCost();
        if (DataBaseManager.Instance.profile.Coins >= cost) {
            DataBaseManager.Instance.profile.Coins -= cost;
            DataBaseManager.Instance.profile.PowerLevel++;
            DataBaseManager.Instance.SaveData();
        }
    }
}
