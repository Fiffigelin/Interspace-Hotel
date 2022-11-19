using MySqlConnector;
using Dapper;
// class DBConnector
// {
//     // public static MySqlConnection Connect()
//     {
//         MySqlConnection connection = new MySqlConnection("Server = localhost;Database = interspace_hotel;Uid=root");
//         return connection;
//     }

    // public void SearchForCustomer()
    // {
    //     using (var connection = new MySqlConnection("Server = localhost;Database = interspace_hotel;Uid=root"))
    //     {
    //         var users = connection.Query<Customer>("SELECT customer.name AS Name,customer.phonenumber AS Phonenumber,customer.id AS ID FROM customer;").ToList();

    //         foreach (Customer u in users)
    //         {
    //             Console.WriteLine(u.ID + " " + u.Name + " " + u.Phonenumber);
    //         }
    //     }
    // }
// }