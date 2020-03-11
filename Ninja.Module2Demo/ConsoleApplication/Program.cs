using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            //SimpleNinjaQueries();
            //SimpleNinjaGraphQuery();
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //RetrieveDataWithFind();
            //RetrieveDataWithStoredProc();
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

        private static void SimpleNinjaQueries()
        {
            using (var context=new NinjaContext())
            {
                var ninjas = context.Ninjas.Where(n=>n.Name == "Raphael");
                //var query = context.Ninjas;
                //var someninjas = query.ToList();
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }//query
            }
        }//SimpleNinjaQueries()

        private static void QueryAndUpdateNinja()
        {
            using(var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }//using
        }//QueryAndUpdateNinja()

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            //This piece of code respresents the service or API retrieving a Ninja
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }//using
            ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);

            using(var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                //checking the state
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }//using
        }//QueryAndUpdateNinjaDisconnected()

        private static void RetrieveDataWithFind()
        {
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#1:" + ninja.Name);

                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#2:" + someNinja.Name);
                ninja = null;
            }//using
        }//RetrieveDataWithFind()

        private static void RetrieveDataWithStoredProc()
        {
            using (var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas");
                foreach(var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }//foreach
            }//using
        }//RetrieveDataWithStoredProc()
    }
}

