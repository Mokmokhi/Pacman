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
    DataBaseManager firebasemanager;
    // Start is called before the first frame update
    public const int SKINCOUNT = 3;

    public enum SKIN {
        DEFAULT,
        SKULL,
        UGLY
    }
    
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
        firebasemanager = DataBaseManager.Instance.GetComponent<DataBaseManager>();
        int[] skinlist = new int[SKINCOUNT];
        shop = new SkinShop(skinlist);
        LoadSkinShop();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            LoadSkinShop();
            print("printed");
        } 
        if (Input.GetKeyDown(KeyCode.S)) {
            SaveSkinShop();
            print("printed");
        } 
    }

    public void SaveSkinShop() {
        var shopjson = JsonUtility.ToJson(shop);
        print(shopjson.ToString());
        firebasemanager.GetReference().Child("SkinShop").SetRawJsonValueAsync(shopjson).ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully) {
                print("List stored.");
            } else print("List store failed.");
        });
    }
    public void LoadSkinShop() {
        firebasemanager.GetReference().Child("SkinShop").GetValueAsync().ContinueWithOnMainThread(task => {
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
    public bool CheckHasSkin(SKIN item) {
        uint val = firebasemanager.profile.HasSkin >> (int)item;
        val = val | (uint)1;
        print(val);
        if (val == 1)
            return true;
        else return false;
    }
    public void BuySkin(SKIN item) {
        if (firebasemanager.profile.Coins >= shop.skinCost[(int)item]) {
            firebasemanager.profile.Coins -= shop.skinCost[(int)item];
            firebasemanager.profile.HasSkin += (uint)1 << (int)item;
            firebasemanager.SaveData();
        }
    }

    public void BuyPower() {
        int cost = (int)Math.Round(firebasemanager.profile.PowerLevel * 1.1 * 100);
        if (firebasemanager.profile.Coins >= cost) {
            firebasemanager.profile.Coins -= cost;
            firebasemanager.profile.PowerLevel++;
            firebasemanager.SaveData();
        }
    }
}
