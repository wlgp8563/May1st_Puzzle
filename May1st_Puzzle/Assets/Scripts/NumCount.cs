using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumCount : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector3 originalPosition;
    private BackNumControl backNumControl;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        backNumControl = FindObjectOfType<BackNumControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backNumControl.CpmpareBackNumIndex(this);
        //Debug.Log("드래그");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backNumControl.ResetBaseIndex();
        backNumControl.BackNum.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //backNumControl.CpmpareBackNumIndex(this);
        //Debug.Log("드래그");
    }
}
