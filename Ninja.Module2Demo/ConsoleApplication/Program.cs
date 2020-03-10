using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            /* code to stop EF from going through its database initialization process when it's working with the NinjaContext
             * This is something to be aware of when you deply apps into production )*/
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());

            //InsertNinja();
            //InsertMultipleNinjas();
            //InsertNinjaWithEquipment();
            //SimpleNinjaQuery();
            //SimpleNinjaGraphQuery();
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //DeleteNinja();
            Console.ReadKey();
        }

        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "NinkaSan",
                ServedInOniwaban = true,
                DateOfBirth = new DateTime(1992, 4, 20),
                ClanId = 1
            };

            //using entity framework to insert the object
            using(var context=new NinjaContext())
            {
                //context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }

        }//InsertNinja()

        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Leonardo",
                ServedInOniwaban = true,
                DateOfBirth = new DateTime(1982, 8, 2),
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "Raphael",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1985, 4, 23),
                ClanId = 1
            };
            var ninja3 = new Ninja
            {
                Name = "Maddie",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1990, 5, 4),
                ClanId = 1
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja> { ninja1, ninja2, ninja3});
                context.SaveChanges();
            }
        }//InsertMultipleNinjas()

    }
}

