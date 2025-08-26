using System;
using System.Globalization;

[Serializable]
public class NPC
{
    public int id;
    public DateTime dateCreated;

    // Основные характеристики
    public string imageName;
    public string firstName;
    public string lastName;
    // public string nickname; // Прозвище
    public int age;
    // public string description;
    public DateTime birthDay;
    public float height; // Рост
    public float weight; // Вес

    public bool isMale;

    public bool relationshipsIsSet;
    public bool relationshipStatus;
    public string relationshipPartner;

    public int health;
    public int mana;
    public int strength;
    public int agility;
    public int intelligence;
    public int luck;
    public int stamina; // Выносливость
    public int reputation; // Репутация (от -100 до 100)
    public int charisma; // Харизма (влияет на диалоги)
    public int gold; // Количество золота
    public string occupation; // Профессия (кузнец, торговец и т.д.)
}

[Serializable]
public class NPCList
{
    public NPC[] npcList;
}
