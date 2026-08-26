using System;

namespace Zyntra.Data
{
    [Serializable]
    public class LevelMetadata
    {
        public string name;
        public string romanizedName;
        
        public string author;
        public string gameName;

        public string[] levelFiles;
        public string songLocation;
    }
}