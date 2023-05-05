using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * PanelSwitcher class
 *
 * used for switching between UI panels
 */

public class PanelSwitcher : MonoBehaviour {
    
    // switch active panel by the panel name
    public void SwitchActivePanelByName(string targetName) {
        List<GameObject> panels = FindPanels();
        for (int i = 0; i < panels.Count; i++) {
            // if the panel name matches the target name, set it to active
            // conditions are not case sensitive and allow to omit the "panel-" prefix
            if (panels[i].name.ToLower() == "panel-" + targetName.ToLower() || panels[i].name.ToLower() == targetName.ToLower()) {
                panels[i].SetActive(true);
            } else {
                panels[i].SetActive(false);
            }
        }
    }
    
    // Get all panels in the scene
    private List<GameObject> FindPanels() {
        return FindObjectsByPrefix("panel", 5);
    }
    
    // Get a panel *GameObject* by the panel name
    public GameObject GetPanelByName(string targetName) {
        List<GameObject> panels = FindPanels();
        for (int i = 0; i < panels.Count; i++) {
            if (panels[i].name.ToLower() == "panel-" + targetName.ToLower() || panels[i].name.ToLower() == targetName.ToLower()) {
                return panels[i].gameObject;
            } 
        }
        throw new ArgumentNullException();
    }
    
    // Find a GameObject List by the name prefix
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
    
    // Find a GameObject list by the tag
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