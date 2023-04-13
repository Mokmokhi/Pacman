using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    public GameObject displayRotate;
    [SerializeField]
    public GameObject[] displaySkin;
    [SerializeField]
    public Button ButtonSkin;
    public Button ButtonPower;
    private int NumOfSkin = 0;
    private int selected = 0;
    private Transform targetRot;

    void Start() {
        NumOfSkin = displaySkin.Length;
        CheckBuyable();
        PrintPowerCost();
    }

    void Update() {
        foreach (GameObject skin in displaySkin) {
            skin.transform.Rotate(new Vector3(0f, 0.1f, 0f), Space.Self);
        }
    }

    public void SwitchLeft() {
        displayRotate.transform.Rotate(0, -360f / NumOfSkin, 0);
        selected--;
        if (selected < 0) {
            selected = NumOfSkin - 1;
        }
        print("selected: " + selected);
        CheckBuyable();
    }

    public void SwitchRight() {
        displayRotate.transform.Rotate(0, 360f / NumOfSkin, 0);
        selected++;
        if (selected > NumOfSkin - 1) {
            selected = 0;
        }
        print("selected: " + selected);
        CheckBuyable();
    }

    public void PrintPowerCost() {
        ButtonPower.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade Power Pellet\n" + 
                            DataBaseManager.Instance.GetComponent<ShopManager>().GetPowerCost().ToString();
    }

    public void UpgradePower() {
        DataBaseManager.Instance.GetComponent<ShopManager>().BuyPower();
        PrintPowerCost();
    }

    public void CheckBuyable() {
        if (!DataBaseManager.Instance.GetComponent<ShopManager>().CheckHasSkin(selected)) {
            ButtonSkin.GetComponentInChildren<TextMeshProUGUI>().text = 
                "Buy\n" + DataBaseManager.Instance.GetComponent<ShopManager>().shop.skinCost[selected];
        } else {
            ButtonSkin.GetComponentInChildren<TextMeshProUGUI>().text  = "Select";
        }
    }

    public void ClickBuy() {
        print("Clicked");
        if (!DataBaseManager.Instance.GetComponent<ShopManager>().CheckHasSkin(selected)) {
            DataBaseManager.Instance.GetComponent<ShopManager>().BuySkin(selected);

        } else {
            if (selected != DataBaseManager.Instance.profile.UsingSkin) {
                DataBaseManager.Instance.profile.UsingSkin = selected;
                DataBaseManager.Instance.SaveData();
            }

        }
        CheckBuyable();
    }
}
