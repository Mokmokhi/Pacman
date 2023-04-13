using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour {

    public void SwitchActivePanelByName(string targetName) {
        List<GameObject> panels = FindPanels();
        for (int i = 0; i < panels.Count; i++) {
            if (panels[i].name.ToLower() == "panel-" + targetName.ToLower() || panels[i].name.ToLower() == targetName.ToLower()) {
                panels[i].SetActive(true);
            } else {
                panels[i].SetActive(false);
            }
        }
    }

    private List<GameObject> FindPanels() {
        return FindObjectsByPrefix("panel", 5);
    }

    public GameObject GetPanelByName(string targetName) {
        List<GameObject> panels = FindPanels();
        for (int i = 0; i < panels.Count; i++) {
            if (panels[i].name.ToLower() == "panel-" + targetName.ToLower() || panels[i].name.ToLower() == targetName.ToLower()) {
                return panels[i].gameObject;
            } 
        }
        throw new ArgumentNullException();
    }
    
    public List<GameObject> FindObjectsByPrefix(string p_prefix, int length, bool isCaseSensity = false) {
        RectTransform[] objs = Resources.FindObjectsOfTypeAll<RectTransform>() as RectTransform[];
        List<GameObject> results = new List<GameObject>();

        if (!isCaseSensity) {
            p_prefix = p_prefix.ToLower();
        }
        
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i].name.Length < length) {
                continue;
            } 
            if (!isCaseSensity) {
                if (objs[i].name.Substring(0, length).ToLower().Equals(p_prefix))
                    results.Add(objs[i].gameObject);
            }
            else {
                if (objs[i].name.Substring(0, length).Equals(p_prefix))
                    results.Add(objs[i].gameObject);
            }
        }
        return results;
    }
    
    public List<GameObject> FindObjectsByTag(string tag) {
        RectTransform[] objs = Resources.FindObjectsOfTypeAll<RectTransform>() as RectTransform[];
        List<GameObject> results = new List<GameObject>();
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i].CompareTag(tag)) {
                results.Add(objs[i].gameObject);
            }
        }
        return results;
    }
}