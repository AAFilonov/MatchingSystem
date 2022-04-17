// See https://aka.ms/new-console-template for more information

using MatchingSystem.Data;
using MatchingSystem.Data.Feature.User;
using MatchingSystem.Data.Model;


DiplomaMatchingContext context = new DiplomaMatchingContext("data source=localhost\\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
UserRepository userRepository =
    new UserRepository(context);

//Console.WriteLine("user " + userRepository.findAll()?? "NULL");
IEnumerable<User> users = userRepository.findAll();

users.ToList().ForEach(entity =>Console.WriteLine("user "+ entity) );
