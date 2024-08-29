using System;
using System.ComponentModel;
using System.Data;
using Microsoft.Maui.Controls;

namespace WeatherAPP
{
    public class MainPageVievModel : INotifyPropertyChanged
    {
        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        public MainPageVievModel()
        {
            UpdateVied();
        }

        private void UpdateVied()
        {
           var hour =DateTime.Now.Hour;
           if (hour >=6 && hour <18)
           {
                Color = Colors.LightBlue;
           }
           else
           {
                Color = Colors.DarkBlue;
           }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}