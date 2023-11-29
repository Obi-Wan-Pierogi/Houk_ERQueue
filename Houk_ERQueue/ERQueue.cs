using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houk_ERQueue
{
    internal class ERQueue
    {
        // Create a priority queue that stores the patients their priority
        public static PriorityQueue<Patient, int> erQueue = new PriorityQueue<Patient, int>();
        
        // Add a patient to the priority queue and return the number of patients in the queue
        public static string Enqueue(Patient patient)
        {
            erQueue.Enqueue(patient, patient.GetPriority());
            return "You have been added. \n" +
                    "There are currently " + erQueue.Count + " patients in the queue";
        }
        // Remove a patient from the priority queue
        public static string Dequeue(out Patient patient)
        {
            // if there are no patients in the queue, return a message
            if (erQueue.Count == 0)
            {
                patient = null;
                return "No patients in queue \n";
            }
            erQueue.TryDequeue(out patient, out _);
            return "Processed: " + patient.ToString() + "\n";
        }
    }
}
