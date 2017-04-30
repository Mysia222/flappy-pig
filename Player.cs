using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flappy
{
    /// <summary>
    /// Представлет игрока
    /// </summary>
    public class Player
    {
        private const int SIZE = 60;
        private const int MAX_VELOCITY = -25;
        private const int FRAMES_COUNT = 2;

        private Image sprite;
        private RotateTransform rotation;
        private BitmapImage[] bmps;

        private int height;
        private double velocity;
        private uint nFrame;

        /// <summary>
        /// Расположение спрайта игрока на поле
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point
                {
                    X = (double)sprite.GetValue(Canvas.LeftProperty),
                    Y = (double)sprite.GetValue(Canvas.TopProperty)
                };
            }
        }

        /// <summary>
        /// Размер спрайта игрока
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(sprite.ActualWidth, sprite.ActualHeight);
            }
        }

        /// <summary>
        /// Взмахнуть крылышками
        /// </summary>
        public void Flap()
        {
            velocity = -MAX_VELOCITY;
        }

        /// <summary>
        /// Обновить состояние игрока
        /// </summary>
        public void Update()
        {
            velocity = velocity > MAX_VELOCITY ? velocity - 3 : MAX_VELOCITY;
            double prevY = (double)sprite.GetValue(Canvas.TopProperty);
            double newY = prevY - velocity;
            if(newY < 0)
            {
                newY = 0;
            }
            if(newY > MainWindow.GROUND_Y - SIZE)
            {
                newY = MainWindow.GROUND_Y - SIZE;
            }

            sprite.SetValue(Canvas.TopProperty, newY);
            rotation.Angle = -velocity;

            sprite.Source = (nFrame % 10 > 5) ? bmps[0] : bmps[1];
            unchecked { nFrame++; }
        }

        /// <summary>
        /// Инициализация игрока
        /// <param name="param name="sprite">Графическое отображение</param>
        /// <param name="dimension">Размеры поля</param>
        /// </summary>
        public Player(FrameworkElement sprite, Size dimension)
        {
            this.sprite = sprite as Image;
            this.sprite.SetValue(Canvas.LeftProperty, (dimension.Width - SIZE) / 2);
            this.sprite.SetValue(Canvas.TopProperty, (dimension.Height - SIZE) / 2);
            this.rotation = sprite.FindName("rtRotation") as RotateTransform;

            this.height = (int)dimension.Height;
            this.velocity = 0;
            this.nFrame = 0;

            bmps = new BitmapImage[FRAMES_COUNT];
            for(int i = 1; i <= bmps.Length; i++)
            {
                Uri urlSprite = new Uri(String.Format("/Flappy;component/piggy{0}.png", i), 
                    UriKind.Relative);
                bmps[i - 1] = new BitmapImage(urlSprite);
            }
        }
    }
}
