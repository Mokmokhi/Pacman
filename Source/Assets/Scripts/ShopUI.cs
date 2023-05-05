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
    }
    // Keeps the displaying model spinning.
    void Update() {
        foreach (GameObject skin in displaySkin) {
            skin.transform.Rotate(new Vector3(0f, 0.1f, 0f), Space.Self);
        }
    }
    // function SwitchLeft to switch to the left item.
    public void SwitchLeft() {
        displayRotate.transform.Rotate(0, -360f / NumOfSkin, 0);
        selected--;
        if (selected < 0) {
            selected = NumOfSkin - 1;
        }
        print("selected " + selected);
        CheckBuyable();
    }
    // function SwitchRight to switch to the right item.

    public void SwitchRight() {
        displayRotate.transform.Rotate(0, 360f / NumOfSkin, 0);
        selected++;
        if (selected > NumOfSkin - 1) {
            selected = 0;
        }
        print("selected " + selected);
        CheckBuyable();
    }
    // function PrintPowerCost to print the cost of upgrading the powerpellet.
    public void PrintPowerCost() {
        ButtonPower.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade Power Pellet\n" + 
                            DataBaseManager.Instance.GetComponent<ShopManager>().GetPowerCost().ToString();
    }
    // function UpgradePower to upgrade the powerpellet.
    public void UpgradePower() {
        DataBaseManager.Instance.GetComponent<ShopManager>().BuyPower();
        PrintPowerCost();
    }
    // function CheckBuyable to check if the skin has been bought or not.
    public void CheckBuyable() {
        if (!DataBaseManager.Instance.GetComponent<ShopManager>().CheckHasSkin(selected)) {
            ButtonSkin.GetComponentInChildren<TextMeshProUGUI>().text = 
                "Buy\n" + DataBaseManager.Instance.GetComponent<ShopManager>().shop.skinCost[selected];
        } else {
            if (selected != DataBaseManager.Instance.profile.UsingSkin)
                ButtonSkin.GetComponentInChildren<TextMeshProUGUI>().text  = "Select";
            else
            ButtonSkin.GetComponentInChildren<TextMeshProUGUI>().text  = "Selected";
        }
    }
    // function ClickBuy to buy the selected skin or use the selected skin if player has it.
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
