using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Flappy
{
    /// <summary>
    /// Мозаичная поверхность
    /// </summary>
    public class TiledGround
    {
        private const int IMG_SIZE = 128;

        private Image[] images;
        private double offset;

        /// <summary>
        /// Обновление состояния
        /// </summary>
        public void Update()
        {
            offset = (offset - MainWindow.MOVE_SPEED) % IMG_SIZE;
            double x = offset;
            foreach(Image tile in images)
            {
                tile.SetValue(Canvas.LeftProperty, Math.Round(x));
                x += IMG_SIZE;
            }
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="parent">Объект-контейнер</param>
        /// <param name="width">Ширина поля</param>
        public TiledGround(Canvas parent, int width)
        {
            offset = 0;
            int nImages = width / IMG_SIZE + 2;
            images = new Image[nImages];
            double x = 0;
            Uri tileUri = new Uri("/Flappy;component/grass.png", UriKind.Relative);
            for(int i = 0; i < nImages; i++)
            {
                Image grassTile = new Image();
                grassTile.Source = new BitmapImage(tileUri);
                images[i] = grassTile;
                grassTile.SetValue(Canvas.TopProperty, (double)MainWindow.GROUND_Y);
                grassTile.SetValue(Canvas.LeftProperty, x);
                parent.Children.Add(grassTile);
                x += MainWindow.MOVE_SPEED;
            }
        }
    }
}
