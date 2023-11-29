using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houk_ERQueue
{
    internal class PatientDictionary
    {
        // Create a dictionary that stores the patients, their priority, and a timestamp to be used for listing all patients
        public static Dictionary<Patient, (int, DateTime)> allPatients = new Dictionary<Patient, (int, DateTime)>();

        // Add a patient to the dictionary with priority and timestamp for stable sorting
        public static void Add(Patient patient)
        {
            allPatients.Add(patient, (patient.GetPriority(), DateTime.Now));
        }

        // Remove a patient from the dictionary
        public static void Remove(Patient patient)
        {
            allPatients.Remove(patient);
        }

        // Access the dictionary and return a string of all patients and their position in the queue
        public static string ListAll()
        {
            // If there are no patients in the queue, return a message
            if (allPatients.Count == 0)
            {
                return "No patients in queue \n";
            }
            // Order the dictionary by priority first,  and then when they were added to the queue
            var orderedPatients = allPatients.OrderBy(x => x.Value.Item1).ThenBy(x => x.Value.Item2);
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
