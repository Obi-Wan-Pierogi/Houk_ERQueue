// Lee Houk
// IT113
// NOTES: This project was quite the challenge and involved a ton of research to get working to spec.
//        I originally used a separate class for the PriorityQueue as well as for the Patient in order to
//        keep things more organized. I found that by default the PriorityQueue did not have a stable
//        sort, so I added a timestamp in the enqueue process in addition to the priority to keep track of when patients  
//        were added to the queue. Then when comparing, I first looked at priority, then at timestamp to keep FIFO. 
// BEHAVIORS NOT IMPLIMENTED AND WHY: I got them all working.
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Houk_ERQueue
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            PriorityQueue<Patient, (int, DateTime)> ERQueue = new PriorityQueue<Patient, (int, DateTime)>();
            //Creates a dictionary that stores the patients, their priority, and a timestamp to be used for listing all patients
            Dictionary<Patient, (int, DateTime)> AllPatients = new Dictionary<Patient, (int, DateTime)>();

            //add patients to the queue from Patients - 1.csv, skipping the header line
            string[] lines = System.IO.File.ReadAllLines(@"Patients-1.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                string firstName = values[0];
                string lastName = values[1];
                var birthdate = DateOnly.ParseExact(values[2], "M/d/yyyy", CultureInfo.InvariantCulture);
                int priority = int.Parse(values[3]);
                Enqueue(new Patient(lastName, firstName, birthdate, priority));
            }

            Console.WriteLine("Welcome to the ER Priority Queue");
            bool exit = false;
            while (!exit)
            {
                // Variables for user input
                string choice;
                string lastName;
                string firstName;
                string input;
                int birthYear = 0;
                int birthMonth = 0;
                int birthDay = 0;
                int priority = 0;
                DateOnly birthdate;
                bool valid = false;

                // Generate a menu for the user to choose from a menu option using a hotkey in parantheses
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("(A)dd a patient");
                Console.WriteLine("(P)rocess current patient");
                Console.WriteLine("(L)ist all patients in queue");
                Console.WriteLine("(Q)uit");

                // Get the user's choice
                choice = Console.ReadLine().ToUpper();

                //Process user's choice using a case switch
                switch (choice)
                {
                    case "A":
                        // Get the user's input for the patient's information
                        Console.WriteLine("Please enter the patient's last name:");
                        lastName = Console.ReadLine();
                        Console.WriteLine("Please enter the patient's first name:");
                        firstName = Console.ReadLine();
                        Console.WriteLine("Please enter the year the patient was born (YYYY):");
                        input = Console.ReadLine();
                        // Validate the year to be the current year or earlier
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out birthYear) && birthYear < DateTime.Today.Year)
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid year (YYYY):");
                                input = Console.ReadLine();
                            }
                        }

                        Console.WriteLine("Please enter the month the patient was born (1-12):");
                        input = Console.ReadLine();

                        // Validate the month to be between 1 and 12
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out birthMonth) && birthMonth >= 1 && birthMonth <= 12)
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid month (1-12):");
                                input = Console.ReadLine();
                            }
                        }

                        Console.WriteLine("Please enter the day the patient was born (1-" + DateTime.DaysInMonth(birthYear, birthMonth) + "):");
                        input = Console.ReadLine();
                        // Validate the day to be valid for the month provided
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out birthDay) && birthDay >= 1 && birthDay <= DateTime.DaysInMonth(birthYear, birthMonth))
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid day (1-" + DateTime.DaysInMonth(birthYear, birthMonth) + "):");
                                input = Console.ReadLine();
                            }
                        }
                        // Create a new DateOnly object using the validated year, month, and day
                        birthdate = new DateOnly(birthYear, birthMonth, birthDay);
                        // Get the patient's priority
                        Console.WriteLine("Please enter the patient's priority (1-5):");
                        input = Console.ReadLine();
                        // Validate the priority to be between 1 and 5
                        valid = false;
                        while (!valid)
                        {
                            if (int.TryParse(input, out priority) && priority >= 1 && priority <= 5)
                            {
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid priority (1-5):");
                                input = Console.ReadLine();
                            }
                        }
                        // Add the patient to the priority queue
                        Console.WriteLine(Enqueue(new Patient(lastName, firstName, birthdate, priority)));
                        break;
                    case "P":
                        // Process the patient
                        Console.WriteLine(Dequeue(out Patient patient));
                        break;
                    case "L":
                        // List all patients
                        Console.WriteLine(ListAll().ToString());
                        break;
                    case "Q":
                        // Quit the program
                        exit = true;
                        break;
                }
            }
            //Adds a patient to the priority queue and the dictionary, returns the number of patients in the queue
            string Enqueue(Patient patient)
            {
                ERQueue.Enqueue(patient, (patient.GetPriority(), DateTime.Now));
                AllPatients.Add(patient, (patient.GetPriority(), DateTime.Now));
                return "The patient has been added. \n" +
                        "There are currently " + ERQueue.Count + " patients in the queue \n";
            }
            //Removes a patient from the priority queue and the dictionary
            string Dequeue(out Patient patient)
            {
                // if there are no patients in the queue, return a message
                if (ERQueue.Count == 0)
                {
                    patient = null;
                    return "No patients in queue \n";
                }
                ERQueue.TryDequeue(out patient, out _);
                AllPatients.Remove(patient);
                return "Processed: " + patient.ToString() + "\n";
            }
            //Accesses the dictionary and returns a string of all patients sorted by their position in the queue
            string ListAll()
            {
                // if there are no patients in the queue, return a message
                if (ERQueue.Count == 0)
                {
                    return "No patients in queue \n";
                }
                //order the dictionary by priority first, and then when they were added to the queue
                var orderedPatients = AllPatients.OrderBy(x => x.Value.Item1).ThenBy(x => x.Value.Item2);
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (KeyValuePair<Patient, (int, DateTime)> kvp in orderedPatients)
                {
                    count++;
                    sb.Append(kvp.Key.ToString() + " " + "Position in queue: " + count + "\n");
                }
                return sb.ToString();
            }
        }
    }
}