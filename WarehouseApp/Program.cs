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
                Console.Clear();
                Console.WriteLine("1.Формирование приходной накладной");
                Console.WriteLine("2.Формирование расходной накладной");
                Console.WriteLine("3.Список товаров на складе");
                Console.WriteLine("4.Выход");
                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.D1)
                {
                    Console.Clear();
                    Console.Title = "Register supplier";
                    Supplier supplier = RegisterSupplier();

                    Console.Title = "Register products";
                    var productList = new List<Product>();

                    while (true)
                    {
                        productList.Add(RegisterProduct());

                        Console.WriteLine("Enter - продолжить ввод \nEsc - вернуться в меню");
                        if (Console.ReadKey().Key == ConsoleKey.Escape)
                            break;
                    }

                    Supply supply = new Supply(supplier, productList);
                    warehouse.AddSupply(supply);
                    warehouse.PrintPurchaseInvoice(supply);

                    Console.WriteLine("Нажмите любую кнопку, чтобы вернуться в меню");
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D2)
                {
                    throw new NotImplementedException();
                }
                else if (key == ConsoleKey.D3)
                {
                    Console.Clear();
                    warehouse.PrintAllProducts();
                    Console.WriteLine("Нажмите любую кнопку, чтобы вернуться в меню");
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
            var name = InputString("Наименование");
            var phoneNumber = InputString("Номер телефона");
            var address = InputString("Адрес");

            return new Supplier(name, phoneNumber, address);
        }

        /// <summary>
        /// Регистрирует данные товаров
        /// </summary>
        /// <returns></returns>
        private static Product RegisterProduct()
        {
            Console.Clear();
            var name = InputString("Наименование");
            var unit = InputString("Единица измерения");
            var count = InputInt("Количество");
            var price = InputDecimal("Цена");

            return new Product(name, unit, count, price);
        }

        /// <summary>
        /// Возвращает строку
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static string InputString(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                var value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}. Введите значение заново.");
            }
        }


        /// <summary>
        /// Возвращает десимал
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static decimal InputDecimal(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");

                if (decimal.TryParse(Console.ReadLine(), out var value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}. Введите значение заново.");
            }
        }

        /// <summary>
        /// Возвращает целое число
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static int InputInt(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");

                if (int.TryParse(Console.ReadLine(), out var value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}. Введите значение заново.");
            }
        }
    }
}
