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
    public class LevelDataDTO
    {
        public List<SerializedObjectWrapper> objects = new();
    }

    public static class DataManager
    {
        public static LevelMetadata BuildLevel(
            string targetDirectory,
            string levelName,
            string romanizedName,
            string author,
            string songLocation,
            string gameName,
            List<LevelData> levelDataList)
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
                generatedFiles.Add(fullPath);
            }

            var metadata = new LevelMetadata
            {
                name = levelName,
                author = author,
                songLocation = songLocation,
                gameName = gameName,
                romanizedName = romanizedName,
                levelFiles = generatedFiles.ToArray()
            };

            var metadataPath = Path.Combine(targetDirectory, "metadata.json");
            SaveMetadataToFile(metadata, metadataPath);

            return metadata;
        }

        public static string SaveLevelData(LevelData levelData)
        {
            if (levelData == null) throw new ArgumentException(nameof(levelData));

            var dto = new LevelDataDTO();

            foreach (var obj in levelData.Objects)
            {
                if (obj == null) continue;

                var objectType = obj.GetType();

                if (!objectType.IsDefined(typeof(SerializableAttribute), false))
                {
                    throw new InvalidOperationException(
                        $"[Zyntra] Cannot serialize '{objectType.Name}'" +
                        "Classes inheriting from TimelineObject MUST be marked as Serializable!");
                }

                dto.objects.Add(new SerializedObjectWrapper
                {
                    typeName = objectType.AssemblyQualifiedName,
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

                var type = Type.GetType(wrapper.typeName);
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

        public static string SaveMetadata(LevelMetadata metadata)
        {
            return metadata == null
                ? throw new ArgumentException(nameof(metadata))
                : JsonUtility.ToJson(metadata, true);
        }

        public static LevelMetadata LoadMetadata(string jsonContent)
        {
            return string.IsNullOrEmpty(jsonContent)
                ? new LevelMetadata()
                : JsonUtility.FromJson<LevelMetadata>(jsonContent);
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

        public static void SaveMetadataToFile(LevelMetadata metadata, string filePath)
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