using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    public GameObject displayRotate;
    [SerializeField]
    public GameObject[] displaySkin;
    [SerializeField]
    public Button buyButton;
    private int NumOfSkin = 0;
    private int selected = 0;
    private Transform targetRot;

    void Awake() {
        NumOfSkin = displaySkin.Length;
    }

    void LateUpdate() {
        foreach (GameObject skin in displaySkin) {
            skin.transform.Rotate(0, 5f * Time.deltaTime, 0);
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

    public void CheckBuyable() {
        if (!DataBaseManager.Instance.GetComponent<ShopManager>().CheckHasSkin(selected)) {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = 
                "Buy\n" + DataBaseManager.Instance.GetComponent<ShopManager>().shop.skinCost[selected];
        } else {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Select";
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
