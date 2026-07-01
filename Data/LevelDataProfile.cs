using System.IO;
using UnityEngine;

namespace Zyntra.Data
{
    public class LevelDataProfile
    {
        public string FolderPath { get; private set; }
        public string FolderName { get; private set; }
        
        public LevelDataSet Metadata {get; private set;}
        
        public LevelDataProfile(string folderPath, LevelDataSet metadata)
        {
            FolderPath = folderPath;
            FolderName = Path.GetFileName(folderPath);
            Metadata = metadata;
        }

        public LevelData LoadFullLevelData(string diffName)
        {
            string jsonPath = Path.Combine(FolderPath, diffName);
            if (!File.Exists(jsonPath)) return null;
            
            string rawJson = File.ReadAllText(jsonPath);
            return JsonUtility.FromJson<LevelData>(rawJson);
        }
    }
}