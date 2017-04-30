using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Flappy
{
    // Препятствие
    public class Obstacle
    {
        private const int WIDTH = 75;
        private const int MARGIN = 100;
        private const int GAP_SIZE = 200;

        private Canvas Parent;
        private Rectangle[] rects;
        private bool scoreEarned;

        /// <summary>
        /// Событие о том, что игрок успешно пролетел препятствие
        /// </summary>
        public event EventHandler EarnScore;

        /// <summary>
        /// Находится ли объект за пределами поля
        /// </summary>
        public bool OutOfBounds { get; private set; }

        /// <summary>
        /// Проверка столкновения объекта с препятствием
        /// </summary>
        /// <param name="target">Элемент, для которого будет произведена проверка</param>
        public bool HitTest(Player target)
        {
            var x0 = (double)rects[0].GetValue(Canvas.LeftProperty);
            var x1 = x0 + WIDTH;
            // Проверка на возможность получения очка
            if(!scoreEarned && target.Location.X + target.Size.Width > x1)
            {
                if(this.EarnScore != null)
                {
                    EarnScore(this, EventArgs.Empty);
                    scoreEarned = true;
                }
            }

            // Проверка на столкновение
            foreach(var testRect in rects)
            {
                var y0 = (double)testRect.GetValue(Canvas.TopProperty);
                var y1 = y0 + testRect.ActualHeight;
                bool xHit = ((target.Location.X > x0) &&
                            (target.Location.X < x0 + WIDTH)) ||
                            ((target.Location.X + target.Size.Width > x0) &&
                            (target.Location.X + target.Size.Width < x1));
                bool yHit = ((target.Location.Y > y0) &&
                            (target.Location.Y < y1)) ||
                            ((target.Location.Y + target.Size.Height > y0) &&
                            (target.Location.Y + target.Size.Height < y1));
                if (xHit && yHit)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Обновить состояние препятствия
        /// </summary>
        public void Update()
        {
            foreach(Rectangle rect in rects)
            {
                double prevX = (double)rect.GetValue(Canvas.LeftProperty);
                double newX = prevX - MainWindow.MOVE_SPEED;
                rect.SetValue(Canvas.LeftProperty, newX);
                
                if(newX < -rect.Width)
                {
                    OutOfBounds = true;
                }
            }
        }

        /// <summary>
        /// Реинициализировать состояние и поместить элемент в контейнер
        /// <param name="container">Элемент-контейнер</param>
        /// </summary>
        public void Attach(Canvas container)
        {
            this.scoreEarned = false;
            this.Parent = container;
            this.OutOfBounds = false;

            Random rnd = new Random();
            int gapStart = rnd.Next(MARGIN, MainWindow.GROUND_Y - MARGIN * 2);
            rects[0].Height = gapStart;
            rects[1].SetValue(Canvas.TopProperty, (double)gapStart + GAP_SIZE);
            rects[1].Height = MainWindow.GROUND_Y - GAP_SIZE - gapStart;

            foreach(Rectangle rect in rects)
            {
                rect.SetValue(Canvas.LeftProperty, Parent.ActualWidth);
                Parent.Children.Add(rect);
            }
        }

        /// <summary>
        /// Удалить препятствие
        /// </summary>
        public void Detach()
        {
            foreach(Rectangle rect in rects)
            {
                Parent.Children.Remove(rect);
            }
        }

        // Установить фон объекта препятствия
        private void setPipeFill()
        {
            GradientStopCollection stops = new GradientStopCollection();
            Color colBorder = Color.FromArgb(255, 200, 255, 200);
            stops.Add(new GradientStop() { Color = colBorder, Offset = 0.0 });
            stops.Add(new GradientStop() { Color = Colors.Green, Offset = 0.5 });
            stops.Add(new GradientStop() { Color = colBorder, Offset = 1.0 });
            GradientBrush gb = new LinearGradientBrush(stops, 0);

            foreach(Rectangle rect in rects)
            {
                rect.Fill = gb;
            }
        }

        // Инициализация объектов, из которых состоит препятствие
        private void initialize()
        {
            Brush fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
            Rectangle topRect = new Rectangle { Width = WIDTH, Fill = fill };
            topRect.Margin = new Thickness { Top = 0 };
            rects[0] = topRect;
            Rectangle bottomRect = new Rectangle { Width = WIDTH, Fill = fill };
            bottomRect.Margin = new Thickness { Bottom = 0 };
            rects[1] = bottomRect;
            setPipeFill();
        }

        /// <summary>
        /// Создание объекта препятствия
        /// </summary>
        public Obstacle()
        {
            this.rects = new Rectangle[2];
            initialize();
        }
    }
}
