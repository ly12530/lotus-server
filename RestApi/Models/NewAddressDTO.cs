using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class NewAddressDTO
    {
        [Required] public string Street { get; set; }

        //[RegularExpression("^(\\d{​​​​1,6}​​​​)([a-z]{​​​​​​​​​​​0,2}​​​​​​​​​​​)(\\d{​​​​​​​​​​​0,1}​​​​​​​​​​​)$")]
        [Required] public string Number { get; set; }

        [Required] public string City { get; set; }

        //[RegularExpression("^(\\d{​​​​​4}​​​​​)(\\s{​​​​​0,1}​​​​​)([A-Z]{​​​​​2}​​​​​)$")]
        [Required] public string Postcode { get; set; }
    }
}