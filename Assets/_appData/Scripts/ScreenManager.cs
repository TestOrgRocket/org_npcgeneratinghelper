using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Advertisement.IosSupport;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject npcGeneration;
    public GameObject villageInfo;
    public GameObject AllNPCsScreen;
    public GameObject privacyPolicyText;

    void OnEnable()
    {
        // OpenMenu();
        StartCoroutine(delayActions());
    }
    IEnumerator delayActions()
    {
        yield return new WaitForSeconds(0.5f);
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
    }
    public void OpenPrivacyPolicy()
    {
        closeScreens();
        privacyPolicyText.SetActive(true);
        privacyPolicyText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
    }
    public void OpenNpcGeneration()
    {
        closeScreens();
        npcGeneration.SetActive(true);
        npcGeneration.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
    }
    public void OpenAllNPCsScreen()
    {
        closeScreens();
        AllNPCsScreen.SetActive(true);
        AllNPCsScreen.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
    }
    public void OpenVillageInfo()
    {
        closeScreens();
        villageInfo.SetActive(true);
        villageInfo.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
    }
    public void OpenMainMenu()
    {
        closeScreens();
        mainMenu.SetActive(true);
        mainMenu.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
    }
    void closeScreens()
    {
        mainMenu.SetActive(false);
        npcGeneration.SetActive(false);
        villageInfo.SetActive(false);
        AllNPCsScreen.SetActive(false);
        privacyPolicyText.SetActive(false);
        privacyPolicyText.transform.localScale = Vector3.zero;
        mainMenu.transform.localScale = Vector3.zero;
        npcGeneration.transform.localScale = Vector3.zero;
        villageInfo.transform.localScale = Vector3.zero;
        AllNPCsScreen.transform.localScale = Vector3.zero;
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
