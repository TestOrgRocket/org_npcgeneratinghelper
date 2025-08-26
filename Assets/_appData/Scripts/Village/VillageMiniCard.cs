using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageMiniCard : MonoBehaviour
{
    public Image NPCImage;
    public Image miniCardBg;
    public Sprite femaleBg, maleBg;
    public List<Sprite> NPCImages = new List<Sprite>();
    public Text nameText;
    public Text ageText;

    NPC _selectedNPC;

    public void SetNPC(NPC npc)
    {
        _selectedNPC = npc;
        string imageName = npc.imageName;
        foreach (Sprite sprite in NPCImages)
        {
            if (sprite.name == imageName)
            {
                NPCImage.sprite = sprite;
                break;
            }
        }
        nameText.text = npc.firstName + " " + npc.lastName;
        ageText.text = "Age: " + npc.age.ToString();

        if (npc.isMale)
        {
            miniCardBg.sprite = maleBg;
        }
        else
        {
            miniCardBg.sprite = femaleBg;
        }
    }
}