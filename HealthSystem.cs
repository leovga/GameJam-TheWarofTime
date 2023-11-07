
public class HealthSystem
{
    private int health;
    private int maxhealth;

    public HealthSystem(int HealthMax)
    {
        maxhealth = HealthMax;
        health = HealthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health < 0)
        {
            health = 0;
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if(health > maxhealth)
        {
            health = maxhealth;
        }
    }
}
