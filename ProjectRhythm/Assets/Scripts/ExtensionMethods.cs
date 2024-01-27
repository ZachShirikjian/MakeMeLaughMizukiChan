using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//Code is from ben-rasooli on this thread
//https://discussions.unity.com/t/how-do-you-add-an-ui-eventtrigger-by-script/125158/4 

public static class ExtensionMethods
{
    //ADDS EVENT TRIGGER FOR SUBMIT 
    public static void AddListener(this EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }
}
