﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Flappy
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer mainTimer;
        private ObjectsPool<Obstacle> obstaclesPool;

        private int time;
        private int score;
        private Queue<Obstacle> obstacles;
        private Player flappy;
        private TiledGround ground;

        private bool isRestart;

        public const int GROUND_Y = 472;
        public const double MOVE_SPEED = 10.5;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            this.time = 0;
            this.score = -1;
            AdvanceScore(this, null);
            if (!isRestart)
            {
                mainTimer = new DispatcherTimer();
                mainTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
                mainTimer.Tick += mainTimer_Tick;

                obstaclesPool = new ObjectsPool<Obstacle>(obstacle =>
                    obstacle.EarnScore += AdvanceScore);
                obstacles = new Queue<Obstacle>();
                ground = new TiledGround(cnvGraph, (int)this.Width);
                flappy = new Player(imgPiggy, 
                    new Size(this.Width, this.Height));
            }
            else
            {
                foreach (var obstacle in obstacles)
                    obstacle.Detach();
                obstacles.Clear();
                obstaclesPool.ReleaseAll();
            }
            mainTimer.Start();
        }

        private void AdvanceScore(object sender, EventArgs e)
        {
            score++;
            tbScore.Text = score.ToString();
        }

        private void UpdateGame()
        {
            bool needDelete = false;

            flappy.Update();

            foreach(Obstacle item in obstacles)
            {
                // Если столкнулось с игроком - конец игры
                if(item.HitTest(flappy))
                {
                    EndGame();
                }
                // Обновление состояния
                item.Update();
                if(item.OutOfBounds)
                {
                    needDelete = true;
                }
            }
            if(needDelete)
            {
                Obstacle itemForDeletion = obstacles.Dequeue();
                itemForDeletion.Detach();
                obstaclesPool.ReleaseItem(itemForDeletion);
            }

            // Земля
            ground.Update();

            if(time % 35 == 0)
            {
                Obstacle newObstacle = obstaclesPool.GetItem();
                newObstacle.Attach(cnvGraph);
                obstacles.Enqueue(newObstacle);
            }

            unchecked { time++; }
        }

        private void EndGame()
        {
            mainTimer.Stop();
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            UpdateGame();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.isRestart = false;
            StartGame();
            this.Focus();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            EndGame();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            flappy.Flap();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                flappy.Flap();
            else if(e.Key == Key.F2)
            {
                this.isRestart = true;
                if (!mainTimer.IsEnabled)
                    StartGame();
            }
        }
    }
}
