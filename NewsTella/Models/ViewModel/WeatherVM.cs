namespace NewsTella.Models.ViewModel
{
    public class WeatherVM
    {
        public string City { get; set; }
        public string Country { get; set; }

        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public string Weather { get; set; }
        public double FeelsLike { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public int Pressure { get; set; }
        public string WeatherDescription { get; set; }
        public double WindSpeed { get; set; }
        public int WindDirection { get; set; }
        public int Cloudiness { get; set; }

    }
}
