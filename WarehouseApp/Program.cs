using System;
using System.Collections.Generic;
using WarehouseLibrary.Models;

namespace WarehouseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Load();

            while (true)
            {
                Console.Title = "Warehouse";
                Console.Clear();
                Console.WriteLine("1.Формирование приходной накладной");
                Console.WriteLine("2.Формирование расходной накладной");
                Console.WriteLine("3.Список товаров на складе");
                Console.WriteLine("4.Выход");
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.D1)
                {
                    Supplier supplier = RegisterSupplier();
                    List<Product> productList = new List<Product>();

                    while (true)
                    {
                        productList.Add(RegisterProduct());

                        Console.Write("Enter - продолжить ввод \n" +
                                      "Esc - вернуться в меню");
                        if (Console.ReadKey().Key != ConsoleKey.Enter)
                            break;
                    }

                    Supply supply = new Supply(supplier, productList);
                    warehouse.AddSupply(supply);
                    warehouse.PrintPurchaseInvoice(supply);

                    Console.Write("Нажмите любую кнопку, чтобы вернуться в меню");
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D2)
                {
                    Console.Clear();
                    if (warehouse.Products.Count == 0)
                    {
                        Console.WriteLine("Склад пуст.");
                        Console.Write("Нажмите любую кнопку, чтобы вернуться в меню");
                        Console.ReadKey();
                        continue;
                    }

                    string recipient = InputString("Получатель");
                    List<(Product, int)> productList = new List<(Product, int)>();

                    while (true)
                    {
                        Console.Clear();
                        warehouse.PrintAllProducts();
                        int number = InputProductNumber(warehouse.Products.Count);
                        warehouse.PrintProduct(number - 1);
                        int count = InputSalesProductCount(warehouse.Products[number - 1].Count);
                        productList.Add((warehouse.GetProduct(number - 1, count), count));

                        Console.Write("Enter - продолжить ввод \n" +
                                      "Esc - вернуться в меню");
                        if (Console.ReadKey().Key != ConsoleKey.Enter)
                            break;
                    }

                    warehouse.Save();
                    warehouse.PrintSalesInvoice(recipient, productList);

                    Console.Write("Нажмите любую кнопку, чтобы вернуться в меню");
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D3)
                {
                    Console.Clear();
                    if (warehouse.Products.Count == 0)
                    {
                        Console.WriteLine("Склад пуст.");
                        Console.Write("Нажмите любую кнопку, чтобы вернуться в меню");
                        Console.ReadKey();
                        continue;
                    }

                    warehouse.PrintAllProducts();
                    Console.WriteLine($"\nВсего товаров: {warehouse.Products.Count}");
                    Console.Write("Нажмите любую кнопку, чтобы вернуться в меню");
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D4)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Регистрирует данные поставщика
        /// </summary>
        /// <returns></returns>
        private static Supplier RegisterSupplier()
        {
            Console.Clear();
            Console.WriteLine("Данные поставщика:");
            string name = InputString("Наименование");
            string phoneNumber = InputString("Номер телефона");
            string address = InputString("Адрес");

            return new Supplier(name, phoneNumber, address);
        }

        /// <summary>
        /// Регистрирует данные товаров
        /// </summary>
        /// <returns></returns>
        private static Product RegisterProduct()
        {
            Console.Clear();
            Console.WriteLine("Данные товара:");
            string name = InputString("Наименование");
            string unit = InputString("Единица измерения");
            int count = InputInt("Количество");
            decimal price = InputDecimal("Цена (за единицу товара)");

            return new Product(name, unit, count, price);
        }

        /// <summary>
        /// Проверяет введенное пользователем значение и возвращает его
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static string InputString(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                string value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля. Введите значение заново.");
            }
        }

        /// <summary>
        /// Проверяет введенное пользователем значение и возвращает его
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static decimal InputDecimal(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");

                if (decimal.TryParse(Console.ReadLine(), out var value) && value > 0)
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат или значение. Введите значение заново.");
            }
        }

        /// <summary>
        /// Проверяет введенное пользователем значение и возвращает его
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static int InputInt(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                if (int.TryParse(Console.ReadLine(), out var value) && value > 0)
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат или значение. Введите значение заново.");
            }
        }

        /// <summary>
        /// Проверяет введенное пользователем значение и возвращает его
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int InputProductNumber(int count)
        {
            while (true)
            {
                int value = InputInt("Номер товара");
                if (value < 1 || value > count)
                {
                    Console.WriteLine("Нет товара с таким номером. Введите значение заново.");
                    continue;
                }

                return value;
            }
        }

        /// <summary>
        /// Проверяет введенное пользователем значение и возвращает его
        /// </summary>
        /// <param name="productCount"></param>
        /// <returns></returns>
        private static int InputSalesProductCount(int productCount)
        {
            while (true)
            {
                int value = InputInt("Количество");

                if (value < 1)
                {
                    Console.WriteLine("Значение не может быть меньше либо равно нулю. Введите значение заново.");
                    continue;
                }

                if (value > productCount)
                {
                    Console.WriteLine("Значение не может быть больше количества товара на складе. Введите значение заново.");
                    continue;
                }

                return value;
            }
        }
    }
}
