using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Zyntra.Data
{
    public class ZyntraDatabase
    {
       public string DatabaseRootPath { get; private set; }

       public List<LevelDataProfile> LoadedProfiles { get; private set; } = new List<LevelDataProfile>();

       public ZyntraDatabase(string path)
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
               string jsonPath = Path.Combine(folderPath, "level.json");
               
               if (!File.Exists(jsonPath)) continue;

               try
               {
                   string rawJson = File.ReadAllText(jsonPath);

                   LevelDataSet partialLoader = JsonUtility.FromJson<LevelDataSet>(rawJson);

                   if (partialLoader != null)
                   {
                       LevelDataProfile profile = new LevelDataProfile(folderPath, partialLoader);
                   }
               }
               catch (Exception e)
               {
                   Debug.LogError($"[Zyntra/Data] Failed to read metadata at {jsonPath}: {e.Message}");
               }
           }
       }
    }
}