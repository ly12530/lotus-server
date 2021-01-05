namespace Core.Domain
{
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        
        public double[] Geometry { get; set; }
    }
}