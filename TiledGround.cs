using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Flappy
{
    /// <summary>
    /// Мозаичная поверхность для земли
    /// </summary>
    public class TiledGround
    {
        // Размер каждого элемента
        private const int TileSize = 128;

        // Ссылка на ресурс изображения поверхности
        private const string ResourceUri = "/Flappy;component/grass.png";

        private Image[] images;
        private double offset;

        // Игровое поле
        private IGameField field;

        /// <summary>
        /// Обновление состояния
        /// </summary>
        public void Update()
        {
            offset = (offset - field.MoveSpeed) % TileSize;
            double x = offset;
            foreach(Image tile in images)
            {
                tile.SetValue(Canvas.LeftProperty, Math.Round(x));
                x += TileSize;
            }
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="parent">Объект-контейнер</param>
        /// <param name="field">Игровое поле</param>
        public TiledGround(Canvas parent, IGameField field)
        {
            this.field = field;

            offset = 0;
            int nImages = (int)field.FieldWidth / TileSize + 1;
            images = new Image[nImages];

            double x = 0;
            Uri tileUri = new Uri(ResourceUri, UriKind.Relative);

            for(int i = 0; i < nImages; i++)
            {
                Image grassTile = new Image();
                grassTile.Source = new BitmapImage(tileUri);
                images[i] = grassTile;
                grassTile.SetValue(Canvas.TopProperty, field.GroundPosition);
                grassTile.SetValue(Canvas.LeftProperty, x);
                parent.Children.Add(grassTile);
                x += field.MoveSpeed;
            }
        }
    }
}
