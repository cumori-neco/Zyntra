using System;

namespace Zyntra.Data
{
    [Serializable]
    public class LevelDataSet
    {
        public string Name;
        public string Author;
        public string GameName;

        public string[] LevelFiles;
    }
}