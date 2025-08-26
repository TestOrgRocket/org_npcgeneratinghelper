using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    public List<GameObject> possibleEffects;
    public Transform effectContainer;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos1 = Input.mousePosition;
            clickPos1.z = 10;
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // clickPos.x = clickPos.x - Screen.width / 2;
            // clickPos.y = clickPos.y - Screen.height / 2;
            // clickPos.z = 0;
            clickPos.z = 0;
            GameObject effect = Instantiate(possibleEffects[Random.Range(0, possibleEffects.Count)], clickPos, Quaternion.identity,effectContainer);
            effect.SetActive(true);
        }
    }
}