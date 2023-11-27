using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houk_ERQueue
{
    internal class ERQueue
    {
        //Creates a priority queue that stores the patients, their priority, and a timestamp for stable sorting
        private PriorityQueue<Patient, (int, DateTime)> erQueue = new PriorityQueue<Patient, (int, DateTime timestamp)>();
        //Creates a dictionary that stores the patients, their priority, and a timestamp to be used for listing all patients
        Dictionary<Patient, (int, DateTime)> allPatients = new Dictionary<Patient, (int, DateTime)>();


        //Adds a patient to the priority queue and the dictionary, returns the number of patients in the queue
        public string Enqueue(Patient patient)
        {
            erQueue.Enqueue(patient, (patient.GetPriority(), DateTime.Now));
            allPatients.Add(patient, (patient.GetPriority(), DateTime.Now));
            return "You have been added. \n" +
                    "There are currently " + erQueue.Count + " patients in the queue";
        }
        //Removes a patient from the priority queue and the dictionary
        public string Dequeue(out Patient patient)
        {
            // if there are no patients in the queue, return a message
            if (erQueue.Count == 0)
            {
                patient = null;
                return "No patients in queue \n";
            }
            erQueue.TryDequeue(out patient, out _);
            allPatients.Remove(patient);
            return "Processed: " + patient.ToString() + "\n";
        }

        //Accesses the dictionary and returns a string of all patients and their position in the queue
        public string ListAll()
        {
            // if there are no patients in the queue, return a message
            if (erQueue.Count == 0)
            {
                return "No patients in queue \n";
            }
            //order the dictionary by priority first,  and then when they were added to the queue
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
