using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRandomHeads : MonoBehaviour
{
    public List<Sprite> randomHeads;
    Image _headImage;
    void OnEnable()
    {
        _headImage = GetComponent<Image>();
        StartCoroutine(pickingCoroutine());
    }
    IEnumerator pickingCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            _headImage.sprite = randomHeads[Random.Range(0, randomHeads.Count)];
            yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
