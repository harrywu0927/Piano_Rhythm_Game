using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.EventSystems.EventTrigger))]
public class MouseHoverOnBtns : MonoBehaviour //���˵��а�ťЧ��
{
    // Start is called before the first frame update
    public Button button;
    void Start()
    {
        button = this.GetComponent<Button>();
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        // �������¼� 
        entry1.eventID = EventTriggerType.PointerEnter;
        // ��껬���¼� 
        entry2.eventID = EventTriggerType.PointerExit;
        entry1.callback = new EventTrigger.TriggerEvent();
        entry1.callback.AddListener(MouseIn);
        trigger.triggers.Add(entry1);

        entry2.callback = new EventTrigger.TriggerEvent();
        entry2.callback.AddListener(MouseOut);
        trigger.triggers.Add(entry2);
    }

    private void MouseOut(BaseEventData pointData)
    {
        button.transform.localScale = new Vector3(1.21f, 1.92f, 1f);
    }

    private void MouseIn(BaseEventData pointData)
    {
        
        button.transform.localScale = new Vector3(1.41f,2.19f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
