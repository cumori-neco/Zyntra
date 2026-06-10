using Zyntra.Data;

namespace Zyntra.Audio
{
    public static class AudioTool
    {
        public static double ToSeconds(int min, int sec, int ms)
        {
            return (min * 60) + sec + (ms / 1000.0);
        }

        public static double GetLevelLength(LevelData level)
        {
            return level.Objects[level.Objects.Count - 1].time;
        }

        public static double GetLevelDensity(LevelData level)
        {
            return level.Objects.Count / GetLevelLength(level);
        }

        /// <summary>
        /// A MUST BE BIGGER THAN B
        /// </summary>
        public static double GetTimeDistance(double a, double b)
        {
            return a - b;
        } 
    }
}