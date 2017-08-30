namespace Collectively.Common.Caching
{
    public class GeoResult<T>
    {
        public T Result { get; set; }
        public double? Distance { get; set;  }
        public double? Longitude { get; set;  }
        public double? Latitude { get; set;  }
    }
}