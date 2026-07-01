using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Zyntra.Data
{
    public class ZyntraDatabase
    {
        public static ZyntraDatabase Instance { get; private set; }

        public string DatabaseRootPath { get; private set; }

        public List<LevelDataProfile> LoadedProfiles { get; private set; } = new List<LevelDataProfile>();

        public static void Initialize(string basePath)
        {
            if (Instance != null)
            {
                Debug.LogWarning("[Zyntra/Data] ZyntraDatabase has already been initialized!");
            }
            
            Instance = new ZyntraDatabase(basePath);
            Instance.ScanAndLoadProfiles();
        }
        
        private ZyntraDatabase(string path)
        {
            DatabaseRootPath = Path.Combine(path, "ZyntraDatabase");

            if (!Directory.Exists(DatabaseRootPath))
            {
                Directory.CreateDirectory(DatabaseRootPath);
            }
        }

        public void ScanAndLoadProfiles()
        {
            LoadedProfiles.Clear();

            if (!Directory.Exists(DatabaseRootPath)) return;

            string[] levelFolders = Directory.GetDirectories(DatabaseRootPath);

            foreach (string folderPath in levelFolders)
            {
                string setJsonPath = Path.Combine(folderPath, "level.json");

                if (!File.Exists(setJsonPath)) continue;

                try
                {
                    string rawJson = File.ReadAllText(setJsonPath);

                    LevelDataSet partialLoader = JsonUtility.FromJson<LevelDataSet>(rawJson);

                    if (partialLoader != null)
                    {
                        LevelDataProfile profile = new LevelDataProfile(folderPath, partialLoader);
                        LoadedProfiles.Add(profile);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Zyntra/Data] Failed to read metadata at {setJsonPath}: {e.Message}");
                }
            }
        }
    }
}