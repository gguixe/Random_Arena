﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIHoverListener : MonoBehaviour
{
    public bool isUIOverride { get; private set; }

    void Update()
    {
        // It will turn true if hovering any UI Elements
        isUIOverride = EventSystem.current.IsPointerOverGameObject();
    }

    //public static class MouseOverUILayerObject
    //{
      //  public static bool IsPointerOverUIObject()
        //{
          //  PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            //eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //List<RaycastResult> results = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            //for (int i = 0; i < results.Count; i++)
            //{
            //    if (results[i].gameObject.layer == 5) //5 = UI layer
            //    {
            //        return true;
            //    }
            //}

            //return false;
        //}
    //}

}

