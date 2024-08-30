using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System; // Импортируем пространство имен для базовых операций, таких как генерация случайных чисел.
using System.Collections.Generic;

namespace WeatherAPP.Animations
{
    public class NightAnimations : GraphicsView
    {
        private const float MoonRadius = 60;
        private const float MoonOffset = 30;
        private const int StarCount = 80;

        private readonly List<PointF> _stars;
        private static float _moonPhaseAngle = 0;

        public NightAnimations()
        {
            _stars = GenerateStars(); // Инициализируем _stars до использования
            Drawable = new NightDrawable(_stars); // Передаем инициализированные звезды в NightDrawable

            Device.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {
                Invalidate(); // Перерисовываем графику, чтобы обновить отображение анимации.
                _moonPhaseAngle += 0.01f; // Увеличиваем угол фазы луны, чтобы создать эффект движения.
                return true; // Возвращаем true, чтобы таймер продолжал работать.
            });
        }

        private List<PointF> GenerateStars()
        {
            var stars = new List<PointF>();
            Random rnd = new Random();
            for (int i = 0; i < StarCount; i++)
            {
                var x = (float)rnd.NextDouble();
                var y = (float)rnd.NextDouble();
                stars.Add(new PointF(x, y));
            }
            return stars;
        }

        class NightDrawable : IDrawable
        {   
            private readonly List<PointF> _stars;

            public NightDrawable(List<PointF> stars)
            {
                _stars = stars; // Сохраняем список звезд
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                // Заполняем фон темно-синим цветом
                canvas.FillColor = Colors.DarkBlue;
                canvas.FillRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

                var centerX = dirtyRect.Center.X;
                var centerY = dirtyRect.Center.Y;

                // Рисуем луну
                canvas.FillColor = Colors.LightGray;
                canvas.FillCircle(centerX+70, centerY - 100+70 + (float)Math.Sin(_moonPhaseAngle) * 50, MoonRadius);

                // Создаем маску для вырезания части луны
                canvas.FillColor = Colors.DarkBlue; // Фон для вырезания
                canvas.FillCircle(centerX+70 + MoonOffset, centerY+70 - 100 + (float)Math.Sin(_moonPhaseAngle) * 50, MoonRadius);

                // Рисуем звезды
                foreach (var star in _stars)
                {
                    // Определяем координаты звезды
                    var starX = star.X * dirtyRect.Width; // Координата X звезды
                    var starY = star.Y * dirtyRect.Height; // Координата Y звезды

                    // Устанавливаем цвет звезды на белый и рисуем её
                    canvas.FillColor = Colors.White;
                    canvas.FillCircle(starX, starY, 2); // Рисуем звезду как маленький круг
                }
            }
        }
    }
}
