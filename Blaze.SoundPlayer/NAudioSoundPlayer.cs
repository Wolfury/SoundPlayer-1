﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using Blaze.SoundPlayer.Waves;
using Blaze.SoundPlayer.WaveProviders;
using Blaze.SoundPlayer.Sounds;

namespace Blaze.SoundPlayer
{

    public class NAudioSoundPlayer : ISoundPlayer
    {

        public NAudioSoundPlayer()
        {
            mWaitForPlayBackStopped = new AutoResetEvent(false);
            mSampleFrequency = 1024 * 16;
        }

        public bool IsPlaying
        {
            get;
            protected set;
        }

        int mSampleFrequency;
        public int SampleFrequency
        {
            get
            {
                return mSampleFrequency;
            }
            set
            {
                if (!IsPlaying)
                    mSampleFrequency = value;
                else
                    throw new InvalidOperationException("Attempting to change sample frequency mid-play");
            }
        }

        readonly protected object mWaveLock = new Object();
        protected WaveOut mWaveOut;

        AutoResetEvent mWaitForPlayBackStopped;
 
        void mWaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            mWaitForPlayBackStopped.Set();
        }

        public static IWaveProviderExposer FactoryCreate(Sounds.SimpleSound sound)
        {
            return new SimpleSoundProvider(sound);
        }

        public static IWaveProviderExposer FactoryCreate(IList<Sounds.SimpleSound> sounds, IList<float> frq =null, IList<float> amps=null)
        {
            return new AdditiveSynthesisWaveProvider(sounds,frq,amps);
        }

        public void PlaySync(IWaveProviderExposer wave, float freq, int fixedDuration = -1)
        {
            wave.Frequency = freq;
            PlaySync(wave, fixedDuration);
        }

        public void PlaySync(IWaveProviderExposer wave, int fixedDuration = -1)
        {
            wave.SetWaveFormat(mSampleFrequency, 1);
            lock (mWaveLock)
            {
                mWaveOut = new WaveOut();
                if (fixedDuration == -1)
                    mWaveOut.PlaybackStopped += mWaveOut_PlaybackStopped;
                mWaveOut.Init((IWaveProvider)wave);
                mWaveOut.Play();
            }

            if (fixedDuration == -1)
                mWaitForPlayBackStopped.WaitOne();
            else
            {
                Thread.Sleep(fixedDuration);
                mWaveOut.Stop();
            }

            lock (mWaveOut)
            {
                if (fixedDuration == -1)
                    mWaveOut.PlaybackStopped -= mWaveOut_PlaybackStopped;
                mWaveOut.Dispose();
            }
        }

        public void PlaySync(Wave track, float freq = 440, int fixedDuration = -1)
        {
            var wave = new FixedDataWaveProvider(track);
            wave.Frequency = freq;
            PlaySync(wave, fixedDuration);
        }


        public void PlaySync(IList<Wave> tracks, IList<float> freq, int fixedDuration)
        {
            var wave = new CompositeFixedDataWaveProvider(tracks);
            wave.Frequencies = freq;
            PlaySync(wave, fixedDuration);
        }


        public void PlaySync(short[] data)
        {
            throw new NotImplementedException();
        }

        public void PlaySync(IList<short[]> datas)
        {
            throw new NotImplementedException();
        }

        public void PlaySync(Track track)
        {
            throw new NotImplementedException();
        }

        public void PlaySync(IList<Track> tracks)
        {
            throw new NotImplementedException();
        }

        public void PlaySync(WaveGenerator track, float freq, int fixedDuration)
        {
            var wave = new WaveGeneratorProvider(track);
            wave.Frequency = freq;
            PlaySync(wave,fixedDuration);
        }

        public void PlaySync(IList<WaveGenerator> tracks, IList<float> freq, int fixedDuration)
        {
            throw new NotImplementedException();
        }

        public void PlaySync(SimpleSound track, float freq, int fixedDuration)
        {
            PlaySync(FactoryCreate(track), freq,fixedDuration);
        }

        public void PlaySync(IList<SimpleSound> tracks, float freq, int fixedDuration)
        {
            PlaySync(FactoryCreate(tracks), freq, fixedDuration);
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }

        public bool Pause()
        {
            throw new NotImplementedException();
        }

        public bool SkipTo(int sample)
        {
            throw new NotImplementedException();
        }

        public bool SkipTo(TimeSpan moment)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(short[] data)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(IList<short[]> datas)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(Track track)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(IList<Track> tracks)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(WaveGenerator track, float freq, int fixedDuration)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(IList<WaveGenerator> tracks, IList<float> freq, int fixedDuration)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(Wave track, float freq, int fixedDuration)
        {
            throw new NotImplementedException();
        }

        public void PlayAsync(IList<Wave> tracks, IList<float> freq, int fixedDuration)
        {
            throw new NotImplementedException();
        }
    }
}
