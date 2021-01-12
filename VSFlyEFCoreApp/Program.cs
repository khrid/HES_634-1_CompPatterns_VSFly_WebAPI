using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace VSFlyEFCoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var ctx = new WWWingsContext();

            var e = ctx.Database.EnsureCreated();

            if (e)
                Console.WriteLine("Database has been created !");
            else
                Console.WriteLine("exists already.");

            // on a la base de données
            PrintFlightsWithJoin();
            
            DeleteBookings();

            NewPassengers();

            NewPilots();

            printFlights();

            Console.WriteLine("---------------------------------");

            printFlightsWithArg();

            Console.WriteLine("---------------------------------");

            printFlightsWithLambda();

            NewFlights();

            NewBooking();

            // vérifier dans la base de données ou sélectionner et afficher tous les vols ici
            printFlights();

            PrintBookings();

            UpdateFlights();

            DeleteBookings();

            DeleteFlights();

            printFlights();

            PrintBookings();
        }

       

        private static void NewPilots()
        {
            using (var ctx = new WWWingsContext())
            {
                Pilot p1 = new Pilot { Surname = "Bono", GivenName = "Jean", Salary = 23000 };

                ctx.PilotSet.Add(p1);

                Pilot p2 = new Pilot { Surname = "Tilde", GivenName = "Pierre", Salary = 4800 };

                ctx.PilotSet.Add(p2);

                ctx.SaveChanges();
            }
        }

        private static void DeleteFlights()
        {
            using (var ctx = new WWWingsContext())
            {
                // supprimer
                // ...
                int key = (from flight in ctx.FlightSet select flight.FlightNo).Max();

                Console.WriteLine("supprime : {0}", key) ;

                Flight f = ctx.FlightSet.Find(key);

                ctx.FlightSet.Remove(f);

                ctx.SaveChanges();
            }
        }

        private static void UpdateFlights()
        {
            using (var ctx = new WWWingsContext())
            {
                // modifier
                // ...
                int key = (from flight in ctx.FlightSet select flight.FlightNo).Max();

                Flight f = ctx.FlightSet.Find(key);

                f.Seats += 1;

                ctx.SaveChanges();
            }
        }

        private static void NewFlights()
        {
            using (var ctx = new WWWingsContext())
            {
                // insérer
                Console.WriteLine("Insérer un nouvel avion : ");
                // un simple objet en C#
                Flight f = new Flight { Departure = "GVA", Destination = "LAX", Seats = 300 };

                f.Pilot = ctx.PilotSet.Find(1);

                // on passe par le context pour accéder à la base de données
                ctx.FlightSet.Add(f);

                // on persiste le changement dans la base de données
                ctx.SaveChanges();
            }
        }

        private static void printFlightsWithLambda()
        {
            using (var ctx = new WWWingsContext())
            {
                var q2 = ctx.FlightSet.Where(f => f.Seats > 100 && f.Departure.StartsWith("C"));

                foreach (Flight flight in q2)
                {
                    Console.WriteLine("{0} {1} {2}", flight.Date, flight.Destination, flight.Seats);
                }
            }
        }

        private static void printFlightsWithArg()
        {
            using (var ctx = new WWWingsContext())
            {
                // SQL -> Linq
                var q1 = from f in ctx.FlightSet
                         where f.Seats > 100
                         && f.Departure.StartsWith("C")
                         select f;

                foreach (Flight flight in q1)
                {
                    Console.WriteLine("{0} {1} {2}", flight.Date, flight.Destination, flight.Seats);
                }
            }
        }

        private static void PrintFlightsWithJoin() {
            using (var ctx = new WWWingsContext())
            {
                var q = from f in ctx.FlightSet.Include(x => x.Pilot)
                        select f;

                foreach (Flight flight in q)
                {
                    Console.WriteLine("{0} {1} {2} {3}",
                        flight.Date, flight.Destination, flight.Seats, flight.Pilot.Surname);
                }
            }
        }

        private static void printFlights()
        {
            // on crée le contexte localement
            using (var ctx = new WWWingsContext())
            {
                // sélectionner et afficher tous les vols
                foreach (Flight flight in ctx.FlightSet)
                {
                    // activer le lazy loading, c'est fait dans WWWingsContext.cs

                    Console.WriteLine("{0} {1} {2} {3}",
                        flight.Date, flight.Destination, flight.Seats, flight.Pilot.Surname);
                }
            } // le contexte est libéré
        }

        public static void NewPassengers()
        {
            using (var ctx = new WWWingsContext())
            {
                Passenger p1 = new Passenger() { GivenName = "Igor", Weight = 9 };
                ctx.Add(p1);

                Passenger p2 = new Passenger() { GivenName = "Toto", Weight = 10 };
                ctx.Add(p2);

                Passenger p3 = new Passenger() { GivenName = "Anne", Weight = 8 };
                ctx.Add(p3);

                Passenger p4 = new Passenger() { GivenName = "Sonia", Weight = 6 };
                ctx.Add(p4);


                ctx.SaveChanges();
            }
        }

        private static void PrintBookings()
        {
            Console.WriteLine("--------------------------------------------------------");

            using (var ctx = new WWWingsContext())
            {
                var q = from b in ctx.BookingSet.Include("Flight").Include("Passenger")
                        select b;

                foreach (Booking b in q)
                    Console.WriteLine("{0} {1} {2}", b.Flight.Date,
                                                    b.Flight.Destination,
                                                    b.Passenger.GivenName);
            }
        }

        public static void NewBooking()
        {
            using (var ctx = new WWWingsContext())
            {

                ctx.BookingSet.Add(new Booking { FlightNo = 1, PassengerID = 1 });
                
                ctx.SaveChanges();
            }
        }

        private static void DeleteBookings()
        {
            using (var ctx = new WWWingsContext())
            {
                ctx.BookingSet.RemoveRange(ctx.BookingSet);

                ctx.SaveChanges();
            }
        }

    }
}
