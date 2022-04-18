namespace GenericApi.Models
{
    public class Customer
    {
        public int Id { get; set; } 
        public string? First_name { get; set; }

        public string? Last_name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? Description { get; set; }

        public static List<Customer> Customers = new()
        {
            new Customer() {
                //Id = 1,
                First_name = "Customer_1",
                Last_name ="Customer_1",
                Email = "customer1@mail.com",
                Phone = "00000000000",
                Address =  "Customer_1 Address",
                Description =  "Customer_1 description"
            },
            new Customer() {
                //Id = 2,
                First_name = "Customer_2",
                Last_name ="Customer_1",
                Email = "customer2@mail.com",
                Phone = "00000000000",
                Address =  "Customer_2 Address",
                Description =  "Customer_2 description"
            },
            new Customer() {
                //Id = 3,
                First_name = "Customer_3",
                Last_name ="Customer_3",
                Email = "customer3@mail.com",
                Phone = "00000000000",
                Address =  "Customer_3 Address",
                Description =  "Customer_3 description"
            }
        };
    }
}
