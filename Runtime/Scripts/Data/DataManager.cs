// Data management has always been the pain when developing a project, also
// todo: Check null errors cause I believe there are some.

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Zyntra.Objects;

namespace Zyntra.Data
{
    [Serializable]
    public class SerializedObjectWrapper
    {
        public string typeName;
        public string jsonContent;
    }

    [Serializable]
    public class SerializedMetadataWrapper
    {
        public string typeName;
        public string jsonContent;
    }

    [Serializable]
    public class LevelDataDTO
    {
        public List<SerializedObjectWrapper> objects = new();
    }

    public static class DataManager
    {
        private static readonly Dictionary<string, Type> TypeCache = new();

        private static Type ResolveType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            if (TypeCache.TryGetValue(typeName, out var cachedType))
                return cachedType;

            var type = Type.GetType(typeName);

            if (type == null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(typeName);
                    if (type != null) break;
                }
            }

            if (type != null)
                TypeCache[typeName] = type;

            return type;
        }

        public static TMetadata BuildLevel<TMetadata>(
            string targetDirectory,
            TMetadata metadata,
            List<LevelData> levelDataList) where TMetadata : LevelMetadata
        {
            if (string.IsNullOrEmpty(targetDirectory))
                throw new ArgumentException("[Zyntra] Failed to build level : Target directory can't be null or empty.",
                    nameof(targetDirectory));
            if (levelDataList == null || levelDataList.Count == 0)
                throw new ArgumentException("[Zyntra] Failed to build level : Please at least add one level data.",
                    nameof(levelDataList));
            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

            var generatedFiles = new List<string>();

            for (var i = 0; i < levelDataList.Count; i++)
            {
                var diffName = $"diff_{i}.json";
                var fullPath = Path.Combine(targetDirectory, diffName);

                SaveLevelDataToFile(levelDataList[i], fullPath);
                generatedFiles.Add(diffName);
            }

            metadata.levelFiles = generatedFiles.ToArray();

            var metadataPath = Path.Combine(targetDirectory, "metadata.json");
            var metadataJson = SaveMetadata(metadata);
            File.WriteAllText(metadataPath, metadataJson);

            return metadata;
        }

        public static string SaveLevelData(LevelData levelData)
        {
            if (levelData == null) throw new ArgumentNullException(nameof(levelData));

            var dto = new LevelDataDTO();

            foreach (var obj in levelData.Objects)
            {
                if (obj == null) continue;

                var objectType = obj.GetType();

                if (!objectType.IsDefined(typeof(SerializableAttribute), false))
                {
                    throw new InvalidOperationException(
                        $"[Zyntra] Cannot serialize '{objectType.Name}' " +
                        "Classes inheriting from TimelineObject MUST be marked as Serializable!");
                }

                dto.objects.Add(new SerializedObjectWrapper
                {
                    typeName = objectType.FullName,
                    jsonContent = JsonUtility.ToJson(obj)
                });
            }

            return JsonUtility.ToJson(dto, true /* Wish there was a "whatever" option */);
        }

        public static LevelData LoadLevelData(string jsonContent)
        {
            if (string.IsNullOrEmpty(jsonContent)) return new LevelData();

            var dto = JsonUtility.FromJson<LevelDataDTO>(jsonContent);
            var levelData = new LevelData();

            if (dto?.objects == null) return levelData;

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            // fuck you resharper it is going to be pain
            // in the balls to read this line
            foreach (var wrapper in dto.objects)
            {
                if (string.IsNullOrEmpty(wrapper.typeName)) continue;

                var type = ResolveType(wrapper.typeName);
                if (type != null)
                {
                    var reconstructedObj = (TimelineObject)JsonUtility.FromJson(wrapper.jsonContent, type);
                    levelData.Objects.Add(reconstructedObj);
                }
                else
                {
                    Debug.LogWarning($"[Zyntra] Could not resolve type '{wrapper.typeName}'");
                }
            }

            return levelData;
        }

        public static string SaveMetadata<T>(T metadata) where T : LevelMetadata
        {
            if (metadata == null) throw new ArgumentException(nameof(metadata));

            var type = metadata.GetType();

            if (!type.IsDefined(typeof(SerializableAttribute), false))
            {
                throw new InvalidOperationException(
                    $"[Zyntra] Cannot serialize '{type.Name}' " +
                    "Classes inheriting from LevelMetadata MUST be marked as Serializable!");
            }

            var wrapper = new SerializedMetadataWrapper
            {
                typeName = type.FullName,
                jsonContent = JsonUtility.ToJson(metadata)
            };

            return JsonUtility.ToJson(wrapper, true);
        }

        public static LevelMetadata LoadMetadata(string jsonContent)
        {
            if (string.IsNullOrEmpty(jsonContent)) return new LevelMetadata();

            var wrapper = JsonUtility.FromJson<SerializedMetadataWrapper>(jsonContent);

            if (wrapper == null &&
                // ReSharper disable once PossibleNullReferenceException
                (string.IsNullOrEmpty(wrapper.typeName) || string.IsNullOrEmpty(wrapper.jsonContent)))
                return JsonUtility.FromJson<LevelMetadata>(jsonContent);
            var type = ResolveType(wrapper.typeName);
            if (type != null)
            {
                return (LevelMetadata)JsonUtility.FromJson(wrapper.jsonContent, type);
            }

            Debug.LogWarning($"[Zyntra] Could not resolve type '{wrapper.typeName}'");
            return JsonUtility.FromJson<LevelMetadata>(wrapper.jsonContent);
        }

        public static void SaveLevelDataToFile(LevelData levelData, string filePath)
        {
            var json = SaveLevelData(levelData);
            File.WriteAllText(filePath, json);
        }

        public static LevelData LoadLevelDataFromFile(string filePath)
        {
            if (File.Exists(filePath)) return LoadLevelData(File.ReadAllText(filePath));
            Debug.LogError($"[Zyntra] LevelData file not found : {filePath}");
            return new LevelData();
        }

        public static void SaveMetadataToFile<T>(T metadata, string filePath) where T : LevelMetadata
        {
            var json = SaveMetadata(metadata);
            File.WriteAllText(filePath, json);
        }

        public static LevelMetadata LoadMetadataFromFile(string filePath)
        {
            if (File.Exists(filePath)) return LoadMetadata(File.ReadAllText(filePath));
            Debug.LogError($"[Zyntra] Metadata file not found : {filePath}");
            return new LevelMetadata();
        }
    }
}