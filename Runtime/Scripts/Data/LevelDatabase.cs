using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Zyntra.Data
{
    public class LevelDatabase
    {
        public static string RootPath => Path.Combine(Application.persistentDataPath, "Zyntra");
        public static string CachePath => Path.Combine(RootPath, "zyntra_index_cache.json");

        [Serializable]
        public class CacheEntry
        {
            public string folderName;
            public long lastModifiedTicks;
            public string jsonContent;
        }

        [Serializable]
        public class DatabaseContainer
        {
            public List<CacheEntry> entries = new();
        }

        private static List<LevelMetadata> _loadedMetadata = new();
        public static IReadOnlyList<LevelMetadata> Levels => _loadedMetadata.AsReadOnly();

        public static void Reindex()
        {
            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);

                var warning = File.CreateText(Path.Combine(RootPath, "README.txt"));
                warning.WriteLine("WARNING: THIS FOLDER CONTAINS CRITICAL FILES TO THE GAME\n" +
                                  "PLEASE DO NOT TOUCH ANYTHING!!!");
                warning.Close();
            }

            DatabaseContainer cacheContainer = new();
            if (File.Exists(CachePath))
            {
                try
                {
                    var cacheJson = File.ReadAllText(CachePath);
                    cacheContainer = JsonUtility.FromJson<DatabaseContainer>(cacheJson);
                }
                catch
                {
                    cacheContainer = new DatabaseContainer();
                }
            }

            var existingLevel = new Dictionary<string, CacheEntry>();
            foreach (var entry in cacheContainer.entries)
            {
                existingLevel[entry.folderName] = entry;
            }

            List<CacheEntry> updatedEntries = new();
            List<LevelMetadata> loadedList = new();
            var isDirty = false;

            var levelDirectories = Directory.GetDirectories(RootPath);

            foreach (var dirPath in levelDirectories)
            {
                var folderName = Path.GetFileName(dirPath);
                var currentModTime = Directory.GetLastWriteTime(dirPath).Ticks;

                if (existingLevel.TryGetValue(folderName, out var cachedEntry) &&
                    cachedEntry.lastModifiedTicks == currentModTime)
                {
                    updatedEntries.Add(cachedEntry);

                    var meta = DataManager.LoadMetadata(Path.Combine(dirPath, "metadata.json"));
                    if (meta != null) loadedList.Add(meta);
                    continue;
                }

                var metadataPath = Path.Combine(dirPath, "metadata.json");
                if (File.Exists(metadataPath))
                {
                    var meta = DataManager.LoadMetadata(metadataPath);

                    if (meta == null) continue;

                    loadedList.Add(meta);
                    updatedEntries.Add(new CacheEntry
                    {
                        folderName = folderName,
                        lastModifiedTicks = currentModTime,
                        jsonContent = File.ReadAllText(metadataPath)
                    });
                    isDirty = true;
                }
            }

            if (updatedEntries.Count != cacheContainer.entries.Count)
            {
                isDirty = true;
            }

            cacheContainer.entries = updatedEntries;
            _loadedMetadata = loadedList;

            if (isDirty)
            {
                File.WriteAllText(CachePath, JsonUtility.ToJson(cacheContainer, true));
                Debug.Log($"[Zyntra] LevelDatabase reindexed and cached ({_loadedMetadata.Count} levels).");
            }
            else
            {
                Debug.Log($"[Zyntra] LevelDatabase initialized from cache ({_loadedMetadata.Count} levels).");
            }
        }
    }
}