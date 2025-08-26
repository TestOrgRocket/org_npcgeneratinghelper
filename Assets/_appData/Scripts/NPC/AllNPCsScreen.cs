using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AllNPCsScreen : MonoBehaviour
{
    List<NPC> createdNPCs = new List<NPC>();
    public GameObject NPCMiniCardObject;
    public GameObject MiniCardContainer;
    public RectAutoSize2 rectAutoSize;

    public
    void OnEnable()
    {
        StartCoroutine(delayedStart());
    }

    IEnumerator delayedStart()
    {
        yield return null;
        DataManager.LoadNpcData();
        createdNPCs = DataManager.npcs;
        yield return null;
        foreach (Transform child in MiniCardContainer.transform)
        {
            Destroy(child.gameObject);
        }
        yield return new WaitWhile(() => createdNPCs.Count == 0);
        foreach (NPC npc in createdNPCs)
        {
            GameObject minicard = Instantiate(NPCMiniCardObject, MiniCardContainer.transform);
            minicard.GetComponent<AllNPCsMiniCard>().SetNPC(npc);
            minicard.GetComponent<Button>().onClick.AddListener(() =>
            {
                UpdateNPCCard(npc);
                DeleteNPCButton.interactable = true;
                DeleteNPCButton.onClick.RemoveAllListeners();
                DeleteNPCButton.onClick.AddListener(() =>
                {
                    DataManager.DeleteNPCData(npc);
                    NPCCard.SetActive(false);
                    StartCoroutine(delayedStart());
                });
                NPCCard.SetActive(true);
                Vector3 initialPos = Vector3.zero;
                NPCCard.transform.localPosition = initialPos + Vector3.right * 2000f;
                NPCCard.transform.DOLocalMove(Vector3.zero, 1f);
            });
        }
        yield return null;
        rectAutoSize.ChangeSize();
    }
    public Button DeleteNPCButton;

    public GameObject NPCCard;
    public List<Sprite> maleSprites, femaleSprites;
    public Image npcCardBg;
    public Sprite maleBg, femaleBg;
    public Sprite maleGender, femaleGender;
    public Image relationshipImage;
    public Sprite singleMale, singleFemale, inRelationship;
    public Image npcImage;
    public Image npcGender;
    public Text npcFirstName,
        npcLastName,
        npcHp,
        npcHeight,
        npcWeight,
        npcMana,
        npcAge,
        npcStrength,
        npcAgiility,
        npcLuck,
        npcStamina,
        npcReputation,
        npcCharisma,
        npcGold,
        npcOccupation,
        npcBirthDay,
        npcRelationship,
        npcIntelligence;

    private void UpdateNPCCard(NPC selectedNPC)
    {
        if (selectedNPC == null) return;

        NPCCard.SetActive(true);

        // Сначала очищаем все текстовые поля
        ClearAllTextFields();

        // Обновляем UI элементы с анимацией
        StartCoroutine(DelayedTextAnimation(selectedNPC));
    }

    private void ClearAllTextFields()
    {
        npcFirstName.text = "";
        npcLastName.text = "";
        npcHp.text = "";
        npcHeight.text = "";
        npcWeight.text = "";
        npcMana.text = "";
        npcAge.text = "";
        npcStrength.text = "";
        npcAgiility.text = "";
        npcLuck.text = "";
        npcStamina.text = "";
        npcReputation.text = "";
        npcCharisma.text = "";
        npcGold.text = "";
        npcOccupation.text = "";
        npcIntelligence.text = "";
        npcRelationship.text = "";
        npcBirthDay.text = "";
    }

    private IEnumerator DelayedTextAnimation(NPC selectedNPC)
    {
        // Список всех твинов
        List<Tween> allTweens = new List<Tween>();

        // Создаем твины для каждого текстового поля
        allTweens.Add(npcFirstName.DOText($"First Name: {selectedNPC.firstName}", 3f));
        allTweens.Add(npcLastName.DOText($"Last Name: {selectedNPC.lastName}", 3f));
        allTweens.Add(npcHp.DOText($"HP: {selectedNPC.health}", 3f));
        allTweens.Add(npcHeight.DOText($"Height: {selectedNPC.height}m", 3f));
        allTweens.Add(npcWeight.DOText($"Weight: {selectedNPC.weight}kg", 3f));
        allTweens.Add(npcMana.DOText($"Mana: {selectedNPC.mana}", 3f));
        allTweens.Add(npcAge.DOText($"Age: {selectedNPC.age}", 3f));
        allTweens.Add(npcStrength.DOText($"Strength: {selectedNPC.strength}", 3f));
        allTweens.Add(npcAgiility.DOText($"Agility: {selectedNPC.agility}", 3f));
        allTweens.Add(npcLuck.DOText($"Luck: {selectedNPC.luck}", 3f));
        allTweens.Add(npcStamina.DOText($"Stamina: {selectedNPC.stamina}", 3f));
        allTweens.Add(npcReputation.DOText($"Reputation: {selectedNPC.reputation}", 3f));
        allTweens.Add(npcCharisma.DOText($"Charisma: {selectedNPC.charisma}", 3f));
        allTweens.Add(npcGold.DOText($"Gold: {selectedNPC.gold}", 3f));
        allTweens.Add(npcOccupation.DOText($"Occupation: {selectedNPC.occupation}", 3f));
        allTweens.Add(npcIntelligence.DOText($"Intelligence: {selectedNPC.intelligence}", 3f));
        allTweens.Add(npcRelationship.DOText(selectedNPC.relationshipPartner, 3f));

        CultureInfo englishCulture = new CultureInfo("en-US");
        allTweens.Add(npcBirthDay.DOText($"Birthday: {selectedNPC.birthDay.ToString("dd MMMM yyyy", englishCulture)}", 3f));

        // Останавливаем все твины перед началом
        foreach (var tween in allTweens)
        {
            tween.Pause();
        }

        // Обновляем изображения отношений и гендера (без анимации)
        if (selectedNPC.relationshipStatus)
        {
            relationshipImage.sprite = inRelationship;
        }
        else
        {
            relationshipImage.sprite = selectedNPC.isMale ? singleMale : singleFemale;
        }

        // Устанавливаем иконку гендера
        bool isMale = true;
        bool isChecked = false;
        foreach (Sprite s in maleSprites)
        {
            if (s.name == selectedNPC.imageName)
            {
                isMale = true;
                isChecked = true;
                break;
            }
        }
        if (!isChecked) isMale = false;
        npcGender.sprite = isMale ? maleGender : femaleGender;

        // Устанавливаем изображение NPC
        if (isMale)
        {
            foreach (Sprite s in maleSprites)
            {
                if (s.name == selectedNPC.imageName)
                {
                    npcImage.sprite = s;
                    break;
                }
            }
        }
        else
        {
            foreach (Sprite s in femaleSprites)
            {
                if (s.name == selectedNPC.imageName)
                {
                    npcImage.sprite = s;
                    break;
                }
            }
        }

        // Запускаем твины с задержками
        foreach (var tween in allTweens)
        {
            tween.Play();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
