using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

/*
 * EventBus
 *
 * EventBus is to used for managing * IN GAME * events only (e.g. Game Start, Game Stop, etc.)
 * It aims at reducing the coupling between classes.
 */

// GameEvent is a list of events that can be published, four events are defined here:
public enum GameEvent {
    START, PAUSE, RESUME, STOP
}

public class EventBus {
    
    // stateList is a list of all possible events for better implementation
    private static List<string> stateList = new List<string> {
        "START", "PAUSE", "RESUME", "STOP"
    };
    
    // Events is a dictionary that maps GameEvent to UnityEvent
    private static readonly IDictionary<GameEvent, UnityEvent> 
        Events = new Dictionary<GameEvent, UnityEvent>();
    
    // a public method that converts a string to GameEvent (enum type)
    public static GameEvent StringToGameEvent(string text) {
        text = text.ToUpper(); // convert to upper case
        if (!stateList.Contains(text)) {
            throw new ArgumentNullException(text + " is not Found in GameEvent!");
        }
        GameEvent result;
        result = (GameEvent)System.Enum.Parse(typeof(GameEvent), text); // convert to enum type
        return result;
    }    
    
    // a public method that allows subscribers to subscribe to a specific event and the corresponding listener
    public static void Subscribe(GameEvent eventType, UnityAction listener) {
        UnityEvent thisEvent;
        
        // if the event is already in the dictionary, add the listener to the event
        if (Events.TryGetValue(eventType, out thisEvent)) {
            thisEvent.AddListener(listener);
        }
        // if the event is not in the dictionary, create a new event and add the listener to the event
        else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Events.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe(GameEvent type, UnityAction listener) {
        UnityEvent thisEvent;
        
        // if the event is in the dictionary, remove the listener from the event
        if (Events.TryGetValue(type, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(GameEvent type) {
        UnityEvent thisEvent;
        // Debug.Log(type.ToString() + " is published");
        if (Events.TryGetValue(type, out thisEvent)) {
            thisEvent.Invoke(); // invoke the event
        }
    }
}