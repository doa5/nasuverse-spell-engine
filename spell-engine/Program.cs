using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Application;
using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

var services = new ServiceCollection();
services.AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Debug));
services.AddSingleton<TrainingSession>();

using ServiceProvider provider = services.BuildServiceProvider();

ILogger logger = provider.GetRequiredService<ILogger<Program>>();
ILogger<Character> charLogger = provider.GetRequiredService<ILogger<Character>>();
ILogger<ResourcePool> resourceLogger = provider.GetRequiredService<ILogger<ResourcePool>>();

logger.LogInformation("Starting Taiga Dojo sandbox");

TrainingSession session = provider.GetRequiredService<TrainingSession>();

bool playAgain;
do
{
    WorldState world = new WorldState();
    IReadOnlyList<Character> roster = CharacterFactory.CreateRoster(CharacterCatalog.Roster, charLogger, resourceLogger);
    playAgain = session.Run(roster, world);
} while (playAgain);

