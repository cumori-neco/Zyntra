using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zyntra.Objects.Notes;
using Zyntra.Objects;

namespace Zyntra.Audio
{
    /// <summary>
    /// A component used to play music and handle events.
    /// </summary>
    public class Conductor : MonoBehaviour
    {
        public AudioSource musicSource;

        [Header("Live Data")] public double bpm;
        public double songOffset;

        // Timing
        private double _dspSongStart;
        public double CurrentSongTime { get; private set; }
        public double CurrentBeat { get; private set; }
        public bool IsPlaying { get; private set; }
        public double secPerBeat;

        [Header("Timeline")] public List<TimelineObject> TimelineObjects = new List<TimelineObject>();
        private int _nextObjectIndex = 0;

        private double _beatOffset = 0;
        private double _timeAtLastBPMChange = 0;

        public void PlaySong(AudioClip clip, double startDelay = 2.0)
        {
            musicSource.clip = clip;

            _dspSongStart = AudioSettings.dspTime;
            musicSource.PlayScheduled(_dspSongStart);

            _nextObjectIndex = 0; // I don't trust myself
            IsPlaying = true;
        }

        void Update()
        {
            if (!IsPlaying) return;

            // Commented out due to latency issues
            // CurrentSongTime = AudioSettings.dspTime - _dspSongStart - songOffset;
            CurrentSongTime = musicSource.time - songOffset;

            while (_nextObjectIndex < TimelineObjects.Count && CurrentSongTime >= TimelineObjects[_nextObjectIndex].time)
            {
                TimelineObject currentObject = TimelineObjects[_nextObjectIndex];
                if (currentObject is Objects.Events.Event e)
                {
                    e.EventAction();
                }
                
                _nextObjectIndex++;
            }

            double timeSinceChange = CurrentSongTime - _timeAtLastBPMChange;
            secPerBeat = 60.0 / bpm; // Confuses me, why did I do that

            CurrentBeat = _beatOffset + (timeSinceChange / secPerBeat);
        }

        /// <summary>
        /// This was made because editing the BPM directly fucks everything up
        /// </summary>
        /// <param name="newBPM">Desired BPM</param>
        /// <param name="eventTimestamp">When the change happens</param>
        public void UpdateBPM(double newBPM, double eventTimestamp)
        {
            double timeSinceLastChange = eventTimestamp - _timeAtLastBPMChange;

            secPerBeat = 60.0 / bpm; // ^^^

            _beatOffset += timeSinceLastChange / secPerBeat;
            _timeAtLastBPMChange = eventTimestamp;
            bpm = newBPM;
        }
    }
}