using System;
using TraceEFChanges.DataAccess.Model;
using TraceEFChanges.DataAccess.Repositories;

namespace TraceEFChanges
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            test.RunTest();
            Console.ReadKey();
        }
    }

    public class Test
    {
        public async void RunTest()
        {
            PersonRepository repository = new PersonRepository();

            Console.WriteLine("Creating a person...");
            PersonEntity person = new PersonEntity()
            {
                Name = "Jorge",
                Address = "Address",
                Email = "jorge@prueba.com",
                PhoneNumber = "123456789"
            };
            person = await repository.Add(person);

            Console.WriteLine("Updating person...");
            person.Address = "Address changed";
            person.Email = "EmailChanged@test.com";
            await repository.Update(person);
        }
    }
}
