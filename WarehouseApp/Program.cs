using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseLibrary.Models;

namespace WarehouseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // главное меню
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1.Формирование приходной накладной");
                Console.WriteLine("2.Формирование расходной накладной");
                Console.WriteLine("3.Перечень товаров на складе");
                Console.WriteLine("4.Выход");
                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.D1)
                {

                }
                else if (key == ConsoleKey.D2)
                {

                }
                else if (key == ConsoleKey.D3)
                {
                    OutputAllProducts(ProductsController.GetAllProducts());
                    Console.WriteLine("Esc - вернуться в меню");
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D4)
                    break;
            }
        }

        // формирование накладной
        private static void FormInvoice()
        {
            var selectedProductList = new List<(Product, int)>();

            while (true)
            {
                Console.Clear();
                var productList = ProductsController.GetAllProducts();
                OutputAllProducts(productList);
                Product product;
                int id;
                int count;

                while (true)
                {
                    id = ParseToInt("Id");
                    product = productList.SingleOrDefault(p => p.Id == id);
                    if (product != null) break;
                    Console.WriteLine($"Нет товара с id: {id}");
                    Console.WriteLine("Введите значение заново");
                }

                while (true)
                {
                    count = ParseToInt("Количество");
                    if (product.Count >= count) break;
                    Console.WriteLine($"Введенное значение ({count}) превышает количество товара на складе ({product.Count})");
                    Console.WriteLine("Введите значение заново");
                }

                selectedProductList.Add((product, count));

                Console.WriteLine("Enter - продолжить \nEsc - вывести накладную");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
            }

            ProductsController.GetProductsForInvoice(selectedProductList);
            //TODO: Добавить вывод накладной
            OutputInvoice(selectedProductList);
        }

        // формирование приходной
        private static List<Product> RegisterProducts()
        {
            Console.Title = "Register products";
            var productList = new List<Product>();

            while (true)
            {
                Console.Clear();

                var name = CheckForNull("Наименование");
                var unit = CheckForNull("Единица измерения");
                var count = ParseToInt("Количество");
                var price = ParseToDouble("Цена");
                //var price = ParseTo<double>("Цена", double.TryParse);
                var receiptDate = ParseToDateTime("Дата получения");

                productList.Add(new Product(name, unit, count, price));

                Console.WriteLine("Enter - продолжить ввод \nEsc - вернуться в меню");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
            }

            return productList;
        }

        private static void OutputInvoice(List<(Product, int)> productList)
        {
            // TODO: изменить вывод
            Console.Clear();
            Console.WriteLine($"{"Id",5}{"Наименование",15}{"Ед. измерения",15}{"Количество",15}{"Цена",15}{"Дата получения",20}");

            foreach (var (product, count) in productList)
                Console.WriteLine($"{product.Id,5}{product.Name,15}{product.Unit,15}"
                                  + $"{count,15}{product.Price,15}{product.ReceiptDate.ToShortDateString(),20}");

            Console.WriteLine("Esc - вернуться в меню");
            Console.ReadKey();
        }

        // вывод всех товаров
        private static void OutputAllProducts(List<Product> productList)
        {
            Console.Clear();
            Console.WriteLine($"{"Id",5}{"Наименование",15}{"Ед. измерения",15}{"Количество",15}{"Цена",15}{"Дата получения",20}");

            foreach (var product in productList)
                Console.WriteLine(product);

        }

        // ввод значения и проверка его на null
        private static string CheckForNull(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                var value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.ToLower();
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}.");
            }
        }

        //delegate bool TryParse<T>(string str, out T value);

        //private static T Input<T>(string nameOfField, TryParse<T> tryParse)
        //{
        //    while (true)
        //    {
        //        Console.Write($"{nameOfField} - ");
        //        if (tryParse(Console.ReadLine(), out var value))
        //        {
        //            return value;
        //        }

        //        Console.WriteLine($"Неверный формат поля {nameOfField}");
        //    }
        //}

        // ввод значения и проверка его на валидности
        private static double ParseToDouble(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                // TODO: Добавить ввод через точку
                if (double.TryParse(Console.ReadLine(), out var value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}");
            }
        }

        // ввод значения и проверка его валидности
        private static int ParseToInt(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                if (int.TryParse(Console.ReadLine(), out var value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}");
            }
        }

        // ввод значения и проверка его на валидности
        private static DateTime ParseToDateTime(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");
                if (DateTime.TryParse(Console.ReadLine(), out var value))
                {
                    return value;
                }

                Console.WriteLine($"Неверный формат поля {nameOfField}");
            }
        }
    }
}
