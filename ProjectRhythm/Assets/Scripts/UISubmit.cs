using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UISubmit : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		EventTrigger myEventTrigger = GetComponent<EventTrigger>(); //you need to have an EventTrigger component attached this gameObject
		myEventTrigger.AddListener(EventTriggerType.Submit, onSubmitListener);
	}

	void onSubmitListener(PointerEventData eventData)
    {
		//put your logic here...
		Debug.Log("JOHNNY TEST");
    }
}
