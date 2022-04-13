namespace Models
{
    public class Airport
    {
        public readonly static string INSERT = "INSERT INTO Airport (City, Country, Code, Continent) VALUES (@City, @Country, @Code, @Continent)";
        public readonly static string SELECTALL = "SELECT Id, City, Country, Code, Continent FROM Airport";
        public readonly static string SELECTBYCODE = "SELECT Id, City, Country, Code, Continent FROM Airport where Code = ";
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Continent { get; set; }

        public Airport()
        {
        }

        public Airport(string city, string country, string code, string continent)
        {
            City = city;
            Country = country;
            Code = code;
            Continent = continent;
        }

        public override string ToString()
        {
            return "Id: " + Id +
                   "\nCity: " + City +
                   "\nCountry: " + Country +
                   "\nContinent: " + Continent;
        }
    }
}
