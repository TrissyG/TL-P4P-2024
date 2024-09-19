using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Step 1: Define the 36 presets
        List<int> presets = Enumerable.Range(1, 36).ToList();
        
        // Step 2: Create a HashSet to store unique orderings (HashSet ensures no duplicates)
        HashSet<string> uniqueOrderings = new HashSet<string>();
        Random rng = new Random();

        // Step 3: Generate 20 unique shuffled orderings
        while (uniqueOrderings.Count < 20)
        {
            // Shuffle the list of presets
            List<int> shuffledPresets = presets.OrderBy(x => rng.Next()).ToList();

            // Convert the list to a comma-separated string to store in the HashSet
            string presetString = string.Join(",", shuffledPresets);

            // Add the unique ordering to the HashSet
            uniqueOrderings.Add(presetString);
        }

        // Step 4: Save the unique orderings to a CSV file
        string filePath = "unique_orderings.csv";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            int orderCount = 1;

            // Write each ordering as a line in the CSV file
            foreach (string ordering in uniqueOrderings)
            {
                // Write the ordering number and the shuffled preset list
                writer.WriteLine($"Ordering {orderCount},{ordering}");
                orderCount++;
            }
        }

        Console.WriteLine("CSV file saved successfully.");
    }
}