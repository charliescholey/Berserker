using UnityEngine;

/*
This is a temporary class used for creating characters to be displayed
in the store and in the customize battalion page until the functionality
for characters is created.

There are static example characters. These are not a reflection of the actual
characters and what abilities they will have.
*/

public class TempCharacter
{
    string name;
    string bio;
    string ability;
    string buffs;
    string nerfs;
    int cost;
    Color color;
    public static TempCharacter exampleCharacter1 = new TempCharacter("Dr. Vale", "A former field medic with a background in experimental medicine", "Healing", "Can revive a solider with less than 50 HP once per mission", "Deals 30% less damage with all weapons", 300, Color.red);
    public static TempCharacter exampleCharacter2 = new TempCharacter("Evan Hayes", "A former engineer turned resistance fighter", "Damage to enemies", "Deals 25% more damage with grenades", "Has 15% worse aim", 200, Color.blue);
    public static TempCharacter exampleCharacter3 = new TempCharacter("Sam Turner", "A skilled marksman and former spy", "Long-range combat aim", "50% better long-range aim", "20% worse short-range aim", 400, Color.green);
    public static TempCharacter exampleCharacter4 = new TempCharacter("Alma Reyes", "A master of stealth and subterfuge", "Movement", "Can move 50% further in a single turn", "Takes 25% more damage from all sources", 100, Color.magenta);

    public static TempCharacter[] exampleCharacters = {exampleCharacter1, exampleCharacter2, exampleCharacter3, exampleCharacter4};
    public TempCharacter(string name, string bio, string ability, string buffs, string nerfs, int cost, Color color)
    {
        this.name = name;
        this.bio = bio;
        this.ability = ability;
        this.buffs = buffs;
        this.nerfs = nerfs;
        this.color = color;
        this.cost = cost;
    }
    
    // Getters
    public string GetName() { return name; }
    public string GetBio() { return bio; }
    public string GetAbility() { return ability; }
    public string GetBuffs() { return buffs; }
    public string GetNerfs() { return nerfs; }
    public int GetCost() { return cost; }
    public Color GetColor() { return color; }
}
