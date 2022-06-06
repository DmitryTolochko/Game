using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Game
{
    public enum MusicType
    {
        MainMenu,
        Pause,
        Game,
        Store,
        GameOver
    }

    public enum SoundType
    {
        Run,
        Crystal,
        StoreBuy,
        StoreChoice,
        Button,
        Banana
    }

    public static class MusicPlayer
    {
        public static Bitmap VolumeImage = Resource1.Volume_100;

        private readonly static MixingSampleProvider mixer = new
            MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        { ReadFully = true };

        private readonly static WaveOutEvent player = new WaveOutEvent { Volume = 1 };

        private readonly static LoopStream MainMenuMusic = new LoopStream(new WaveFileReader(@"Sounds\MainMenu.wav"));
        private readonly static LoopStream GameMusic = new LoopStream(new WaveFileReader(@"Sounds\Pulse.wav"));
        private readonly static LoopStream ParkSound = new LoopStream(new WaveFileReader(@"Sounds\Park.wav"));

        private readonly static AudioFileReader GameOverMusic = new AudioFileReader(@"Sounds\Game_Over.wav");
        
        private readonly static CachedSound StoreBuySound = new CachedSound(@"Sounds\Buy.wav");
        private readonly static CachedSound StoreChoiceSound = new CachedSound(@"Sounds\Choice.wav");
        private readonly static CachedSound ButtonSound = new CachedSound(@"Sounds\button_hover.wav");
        private readonly static CachedSound BananaSound = new CachedSound(@"Sounds\Banana.wav");
        private readonly static CachedSound CrystalSound = new CachedSound(@"Sounds\Crystal.wav");
        
        private readonly static Random random = new Random();

        private readonly static List<CachedSound> RunSound = new List<CachedSound>
        {
            new CachedSound(@"Sounds\Run_1.wav"),
            new CachedSound(@"Sounds\Run_2.wav"),
            new CachedSound(@"Sounds\Run_3.wav"),
        };

        static MusicPlayer()
        {
            player.Init(mixer);
            player.Play();
        }

        public static void Play(MusicType musicType, bool isFirstFrame)
        {
            if (isFirstFrame)
                mixer.RemoveAllMixerInputs();
            switch (musicType)
            {
                case MusicType.MainMenu:
                    {
                        if (isFirstFrame)
                        {
                            MainMenuMusic.Position = 0;
                            mixer.AddMixerInput(MainMenuMusic);
                        }
                        break;
                    }
                case MusicType.Game:
                    {
                        if (isFirstFrame)
                        {
                            GameMusic.Position = 0;
                            GameMusic.Position = 0;
                            mixer.AddMixerInput(GameMusic);
                            mixer.AddMixerInput(ParkSound);
                        }
                        break;
                    }
                case MusicType.GameOver:
                    {
                        GameOverMusic.Position = 0;
                        mixer.RemoveAllMixerInputs();
                        mixer.AddMixerInput(new AutoDisposeFileReader(GameOverMusic));
                        break;
                    }
            }
        }

        public static void Play(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Run:
                    {
                        var index = random.Next() % 3;
                        mixer.AddMixerInput(new CachedSoundSampleProvider(RunSound[index]));
                        break;
                    }
                case SoundType.Crystal:
                    {
                        mixer.AddMixerInput(new CachedSoundSampleProvider(CrystalSound));
                        break;
                    }
                case SoundType.StoreBuy:
                    {
                        mixer.AddMixerInput(new CachedSoundSampleProvider(StoreBuySound));
                        break;
                    }
                case SoundType.StoreChoice:
                    {
                        mixer.AddMixerInput(new CachedSoundSampleProvider(StoreChoiceSound));
                        break;
                    }
                case SoundType.Button:
                    {
                        mixer.AddMixerInput(new CachedSoundSampleProvider(ButtonSound));
                        break;
                    }
                case SoundType.Banana:
                    {
                        mixer.AddMixerInput(new CachedSoundSampleProvider(BananaSound));
                        break;
                    }
            }
        }

        public static Bitmap ChangeVolume()
        {
            if (player.Volume == 0)
            {
                player.Volume = 1;
                VolumeImage = Resource1.Volume_100;
                return VolumeImage;
            }
            else if (player.Volume == 1)
            {
                player.Volume = 0.5f;
            }
            else
            {
                player.Volume = 0;
            }
            VolumeImage = player.Volume == 0 ? Resource1.Volume_0 : Resource1.Volume_50;
            return VolumeImage;
        }
    }

    public class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string audioFileName)
        {
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                WaveFormat = audioFileReader.WaveFormat;
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }
    }

    public class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
    }

    public class AutoDisposeFileReader : ISampleProvider
    {
        private readonly AudioFileReader reader;
        public bool isDisposed;
        public AutoDisposeFileReader(AudioFileReader reader)
        {
            this.reader = reader;
            this.WaveFormat = reader.WaveFormat;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (isDisposed)
                return 0;
            int read = reader.Read(buffer, offset, count);
            if (read == 0)
            {
                isDisposed = true;
            }
            return read;
        }

        public WaveFormat WaveFormat { get; private set; }
    }

    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        public bool EnableLooping { get; set; }

        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        public override long Length
        {
            get { return sourceStream.Length; }
        }

        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        break;
                    }
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
