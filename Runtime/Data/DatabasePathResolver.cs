using System;
using System.IO;
using UnityEngine;

namespace Zyntra.Data
{
    public static class DatabasePathResolver
    {
        public static string FilePath()
        {
#if UNITY_IOS || UNITY_EDITOR
            return Path.Combine(Application.persistentDataPath, "ZyntraDatabase");

#elif UNITY_ANDROID
            return Path.Combine("/storage/emulated/0/Documents", "ZyntraDatabase");
#else
            return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ZyntraDatabase");
#endif
        }
    }
}