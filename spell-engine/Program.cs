using NasuverseSpellEngine;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
ILogger logger = loggerFactory.CreateLogger<Program>();
ILogger<Character> charLogger = loggerFactory.CreateLogger<Character>();

logger.LogInformation("Starting Taiga Dojo sandbox");

TaigaDojo dojo = new TaigaDojo();
Character aoko = CreateAoko();

Console.WriteLine($"Welcome to the Taiga Dojo with {aoko.Name}!");
Console.WriteLine($"Initial Mana: {aoko.Resources.Mana}, Dojo HP: {dojo.TargetHP}\n");

bool isTraining = true;
while (isTraining)
{
    Console.WriteLine("--- Aoko's Spells ---");
    for (int i = 0; i < aoko.AvailableSpells.Count; i++)
    {
        Spell spell = aoko.AvailableSpells[i];
        Console.WriteLine($"{i + 1}. {spell.Name} (Cost: {spell.ManaCost}, Damage: {spell.Damage})");
    }
    int exitOption = aoko.AvailableSpells.Count + 1;
    Console.WriteLine($"{exitOption}. Exit\n");

    Console.Write("Choose a spell: ");
    string input = Console.ReadLine()!; // ! means "trust me, it's not null"

    if (!int.TryParse(input, out int choice))
    {
        Console.WriteLine("That's not a number. Try again.\n");
        continue;
    }

    if (choice < 1 || choice > exitOption)
    {
        Console.WriteLine("Invalid choice. Try again.\n");
        continue;
    }

    if (dojo.TargetHP <= 0)
    {
        Console.WriteLine("Taiga: The dojo has already been destroyed! What are you doing??\n");
        continue;
    }

    isTraining = choice switch
    {
        1 => HandleSpellCast(aoko, dojo, 0),
        2 => HandleSpellCast(aoko, dojo, 1),
        3 => false,
        _ => true
    };

    if (dojo.TargetHP <= 0)
    {
        Console.WriteLine("The Taiga Dojo has been destroyed!\n");
    }
}

static bool HandleSpellCast(Character character, TaigaDojo dojo, int spellIndex)
{
    string result = character.CastSpell(character.AvailableSpells[spellIndex], dojo);
    Console.WriteLine(result);
    Console.WriteLine($"{character.Name}'s Mana: {character.Resources.Mana}, Dojo HP: {dojo.TargetHP}\n");
    return true;
}

static Character CreateAoko()
{
    Character aoko = new Character("Aoko", new ResourcePool(initialMana: 500));

    Spell snapAndDraw = new Spell("Snap & Draw", manaCost: 20, damage: 5);
    Spell earthlightStarbow = new Spell("Earthlight Starbow", manaCost: 50, damage: 35);

    aoko.AvailableSpells.Add(earthlightStarbow);
    aoko.AvailableSpells.Add(snapAndDraw);

    return aoko;
}