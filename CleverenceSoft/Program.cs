// See https://aka.ms/new-console-template for more information

using CleverenceSoft.Task_1;


while (true)
{
    Console.WriteLine("Введите номер задачи: \n 1 - компрессия и декомпрессия строки \n 2 - сервер статического класса \n 3 - стандартизация лог-файлов");
    string? task=Console.ReadLine();
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
        default:
            Console.WriteLine("Неверный номер задачи");
            break;
    }
}
