# FileSignature
Тестовое задание в компании Veeam
Консольная программа на C# для генерации сигнатуры указанного файла. 

Сигнатура генерируется следующим образом: исходный файл делится на блоки заданной длины (кроме последнего блока), для каждого блока вычисляется значение hash-функции SHA256, и вместе с его номером выводится в консоль. 
Программа должна уметь обрабатывать файлы, размер которых превышает объем оперативной памяти, и при этом максимально эффективно использовать вычислительные мощности многопроцессорной системы. 
При работе с потоками допускается использовать только стандартные классы и библиотеки из .Net (исключая ThreadPool, BackgroundWorker, TPL)
Ожидается реализация с использованием Thread-ов.
Путь до входного файла и размер блока задаются в командной строке.
В случае возникновения ошибки во время выполнения программы ее текст и StackTrace необходимо вывести в консоль.

To start application:
Veeam.FileSignature.exe <file path> <block size in byte> <thread count/ optional>

For example:
Veeam.FileSignature.exe "D:\get8_tpg_v52\vinyl.lbh" 524288
vinyl.lbh - some file 4.1Gb
524288 - 512 Kbytes block size

Veeam.FileSignature.exe "D:\get8_tpg_v52\vinyl.lbh" 524288 5
vinyl.lbh - some file 4.1Gb
524288 - 512 Kbytes block size
5 - count of threads which will be created to process data file

Core count: 6 CPUs ( 12 virtual) Intel Core i7-8700
Some results for file of 1.7Gb

Block size: 524288 bytes (512 Kb) - 3501 blocks
Threads count:	2	3	4	5	6	12
Time:	00:04.26	00:03.29	00:02.64	00:02.55	00:02.37	00:02.32

Block size: 1048576 bytes (1 Mb) - 1750 blocks
Threads count:	2	3	4	5	6	12
Time:	00:04.04	00:02.94	00:02.52	00:02.23	00:02.13	00:02.06

Block size: 10485760 bytes (10 Mb) - 175 blocks
Threads count:	2	3	4	5	6	12
Time:	00:04.19	00:03.03	00:02.48	00:02.24	00:01.99	00:01.95

Block size: 52428800 bytes (50 Mb) - 35 blocks
Threads count:	2	3	4	5	6	12
Time:	00:04.21	00:02.97	00:02.57	00:02.24	00:02.07	00:02.59
