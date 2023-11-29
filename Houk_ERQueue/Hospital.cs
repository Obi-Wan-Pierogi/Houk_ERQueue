using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houk_ERQueue
{
    internal class Hospital
    {
        // Add a patient to the queue and the dictionary
        public string Add(Patient patient)
        {
            PatientDictionary.Add(patient);            
            return ERQueue.Enqueue(patient);
        }

        // Remove a patient from the queue and the dictionary
        public string Process()
        {
            var patientToRemove = ERQueue.erQueue.Peek();
            PatientDictionary.Remove(patientToRemove);
            return ERQueue.Dequeue(out patientToRemove);
        }

        // List all patients in the queue, with position and priority
        public string ListAll()
        {
            return PatientDictionary.ListAll();
        }
    }
}
