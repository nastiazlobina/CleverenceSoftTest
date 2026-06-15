
using CleverenceSoft.Task_1;
using CleverenceSoft.Task_3;


while (true)
{
    Console.WriteLine(
        "Введите номер задачи: \n 1 - компрессия и декомпрессия строки \n 2 - сервер статического класса \n 3 - стандартизация лог-файлов");
    string? task = Console.ReadLine();
    switch (task)
    {
        case "1":
            Console.Write("Исходная строка: ");
            string? originString = Console.ReadLine();
            CompressionAlgorithm compressionAlgorithm = new CompressionAlgorithm(originString);
            string compression = compressionAlgorithm.Compression();
            Console.WriteLine($"Сжатая строка: {compression}");
            string decompression = compressionAlgorithm.Decompression(compression);
            Console.WriteLine($"Декомпрессия сжатой строки: {decompression}");
            break;
        case "2":

            Console.WriteLine("Запуск теста сервера");

            var tasks = new List<Task>();
            
            for (int i = 0; i < 5; i++)
            {
                int readerId = i;

                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Console.WriteLine(
                            $"Читатель {readerId}: {Server.GetCount()}");

                        Thread.Sleep(100);
                    }
                }));
            }
            
            for (int i = 0; i < 3; i++)
            {
                int writerId = i;

                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Server.AddToCount(1);

                        Console.WriteLine(
                            $"Писатель {writerId}: +1");

                        Thread.Sleep(250);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(
                $"Итоговое значение count = {Server.GetCount()}");

            break;
        
        case "3":
        
            Console.WriteLine("Запуск обработки логов");

            var baseDir = AppContext.BaseDirectory;

            var input = Path.Combine(baseDir, "Task 3", "input.txt");
            var output = Path.Combine(baseDir, "Task 3", "output.txt");
            var problems = Path.Combine(baseDir, "Task 3", "problems.txt");

            LogProcessor.Process(input, output, problems);

            Console.WriteLine("Файл обработан, результаты находятся в папке с bin");
            break;
        
        
        default:
            Console.WriteLine("Неверный номер задачи");
            break;
    }
}
