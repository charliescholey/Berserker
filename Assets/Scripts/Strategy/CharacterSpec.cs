using UnityEngine;

public class CharacterSpec : MonoBehaviour
{
    //basic stats of a character that can be changed 
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseMovementRange;
    public int currentHealth;
    //this is for if we decide to have class specific chcaracters
    public string characterClass; 
    //array of 3 potential moves, made them strings as i am not sure how we are going to implement them yet 
    public string[] generalAbilities = new string[3];
    // One unique ability specific to each character
    public string uniqueAbility;

    void Start()
    {
        // Initialize current health
        currentHealth = baseHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - baseDefense, 1);
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //not sure how we are going to go about having thr characters die but here is a function for it
    private void Die()
    {
        
        
    }
}
