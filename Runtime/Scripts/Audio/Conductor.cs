using System.Collections.Generic;
using UnityEngine;
using Zyntra.Objects;
using Event = Zyntra.Objects.Events.Event;

namespace Zyntra.Audio
{
    /// <summary>
    /// A component used to play music and handle events.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Conductor : MonoBehaviour
    {
        [Header("Live Data")] [SerializeField] private double bpm = 120.0;
        public double songOffset = 0.0;

        // Timing
        private double _dspSongStart;
        public double CurrentSongTime { get; private set; }
        public double CurrentBeat { get; private set; }
        public bool IsPlaying { get; private set; }

        public double BPM => bpm;
        public double secPerBeat => bpm > 0 ? 60.0 / bpm : 0.0;

        [Header("Timeline")] public List<TimelineObject> TimelineObjects = new();
        private int _nextObjectIndex;

        private double _beatOffset;
        private double _timeAtLastBPMChange;

        private AudioSource _musicSource;

        private void Awake()
        {
            _musicSource = GetComponent<AudioSource>();
        }

        public void PlaySong(AudioClip clip, double startDelay = 2.0)
        {
            if (clip == null)
            {
                Debug.LogError("[Zyntra] Cannot play null AudioClip");
                return;
            }

            _musicSource.clip = clip;

            TimelineObjects.Sort((a, b) => a.time.CompareTo(b.time));

            _nextObjectIndex = 0;
            _beatOffset = 0.0;
            _timeAtLastBPMChange = 0.0;

            _dspSongStart = AudioSettings.dspTime + startDelay;
            _musicSource.PlayScheduled(_dspSongStart);

            IsPlaying = true;
        }

        public void StopSong()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
            }

            IsPlaying = false;
        }

        public void PauseSong()
        {
            if (!_musicSource.isPlaying) return;
            _musicSource.Pause();
            IsPlaying = false;
        }

        private void Update()
        {
            if (!IsPlaying) return;

            // Commented out due to latency issues
            // 2026/9/1 Was commented out due to latency
            // Because of how unstable it was, I decided to give dsp
            // another try, people trusts it, so I should as well.

            CurrentSongTime = AudioSettings.dspTime - _dspSongStart - songOffset;

            // CurrentSongTime = _musicSource.time - songOffset;

            while (_nextObjectIndex < TimelineObjects.Count &&
                   CurrentSongTime >= TimelineObjects[_nextObjectIndex].time)
            {
                var currentObject = TimelineObjects[_nextObjectIndex];
                if (currentObject is Event e)
                {
                    e.EventAction();
                }

                _nextObjectIndex++;
            }

            if (!(secPerBeat > 0)) return;
            var timeSinceChange = CurrentSongTime - _timeAtLastBPMChange;
            CurrentBeat = _beatOffset + timeSinceChange / secPerBeat;
        }

        /// <summary>
        /// This was made because editing the BPM directly fucks everything up
        /// </summary>
        /// <param name="newBPM">Desired BPM</param>
        /// <param name="eventTimestamp">When the change happens</param>
        public void UpdateBPM(double newBPM, double eventTimestamp)
        {
            if (secPerBeat > 0)
            {
                var timeSinceLastChange = eventTimestamp - _timeAtLastBPMChange;
                _beatOffset += timeSinceLastChange / secPerBeat;
            }

            _timeAtLastBPMChange = eventTimestamp;
            bpm = newBPM;
        }
    }
}