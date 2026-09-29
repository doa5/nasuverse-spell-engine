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

WorldState world = new WorldState();
Character aoko = CharacterFactory.Create(CharacterCatalog.Aoko, charLogger, resourceLogger);
Character arcueid = CharacterFactory.Create(CharacterCatalog.Arcueid, charLogger, resourceLogger);

TrainingSession session = provider.GetRequiredService<TrainingSession>();
Character selectedCharacter = session.SelectCharacter(aoko, arcueid);
session.Run(selectedCharacter, world);
