using System;

public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}

public class Game
{
    private Player player;

    public Game()
    {
        
        Console.WriteLine("Добро пожаловать, воин! Назови себя:");
        string playerName = Console.ReadLine();

        Aid aidKit = new Aid("Средняя аптечка", 10);
        Weapon weapon = new Weapon("Меч Фламберг", 20, 10);
        player = new Player(playerName, 100, aidKit, weapon);

        Console.WriteLine($"Ваше имя {player.Name}!");
        Console.WriteLine($"Вам был ниспослан {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.AidKit.Name} ({player.AidKit.HealAmount}hp).");
        Console.WriteLine($"У вас {player.MaxHealth}hp.\n");
    }

    public void Start()
    {
        while (player.CurrentHealth > 0)
        {
            Enemy enemy = GenerateRandomEnemy();
            Console.WriteLine($"Killer111 встречает врага {enemy.Name} ({enemy.CurrentHealth}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");

            while (enemy.CurrentHealth > 0 && player.CurrentHealth > 0)
            {
                Console.WriteLine("Что вы будете делать?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        player.DealDamage(enemy);
                        if (enemy.CurrentHealth > 0)
                        {
                            enemy.Attack(player);
                        }
                        break;
                    case "2":
                        Console.WriteLine($"{player.Name} пропустил ход.");
                        enemy.Attack(player); 
                        break;
                    case "3":
                        player.UseAid();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp\n");
            }

            if (player.CurrentHealth <= 0)
            {
                Console.WriteLine("Вы погибли. Игра окончена.");
            }
            else
            {
                Console.WriteLine($"Вы победили {enemy.Name}! Получаете 10 очков.");
                player.Score += 10;
            }
        }
    }

    private Enemy GenerateRandomEnemy()
    {
        Random rand = new Random();
        string name = $"Варвар {rand.Next(1, 100)}";
        int maxHealth = rand.Next(30, 100);
        Weapon enemyWeapon = new Weapon($"Оружие {rand.Next(1, 100)}", rand.Next(5, 15), 5);
        return new Enemy(name, maxHealth, enemyWeapon);
    }
}

public class Player
{
    public string Name { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public Aid AidKit { get; private set; }
    public Weapon Weapon { get; private set; }
    public int Score { get; set; }

    public Player(string name, int maxHealth, Aid aidKit, Weapon weapon)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        AidKit = aidKit;
        Weapon = weapon;
        Score = 0;
    }

    public void DealDamage(Enemy enemy)
    {
        enemy.TakeDamage(Weapon.Damage);
        Weapon.Use();
    }

    public void UseAid()
    {
        if (AidKit != null)
        {
            CurrentHealth += AidKit.HealAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            Console.WriteLine($"{Name} использовал аптечку. У вас {CurrentHealth}hp.");
            AidKit = null; 
        }
        else
        {
            Console.WriteLine("Аптечка уже использована.");
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;
        Console.WriteLine($"{Name} получил {damage} урона. У вас {CurrentHealth}hp.");
    }
}

public class Enemy
{
    public string Name { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public Weapon Weapon { get; private set; }

    public Enemy(string name, int maxHealth, Weapon weapon)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Weapon = weapon;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;
        Console.WriteLine($"{Name} получил {damage} урона. У него {CurrentHealth}hp.");
    }

    public void Attack(Player player)
    {
        player.TakeDamage(Weapon.Damage);
        Console.WriteLine($"{Name} атакует {player.Name} на {Weapon.Damage} урона.");
    }
}

public class Aid
{
    public string Name { get; private set; }
    public int HealAmount { get; private set; }

    public Aid(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }
}

public class Weapon
{
    public string Name { get; private set; }
    public int Damage { get; private set; }
    public int Durability { get; private set; }

    public Weapon(string name, int damage, int durability)
    {
        Name = name;
        Damage = damage;
        Durability = durability;
    }

    public void Use()
    {
        if (Durability > 0)
        {
            Durability--;
        }
        else
        {
            Console.WriteLine($"{Name} сломано!");
        }
    }
}