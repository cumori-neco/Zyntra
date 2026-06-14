using System.Collections.Generic;

namespace Zyntra.Data
{
    public class LevelDataSet
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string GameName { get; set; }
        
        public List<LevelData> Levels = new();
    }
}