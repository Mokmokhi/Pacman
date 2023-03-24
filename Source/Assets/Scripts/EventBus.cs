using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent {
    START, PAUSE, RESUME, STOP
}

public class EventBus {
    private static List<string> stateList = new List<string> {
        "START", "PAUSE", "RESUME", "STOP"
    };

    private static readonly IDictionary<GameEvent, UnityEvent> 
        Events = new Dictionary<GameEvent, UnityEvent>();
    
    public static GameEvent StringToGameEvent(string text) {
        text = text.ToUpper();
        if (!stateList.Contains(text)) {
            throw new ArgumentNullException(text + " is not Found in GameEvent!");
        }
        GameEvent result;
        result = (GameEvent)System.Enum.Parse(typeof(GameEvent), text);
        return result;
    }    
        
    public static void Subscribe(GameEvent eventType, UnityAction listener) {
        UnityEvent thisEvent;
        if (Events.TryGetValue(eventType, out thisEvent)) {
            thisEvent.AddListener(listener);
        }
        else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Events.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe(GameEvent type, UnityAction listener) {
        UnityEvent thisEvent;
        if (Events.TryGetValue(type, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(GameEvent type) {
        UnityEvent thisEvent;
        Debug.Log(type.ToString() + " is published");
        if (Events.TryGetValue(type, out thisEvent)) {
            thisEvent.Invoke();
        }
    }
}