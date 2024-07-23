using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorButtonControl : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 5f;

    private void Update()
    {
        float horizontalScrollInput = Input.GetAxis("Horizontal");
        float verticalScrollInput = Input.GetAxis("Vertical");

        scrollRect.horizontalNormalizedPosition += horizontalScrollInput * scrollSpeed * Time.deltaTime;
        scrollRect.verticalNormalizedPosition += verticalScrollInput * scrollSpeed * Time.deltaTime;

        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition);
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }
}
