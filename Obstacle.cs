using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Flappy
{
    /// <summary>
    /// Препятствие
    /// </summary>
    public class Obstacle
    {
        private const int Width = 75;
        private const int Margin = 100;
        private const int GapSize = 150;

        private Canvas Parent;
        private Rectangle[] rects;

        private bool scoreEarned;

        /// <summary>
        /// Поле, на которое помещается препятисвие
        /// </summary>
        public IGameField Field { get; set; }

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
            double x0 = (double)rects[0].GetValue(Canvas.LeftProperty);
            double x1 = x0 + Width;

            double tX0 = target.Location.X;
            double tY0 = target.Location.Y;
            double tX1 = tX0 + target.Size.Width;
            double tY1 = tY0 + target.Size.Height;

            // Проверка на возможность получения очка
            if(!scoreEarned && tX1 > x1)
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
                double y0 = (double)testRect.GetValue(Canvas.TopProperty);
                double y1 = y0 + testRect.ActualHeight;

                bool xHit = x0 <= tX1 && x1 >= tX0;
                bool yHit = y0 <= tY1 && y1 >= tY0;

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
                double newX = prevX - Field.MoveSpeed;
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
            int maxGapSize = (int)Field.GroundPosition - Margin * 2;
            int gapStart = rnd.Next(Margin, maxGapSize);

            rects[0].Height = gapStart;
            rects[1].SetValue(Canvas.TopProperty, (double)gapStart + GapSize);
            rects[1].Height = Field.GroundPosition - GapSize - gapStart;

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
        private void SetPipeFill()
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
        private void Initialize()
        {
            Brush fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
            Rectangle topRect = new Rectangle { Width = Width, Fill = fill };
            topRect.Margin = new Thickness { Top = 0 };
            rects[0] = topRect;
            Rectangle bottomRect = new Rectangle { Width = Width, Fill = fill };
            bottomRect.Margin = new Thickness { Bottom = 0 };
            rects[1] = bottomRect;
            SetPipeFill();
        }

        /// <summary>
        /// Создание объекта препятствия
        /// </summary>
        public Obstacle()
        {
            this.rects = new Rectangle[2];
            Initialize();
        }
    }
}
