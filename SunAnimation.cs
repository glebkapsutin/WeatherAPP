using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace WeatherAPP.Animations
{
    public class SunAnimation :GraphicsView
    {
        public SunAnimation()
        {
            Drawable = new SunDrawable();
            Device.StartTimer(TimeSpan.FromMilliseconds(16),() =>
            {
                Invalidate();
                return true;
            });
        }
        class SunDrawable() : IDrawable
        {
            private float _angle = 0;

            public void Draw (ICanvas canvas,RectF dirtyRect)
            {
                canvas.FillColor = Colors.LightBlue;
                canvas.FillRectangle(dirtyRect);

                var PosX = dirtyRect.Center.X;
                var PosY = dirtyRect.Center.Y;

                var sunRadius = 60;
                canvas.FillColor = Colors.Yellow;

                canvas.FillCircle(PosX,PosY, sunRadius);

                for (int i = 0; i< 18; i++)
                {
                    double angle = _angle +(Math.PI / 6 * i);

                    float startX = PosX +(float)(sunRadius * Math.Cos(angle));
                    float startY = PosY +(float)(sunRadius * Math.Sin(angle));

                    float endX = PosX + (float)((sunRadius + 40) * Math.Cos(angle));
                    float endY = PosY + (float)((sunRadius + 40) * Math.Sin(angle));

                    canvas.StrokeColor = Colors.Orange;

                    canvas.StrokeSize = 5;

                    canvas.DrawLine(startX,startY,endX,endY);

                }
                _angle += 0.01f;
            }

        }
    }
   
}