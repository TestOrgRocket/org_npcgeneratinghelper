using System.Collections;
using Unity.Advertisement.IosSupport;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public Transform rotator;
    void Update()
    {
        rotator.Rotate(new Vector3(0, 0, 130f) * Time.deltaTime);
    }
    void OnEnable()
    {
        StartCoroutine(l());
        IEnumerator l()
        {
            yield return new WaitForSeconds(2.5f);
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(1);
        }
    }
}