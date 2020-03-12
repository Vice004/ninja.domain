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
            //DeleteNinjaWithKeyValue();
            //DeleteNinjaViaStoredProcedure();
            //SimpleNinjaGraphQuery();
            ProjectionQuery();
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
                var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas").ToList();
                foreach(var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }//foreach
            }//using
        }//RetrieveDataWithStoredProc()

        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
                //context.Ninjas.Remove(ninja);
                //context.SaveChanges();
            }
            using (var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                /* making the context aware of the ninja object(prevents the context from being seen as a new context by the EF) */
                //context.Ninjas.Attach(ninja);
                //context.Ninjas.Remove(ninja);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }//using
        }//Deleting a ninja from the database

        //Not that ideal sice the function makes #2 round trips to the DB
        private static void DeleteNinjaWithKeyValue()
        {
            
            var keyval = 1;
            using (var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);//DB round trip #1
                context.Ninjas.Remove(ninja);
                context.SaveChanges();//DB round trip #2
            }//
        }//DeleteNinjaWithKeyValue()

        //Another option for deleting using a stored procedure
        private static void DeleteNinjaViaStoredProcedure()
        {
            var keyval = 4;
            using (var context =new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand(
                    "exec DeleteNinjaViaId {0}", keyval);
            }
        }//DeleteNinjaViaStoredProcedure()

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //Creating a Ninja object
                var ninja = new Ninja
                {
                    Name = "Kacy Catanzaro",
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1990,1,14),
                    ClanId = 1
                };
                //Creating 2 Equipment objects
                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool
                };
                var spunk = new NinjaEquipment
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };
                //adding the Ninja to the context
                context.Ninjas.Add(ninja);
                //adding the Equipment objects to the Ninja object
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();
            }//using
        }//InsertNinjaWithEquipment()

        private static void SimpleNinjaGraphQuery()
        {
            using (var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                /*Eager loading - To bring all the data in advance, back from the DB in one Call
                 *  this employs entity framework's 'DbSet.Include' method*/

                /*Explicit loading allows you to retrieve specific data on the fly*/

                //Eager loading example
                //var ninja = context.Ninjas.Include(n => n.EquipmentOwned)
                    //.FirstOrDefault(n => n.Name.StartsWith("Kacy"));
                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));
                Console.WriteLine("Ninja Retrieved:" + ninja.Name);
                /*Explicit loading*/
                //context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
                /*Lazy loading - causes extra trips to the DB when loading a data-bound column*/
                Console.WriteLine
                    ("Ninja Equipment Count: {0}", ninja.EquipmentOwned.Count());
            }//using
        }//SimpleNinjaGraphQuery()

        //LINQ Projection queries return an "<Anonymous Type>"
        private static void ProjectionQuery()
        {
            using(var context=new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Select(n => new { n.Name, n.DateOfBirth, n.EquipmentOwned })
                    .ToList();
            }//using
        }//ProjectionQuery
    }
}

