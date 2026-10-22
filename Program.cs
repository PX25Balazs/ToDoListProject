using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TodoConsoleApp
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Priority { get; set; } = "Vidēja";
        public string Deadline { get; set; } = "";
        public bool IsInThisWeek { get; set; } = false;
        public string WeekColumn { get; set; } = "Jāsāk";
    }

    class Program
    {
        private static string filePath = "tasks.json";
        private static List<TaskItem> tasks = new List<TaskItem>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            LoadTasks();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("       TO-DO SARAKSTS (PROJEKTS)        ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Visi uzdevumi (Skatīties, Pievienot, Dzēst)");
                Console.WriteLine("2. Šīs nedēļas saraksts (Kanban: Jāsāk -> Daru -> Pabeidzu)");
                Console.WriteLine("3. Iziet un saglabāt");
                Console.WriteLine("========================================");
                Console.Write("Izvēlieties darbību (1-3): ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    AllTasksMenu();
                }
                else if (choice == "2")
                {
                    ThisWeekMenu();
                }
                else if (choice == "3")
                {
                    SaveTasks();
                    break;
                }
            }
        }

        private static void LoadTasks()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
        }

        private static void SaveTasks()
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private static void AllTasksMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== VISI UZDEVUMI ===");

                var generalTasks = tasks.Where(t => !t.IsInThisWeek).ToList();

                if (generalTasks.Count == 0)
                {
                    Console.WriteLine("Saraksts pašlaik ir tukšs.");
                }
                else
                {
                    Console.WriteLine("{0,-4} | {1,-25} | {2,-10} | {3,-12}", "ID", "Uzdevums", "Prioritāte", "Termiņš");
                    Console.WriteLine(new string('-', 60));
                    foreach (var t in generalTasks)
                    {
                        Console.WriteLine($"{t.Id,-4} | {t.Title,-25} | {t.Priority,-10} | {t.Deadline,-12}");
                    }
                }

                Console.WriteLine("\nDarbības:");
                Console.WriteLine("1. Pievienot jaunu uzdevumu");
                Console.WriteLine("2. Paņemt uzdevumu uz 'Šīs nedēļas' sarakstu");
                Console.WriteLine("3. Dzēst uzdevumu");
                Console.WriteLine("4. Atgriezties galvenajā izvēlnē");
                Console.Write("Izvēlieties: ");

                string action = Console.ReadLine();

                if (action == "1")
                {
                    Console.Write("Ievadiet uzdevuma nosaukumu: ");
                    string title = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(title)) continue;

                    Console.Write("Ievadiet prioritāti (Augsta / Vidēja / Zema): ");
                    string priority = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(priority)) priority = "Vidēja";

                    Console.Write("Ievadiet termiņu (piem., 2026-06-01): ");
                    string deadline = Console.ReadLine();

                    int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
                    tasks.Add(new TaskItem { Id = newId, Title = title, Priority = priority, Deadline = deadline, IsInThisWeek = false, WeekColumn = "Jāsāk" });
                    SaveTasks();

                    Console.WriteLine("\nUzdevums veiksmīgi pievienots! Nospiediet jebkuru taustiņu...");
                    Console.ReadKey();
                }
                else if (action == "2")
                {
                    Console.Write("Ievadiet uzdevuma ID, kuru pārvietot uz nedēļu: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var t = tasks.FirstOrDefault(x => x.Id == id && !x.IsInThisWeek);
                        if (t != null)
                        {
                            t.IsInThisWeek = true;
                            t.WeekColumn = "Jāsāk";
                            SaveTasks();
                            Console.WriteLine("Uzdevums pārvietots uz šīs nedēļas sarakstu!");
                        }
                        else
                        {
                            Console.WriteLine("Uzdevums ar šādu ID netika atrasts.");
                        }
                    }
                    Console.WriteLine("Nospiediet jebkuru taustiņu...");
                    Console.ReadKey();
                }
                else if (action == "3")
                {
                    Console.Write("Ievadiet dzēšamā uzdevuma ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var t = tasks.FirstOrDefault(x => x.Id == id);
                        if (t != null)
                        {
                            tasks.Remove(t);
                            SaveTasks();
                            Console.WriteLine("Uzdevums izdzēsts!");
                        }
                    }
                    Console.ReadKey();
                }
                else if (action == "4")
                {
                    break;
                }
            }
        }

        private static void ThisWeekMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ŠĪS NEDĒĻAS SARAKSTS (KANBAN) ===");

                var weekTasks = tasks.Where(t => t.IsInThisWeek).ToList();

                Console.WriteLine("\n--- [ 1. JĀSĀK ] ---");
                foreach (var t in weekTasks.Where(x => x.WeekColumn == "Jāsāk"))
                {
                    Console.WriteLine($"  [ID: {t.Id}] {t.Title} (Prioritāte: {t.Priority}, Termiņš: {t.Deadline})");
                }

                Console.WriteLine("\n--- [ 2. DARU ] ---");
                foreach (var t in weekTasks.Where(x => x.WeekColumn == "Daru"))
                {
                    Console.WriteLine($"  [ID: {t.Id}] {t.Title} (Prioritāte: {t.Priority}, Termiņš: {t.Deadline})");
                }

                Console.WriteLine("\n--- [ 3. PABEIDZU ] ---");
                foreach (var t in weekTasks.Where(x => x.WeekColumn == "Pabeidzu"))
                {
                    Console.WriteLine($"  [ID: {t.Id}] {t.Title} (Pabeigts)");
                }

                Console.WriteLine("\nDarbības:");
                Console.WriteLine("1. Pārvietot uzdevumu starp kolonnām (Jāsāk -> Daru -> Pabeidzu)");
                Console.WriteLine("2. Izņemt no nedēļas saraksta / Dzēst");
                Console.WriteLine("3. Atgriezties galvenajā izvēlnē");
                Console.Write("Izvēlieties: ");

                string action = Console.ReadLine();

                if (action == "1")
                {
                    Console.Write("Ievadiet uzdevuma ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var t = tasks.FirstOrDefault(x => x.Id == id && x.IsInThisWeek);
                        if (t != null)
                        {
                            Console.WriteLine("Uz kurieni pārvietot?");
                            Console.WriteLine("1. Jāsāk");
                            Console.WriteLine("2. Daru");
                            Console.WriteLine("3. Pabeidzu");
                            Console.Write("Izvēle: ");
                            string col = Console.ReadLine();

                            if (col == "1") t.WeekColumn = "Jāsāk";
                            else if (col == "2") t.WeekColumn = "Daru";
                            else if (col == "3") t.WeekColumn = "Pabeidzu";

                            SaveTasks();
                            Console.WriteLine("Statuss veiksmīgi nomainīts!");
                        }
                        else
                        {
                            Console.WriteLine("Uzdevums nav atrasts šīs nedēļas sarakstā.");
                        }
                    }
                    Console.ReadKey();
                }
                else if (action == "2")
                {
                    Console.Write("Ievadiet uzdevuma ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var t = tasks.FirstOrDefault(x => x.Id == id);
                        if (t != null)
                        {
                            Console.WriteLine("1. Atgriezt atpakaļ uz 'Visi uzdevumi'");
                            Console.WriteLine("2. Pilnībā izdzēst");
                            string sub = Console.ReadLine();
                            if (sub == "1")
                            {
                                t.IsInThisWeek = false;
                                t.WeekColumn = "Jāsāk";
                            }
                            else if (sub == "2")
                            {
                                tasks.Remove(t);
                            }
                            SaveTasks();
                        }
                    }
                    Console.ReadKey();
                }
                else if (action == "3")
                {
                    break;
                }
            }
        }
    }
}