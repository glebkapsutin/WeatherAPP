using Microsoft.Maui.Controls;
using System;
namespace WeatherAPP
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new WeatherUImodel();
            AnimButton();
           
            
        }
        private void AnimButton()
        {
            var scaleButton1 =new  Animation( v=> GetWeatherButton.Scale= v ,0.5,1.0);
            scaleButton1.Commit(this, "ScaleAnimation1", length: 1000, easing: Easing.Linear);
            var scaleButton2 =new  Animation( v=> GetForecastButton.Scale= v ,0.5,1.0);
            scaleButton2.Commit(this, "ScaleAnimation2", length: 1000, easing: Easing.Linear);
        }
        
       

    }

}
