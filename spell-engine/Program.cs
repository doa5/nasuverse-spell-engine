using NasuverseSpellEngine;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Debug));
ILogger logger = loggerFactory.CreateLogger<Program>();
ILogger<Character> charLogger = loggerFactory.CreateLogger<Character>();
ILogger<ResourcePool> resourceLogger = loggerFactory.CreateLogger<ResourcePool>();
ILogger<TaigaDojo> dojoLogger = loggerFactory.CreateLogger<TaigaDojo>();

logger.LogInformation("Starting Taiga Dojo sandbox");

TaigaDojo dojo = new TaigaDojo(100, dojoLogger);
Character aoko = CreateAoko(charLogger, resourceLogger);
Character arcuied = CreateArcueid(charLogger, resourceLogger);
Character selectedCharacter = aoko;

Console.WriteLine("Select a character:");
Console.WriteLine($"1. {aoko.Name}");
Console.WriteLine($"2. {arcuied.Name}\n");

bool selectingCharacter = true;
while (selectingCharacter)
{
    Console.Write("Choose a character: ");
    string characterInput = Console.ReadLine()!;

    if (!int.TryParse(characterInput, out int characterChoice))
    {
        logger.LogWarning("Invalid character input (non-numeric): {Input}", characterInput);
        Console.WriteLine("That's not a number. Try again.\n");
        continue;
    }

    logger.LogDebug("User selected character {Choice}", characterChoice);

    if (characterChoice == 1)
    {
        selectedCharacter = aoko;
        selectingCharacter = false;
    }
    else if (characterChoice == 2)
    {
        selectedCharacter = arcuied;
        selectingCharacter = false;
    }
    else
    {
        logger.LogWarning("Choice out of range: {Choice}", characterChoice);
        Console.WriteLine("Please choose 1 or 2.\n");
    }
}

Console.WriteLine($"Welcome to the Taiga Dojo with {selectedCharacter.Name}!");
Console.WriteLine($"Initial Mana: {selectedCharacter.Resources.Mana}, Dojo HP: {dojo.TargetHP}\n");

bool isTraining = true;
while (isTraining)
{
    Console.WriteLine($"--- {selectedCharacter.Name}'s Spells ---");
    for (int i = 0; i < selectedCharacter.AvailableSpells.Count; i++)
    {
        Spell spell = selectedCharacter.AvailableSpells[i];
        Console.WriteLine($"{i + 1}. {spell.Name} (Cost: {spell.ManaCost}, Damage: {spell.Damage})");
    }
    int exitOption = selectedCharacter.AvailableSpells.Count + 1;
    Console.WriteLine($"{exitOption}. Exit\n");

    Console.Write("Choose a spell: ");
    string input = Console.ReadLine()!; // ! means "trust me, it's not null"

    if (!int.TryParse(input, out int choice))
    {
        logger.LogWarning("Invalid input (non-numeric): {Input}", input);
        Console.WriteLine("That's not a number. Try again.\n");
        continue;
    }

    logger.LogDebug("User selected {Choice}", choice);

    if (choice < 1 || choice > exitOption)
    {
        logger.LogWarning("Choice out of range: {Choice}", choice);
        Console.WriteLine("Invalid choice. Try again.\n");
        continue;
    }

    // Allow exit even when dojo destroyed
    if (choice == exitOption)
    {
        logger.LogInformation("User selected Exit");
        isTraining = false;
        continue;
    }

    if (dojo.TargetHP <= 0)
    {
        logger.LogInformation("Attempt to cast on destroyed dojo (HP {HP})", dojo.TargetHP);
        Console.WriteLine("Taiga: The dojo has already been destroyed! What are you doing??\n");
        continue;
    }

    // Dispatch by index (scales with number of spells)
    int spellIndex = choice - 1; 
    if (spellIndex < 0 || spellIndex >= selectedCharacter.AvailableSpells.Count)
    {
        logger.LogWarning("Computed spell index out of range: {Index}", spellIndex);
        Console.WriteLine("Invalid spell selection. Try again.\n");
        continue;
    }

    // Single exit flag controls loop
    isTraining = HandleSpellCast(selectedCharacter, dojo, spellIndex);

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

static Character CreateAoko(ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
{
    var pool = new ResourcePool(200, resourceLogger);
    Character aoko = new Character("Aoko", pool, logger);

    Spell snapAndDraw = new Spell("Snap & Draw", manaCost: 20, new DamageEffect(5));
    Spell earthlightStarbow = new Spell("Earthlight Starbow", manaCost: 50, new DamageEffect(35));

    aoko.AvailableSpells.Add(earthlightStarbow);
    aoko.AvailableSpells.Add(snapAndDraw);

    return aoko;
}

static Character CreateArcueid(ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
{
    var pool = new ResourcePool(500, resourceLogger);
    Character arcuied = new Character("Arcueid", pool, logger);

    Spell mysticEyes = new Spell("Mystic Eyes of Enchantment", manaCost: 15, new VulnerableEffect());
    Spell meltyBlood = new Spell("Melty Blood", manaCost: 20, new DamageEffect(25));

    arcuied.AvailableSpells.Add(meltyBlood);
    arcuied.AvailableSpells.Add(mysticEyes);

    return arcuied;
}
