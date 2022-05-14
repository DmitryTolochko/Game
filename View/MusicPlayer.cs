﻿using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace Game
{
    public enum MusicType
    {
        MainMenu,
        Pause,
        Game,
        Store,
        GameOver,
        Park
    }

    public enum SoundType
    {
        Run,
        Crystal,
        Jump,
        StoreBuy,
        StoreChoice
    }

    public static class MusicPlayer
    {
        private readonly static WaveOutEvent player = new WaveOutEvent();
        private readonly static WaveOutEvent FXplayer = new WaveOutEvent();
        private readonly static WaveOutEvent ParkPlayer = new WaveOutEvent();

        private readonly static AudioFileReader MainMenuMusic = new AudioFileReader(@"Sounds\MainMenu.wav");
        private readonly static AudioFileReader GameMusic = new AudioFileReader(@"Sounds\Pulse.wav");
        private readonly static AudioFileReader CrystalSound = new AudioFileReader(@"Sounds\Crystal.mp3");
        private readonly static AudioFileReader GameOverMusic = new AudioFileReader(@"Sounds\Game_Over.mp3");
        private readonly static AudioFileReader ParkSound = new AudioFileReader(@"Sounds\Park.mp3");
        private readonly static AudioFileReader JumpSound = new AudioFileReader(@"Sounds\Jump.mp3");
        private readonly static AudioFileReader StoreBuySound = new AudioFileReader(@"Sounds\Buy.mp3");
        private readonly static AudioFileReader StoreChoiceSound = new AudioFileReader(@"Sounds\Choice.mp3");
        private readonly static List<AudioFileReader> Run = new List<AudioFileReader>
        { 
            new AudioFileReader(@"Sounds\Run_1.mp3"), 
            new AudioFileReader(@"Sounds\Run_2.mp3"), 
            new AudioFileReader(@"Sounds\Run_3.mp3")
        };
        private readonly static Random random = new Random();

        public static void Play(MusicType musicType, bool isFirstFrame)
        {
            switch (musicType)
            {
                case MusicType.MainMenu:
                    {
                        if (MainMenuMusic.Position == MainMenuMusic.Length || isFirstFrame)
                        {
                            ParkPlayer.Dispose();
                            MainMenuMusic.Position = 1;
                            player.Dispose();
                            player.Init(MainMenuMusic);
                            player.Play();
                        }
                        break;
                    }
                case MusicType.Game:
                    {
                        if (GameMusic.Position == GameMusic.Length || isFirstFrame)
                        {
                            GameMusic.Position = 1;
                            GameMusic.Volume = 0.7f;
                            player.Dispose();
                            player.Init(GameMusic);
                            player.Play();
                        }
                        break;
                    }
                case MusicType.GameOver:
                    {
                        GameOverMusic.Position = 0;
                        player.Dispose();
                        player.Init(GameOverMusic);
                        player.Play();
                        break;
                    }
                case MusicType.Park:
                    {
                        if (ParkSound.Position == ParkSound.Length || isFirstFrame)
                        {
                            ParkSound.Position = 1;
                            ParkSound.Volume = 2.1f;
                            ParkPlayer.Dispose();
                            ParkPlayer.Init(ParkSound);
                            ParkPlayer.Play();
                        }
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
                        var sound = Run[random.Next() % 3];
                        sound.Position = 0;
                        var player = new WaveOutEvent();
                        player.Dispose();
                        player.Init(sound);
                        player.Play();
                        break;
                    }
                case SoundType.Crystal:
                    {
                        CrystalSound.Position = 0;
                        FXplayer.Dispose();
                        FXplayer.Init(CrystalSound);
                        FXplayer.Play();
                        break;
                    }
                case SoundType.Jump:
                    {
                        JumpSound.Position = 0;
                        JumpSound.Volume = 2f;
                        FXplayer.Dispose();
                        FXplayer.Init(JumpSound);
                        FXplayer.Play();
                        break;
                    }
                case SoundType.StoreBuy:
                    {
                        StoreBuySound.Position = 0;
                        StoreBuySound.Volume = 2f;
                        FXplayer.Dispose();
                        FXplayer.Init(StoreBuySound);
                        FXplayer.Play();
                        player.Dispose();
                        break;
                    }
                case SoundType.StoreChoice:
                    {
                        StoreChoiceSound.Position = 0;
                        StoreChoiceSound.Volume = 1.6f;
                        FXplayer.Dispose();
                        FXplayer.Init(StoreChoiceSound);
                        FXplayer.Play();
                        break;
                    }
            }
        }
    }
}