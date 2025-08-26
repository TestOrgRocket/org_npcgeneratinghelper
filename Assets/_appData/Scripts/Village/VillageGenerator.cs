using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VillageGenerator : MonoBehaviour
{
    List<NPC> villageData = new List<NPC>();
    public GameObject GeneratedVillagePanel;
    public Slider villagersCount;

    public GameObject NPCMiniCardObject;
    public Transform villageMiniCardContainer;
    public RectAutoSize rectAutoSize;
    void OnEnable()
    {
        villageData = DataManager.LoadVillageData();
        GeneratedVillagePanel.SetActive(villageData.Count > 0);
        if (villageMiniCardContainer.childCount == 0)
        {
            foreach (NPC npc in villageData)
            {
                GameObject minicard = Instantiate(NPCMiniCardObject, villageMiniCardContainer);
                minicard.GetComponent<VillageMiniCard>().SetNPC(npc);
                minicard.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UpdateNPCCard(npc);
                    NPCCard.SetActive(true);
                });
            }
        }
        rectAutoSize.ChangeSize();
        NPCCard.SetActive(false);
    }
    public void GenerateVillage()
    {
        villageData = new List<NPC>();
        for (int i = 0; i < villagersCount.value; i++)
        {
            villageData.Add(GenerateNewNpc());
        }
        determineRelationships();
        foreach (NPC npc in villageData)
        {
            GameObject minicard = Instantiate(NPCMiniCardObject, villageMiniCardContainer);
            minicard.GetComponent<VillageMiniCard>().SetNPC(npc);
            minicard.GetComponent<Button>().onClick.AddListener(() =>
            {
                UpdateNPCCard(npc);
                NPCCard.SetActive(true);
                
                Vector3 initialPos = Vector3.zero;
                NPCCard.transform.localPosition = new Vector3(initialPos.x + 2000f, initialPos.y, initialPos.z);
                NPCCard.transform.DOLocalMove(Vector3.zero, 1f);
            });
            
        }
        rectAutoSize.ChangeSize();
        GeneratedVillagePanel.SetActive(true);
        // DataManager.SaveVillageData(villageData);
    }
    public void SaveVillage()
    {
        foreach (NPC npc in villageData)
        {
            DataManager.CreateNPC(npc);
        }
        DataManager.SaveVillageData(villageData);
        DataManager.SaveNpcData();
    }
    public void DeleteVillage()
    {
        foreach (NPC npc in villageData)
        {
            DataManager.DeleteNPCData(npc);
        }
        villageData.Clear();
        foreach (Transform child in villageMiniCardContainer)
        {
            Destroy(child.gameObject);
        }
        GeneratedVillagePanel.SetActive(false);
        DataManager.DeleteExistingVillage();
    }

    void determineRelationships()
    {
        List<NPC> males = villageData.Where(n => n.isMale).ToList();
        List<NPC> females = villageData.Where(n => !n.isMale).ToList();

        List<NPC> availableMales = males.Where(m => !m.relationshipsIsSet).ToList();
        List<NPC> availableFemales = females.Where(f => !f.relationshipsIsSet).ToList();

        // Shuffle the lists for random pairing
        availableMales = availableMales.OrderBy(x => Random.value).ToList();
        availableFemales = availableFemales.OrderBy(x => Random.value).ToList();

        int maxPairs = Mathf.Min(availableMales.Count, availableFemales.Count);

        for (int i = 0; i < maxPairs; i++)
        {
            // Only create relationship with 50% chance for each potential pair
            if (Random.Range(0, 2) == 0)
            {
                NPC male = availableMales[i];
                NPC female = availableFemales[i];

                male.relationshipsIsSet = true;
                female.relationshipsIsSet = true;
                male.relationshipStatus = true;
                female.relationshipStatus = true;
                male.relationshipPartner = $"{female.firstName} {female.lastName}";
                female.relationshipPartner = $"{male.firstName} {male.lastName}";
            }
        }
    }

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

    public void DeleteNpc()
    {
        NPCCard.SetActive(false);
    }

    public NPC GenerateNewNpc()
    {
        NPC newNpc = new NPC();
        newNpc.id = UnityEngine.Random.Range(1000, 9999);
        newNpc.dateCreated = DateTime.Now;
        bool isMale = UnityEngine.Random.Range(0, 2) == 0;
        if (isMale)
        {
            newNpc.imageName = maleSprites[Random.Range(0, maleSprites.Count)].name;
        }
        else
        {
            newNpc.imageName = femaleSprites[Random.Range(0, femaleSprites.Count)].name;
        }
        npcCardBg.sprite = isMale ? maleBg : femaleBg;
        newNpc.isMale = isMale;
        var nameData = GenerateName(isMale);
        newNpc.firstName = nameData.firstName;
        newNpc.lastName = nameData.lastName;
        newNpc.relationshipsIsSet = false;
        newNpc.age = UnityEngine.Random.Range(18, 80);
        newNpc.height = (float)Math.Round(UnityEngine.Random.Range(1.5f, 2.1f), 2);
        newNpc.weight = (float)Math.Round(UnityEngine.Random.Range(50f, 120f), 1);
        int birthYear = DateTime.Now.Year - newNpc.age;
        int birthMonth = UnityEngine.Random.Range(1, 13);
        int birthDay = UnityEngine.Random.Range(1, DateTime.DaysInMonth(birthYear, birthMonth) + 1);
        newNpc.birthDay = new DateTime(birthYear, birthMonth, birthDay);
        newNpc.health = UnityEngine.Random.Range(70, 150);
        newNpc.mana = UnityEngine.Random.Range(10, 100);
        newNpc.strength = UnityEngine.Random.Range(1, 20);
        newNpc.agility = UnityEngine.Random.Range(1, 20);
        newNpc.intelligence = UnityEngine.Random.Range(1, 20);
        newNpc.luck = UnityEngine.Random.Range(1, 20);
        newNpc.stamina = UnityEngine.Random.Range(10, 100);
        newNpc.reputation = UnityEngine.Random.Range(-50, 100);
        newNpc.charisma = UnityEngine.Random.Range(1, 20);
        newNpc.gold = UnityEngine.Random.Range(0, 500);
        newNpc.occupation = GetRandomOccupation();
        return newNpc;
    }

    private (string firstName, string lastName) GenerateName(bool isMale)
    {
        List<string> firstNames;
        List<string> lastNames = new List<string>
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis",
            "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor",
            "Thomas", "Hernandez", "Moore", "Martin", "Jackson", "Thompson",
            "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis",
            "Robinson", "Walker", "Perez", "Hall", "Young", "Allen", "Sanchez",
            "Wright", "King", "Scott", "Green", "Baker", "Adams", "Nelson",
            "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter",
            "Phillips", "Evans", "Turner", "Torres"
        };

        if (isMale)
        {
            firstNames = new List<string>
            {
                "James", "John", "Robert", "Michael", "William", "David", "Richard",
                "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul",
                "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian",
                "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", "Timothy",
                "Jose", "Larry", "Jeffrey", "Frank", "Scott", "Eric", "Stephen",
                "Andrew", "Raymond", "Gregory", "Joshua", "Jerry", "Dennis", "Walter",
                "Patrick", "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur",
                "Ryan", "Roger"
            };
        }
        else
        {
            firstNames = new List<string>
            {
                "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer",
                "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen",
                "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon",
                "Michelle", "Laura", "Sarah", "Kimberly", "Deborah", "Jessica",
                "Shirley", "Cynthia", "Angela", "Melissa", "Brenda", "Amy", "Anna",
                "Rebecca", "Virginia", "Kathleen", "Pamela", "Martha", "Debra",
                "Amanda", "Stephanie", "Carolyn", "Christine", "Marie", "Janet",
                "Catherine", "Frances", "Ann", "Joyce", "Diane"
            };
        }

        string firstName = firstNames[UnityEngine.Random.Range(0, firstNames.Count)];
        string lastName = lastNames[UnityEngine.Random.Range(0, lastNames.Count)];

        return (firstName, lastName);
    }

    private string GetRandomOccupation()
    {
        List<string> occupations = new List<string>
        {
            "Blacksmith", "Merchant", "Guard", "Farmer", "Baker", "Innkeeper",
            "Tailor", "Carpenter", "Miner", "Hunter", "Fisherman", "Scholar",
            "Priest", "Bard", "Alchemist", "Herbalist", "Soldier", "Knight",
            "Noble", "Servant", "Cook", "Shepherd", "Potter", "Weaver", "Mason"
        };

        return occupations[UnityEngine.Random.Range(0, occupations.Count)];
    }

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
