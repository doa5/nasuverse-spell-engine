using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Application;
using NasuverseSpellEngine.Domain;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Debug));
ILogger logger = loggerFactory.CreateLogger<Program>();
ILogger<Character> charLogger = loggerFactory.CreateLogger<Character>();
ILogger<ResourcePool> resourceLogger = loggerFactory.CreateLogger<ResourcePool>();
ILogger<TaigaDojo> dojoLogger = loggerFactory.CreateLogger<TaigaDojo>();
ILogger<TrainingSession> sessionLogger = loggerFactory.CreateLogger<TrainingSession>();

logger.LogInformation("Starting Taiga Dojo sandbox");

TaigaDojo dojo = new TaigaDojo(100, dojoLogger);
Character aoko = CharacterFactory.CreateAoko(charLogger, resourceLogger);
Character arcueid = CharacterFactory.CreateArcueid(charLogger, resourceLogger);

TrainingSession session = new TrainingSession(sessionLogger);
Character selectedCharacter = session.SelectCharacter(aoko, arcueid);
session.Run(selectedCharacter, dojo);
