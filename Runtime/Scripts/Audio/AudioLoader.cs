using System.IO;
using System.Threading.Tasks; //??? never used that before
using UnityEngine;
using UnityEngine.Networking;

namespace Zyntra.Audio
{
    public static class AudioLoader
    {
        public static async Task<AudioClip> LoadAudioFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[Zyntra] Audio file not found : {filePath}");
                return null;
            }
            
            var audioType = GetAudioType(filePath);
            if (audioType == AudioType.UNKNOWN)
            {
                Debug.LogError($"[Zyntra] Unsupported audio extension : {filePath}");
                return null;
            }
            
            var formattedPath = "file://" + Path.GetFullPath(filePath);

            using (var www = UnityWebRequestMultimedia.GetAudioClip(formattedPath, audioType))
            {
                var operation = www.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[Zyntra] Audio load failed :  {www.error}");
                    return null;
                }
                
                var audio = DownloadHandlerAudioClip.GetContent(www);
                audio.name = Path.GetFileNameWithoutExtension(filePath);
                return audio;
            }
        }

        private static AudioType GetAudioType(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".ogg" => AudioType.OGGVORBIS,
                ".wav" => AudioType.WAV,
                ".mp3" => AudioType.MPEG,
                _ => AudioType.UNKNOWN
            };
        }
    }
}