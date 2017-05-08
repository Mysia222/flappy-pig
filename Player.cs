using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flappy
{
    /// <summary>
    /// Игрок - летящий порось
    /// </summary>
    public class Player
    {
        private const int SpriteSize = 60;
        private const int FallVelocity = -25;
        private const int FramesCount = 2;

        private const string ResourceUri = "/Flappy;component/piggy{0}.png";

        private Image sprite;
        private RotateTransform rotation;
        private BitmapImage[] bmps;

        private IGameField field;

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
            velocity = -FallVelocity;
        }

        /// <summary>
        /// Обновить состояние игрока
        /// </summary>
        public void Update()
        {
            velocity = velocity > FallVelocity ? velocity - 3 : FallVelocity;
            double prevY = (double)sprite.GetValue(Canvas.TopProperty);
            double newY = prevY - velocity;
            if(newY < 0)
            {
                newY = 0;
            }
            if(newY > field.GroundPosition - SpriteSize)
            {
                newY = field.GroundPosition - SpriteSize;
            }

            sprite.SetValue(Canvas.TopProperty, newY);
            rotation.Angle = -velocity;

            sprite.Source = (nFrame % 10 > 5) ? bmps[0] : bmps[1];
            unchecked { nFrame++; }
        }

        /// <summary>
        /// Инициализация игрока
        /// <param name="sprite">Графическое отображение</param>
        /// <param name="field">Игровое поле</param>
        /// </summary>
        public Player(FrameworkElement sprite, IGameField field)
        {
            this.field = field;

            this.sprite = sprite as Image;
            double startX = (field.FieldWidth - SpriteSize) / 2;
            this.sprite.SetValue(Canvas.LeftProperty, startX);
            double startY = (field.GroundPosition - SpriteSize) / 2;
            this.sprite.SetValue(Canvas.TopProperty, startY);
            this.rotation = sprite.FindName("rtRotation") as RotateTransform;

            this.velocity = 0;
            this.nFrame = 0;

            bmps = new BitmapImage[FramesCount];
            for(int i = 1; i <= bmps.Length; i++)
            {
                Uri urlSprite = new Uri(String.Format(ResourceUri, i), 
                    UriKind.Relative);
                bmps[i - 1] = new BitmapImage(urlSprite);
            }
        }
    }
}
