using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WarehouseLibrary.Models;

namespace WarehouseApp
{
    class Program
    {
        private const string ErrorMessage = "Неверный формат поля или значение. Введите значение заново.";
        private const string ClickAnyButtonMessage = "Нажмите любую клавишу, чтобы вернуться в меню.";
        private const string PurchaseInvoiceDirectoryPath = "purchaseInvoices";
        private const string SalesInvoicesDirectoryPath = "salesInvoices";


        static void Main(string[] args)
        {
            Console.WindowWidth = 135;
            Console.Title = "Warehouse";
            Warehouse warehouse = new Warehouse();
            warehouse.Load();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1.Формирование приходной накладной");
                Console.WriteLine("2.Формирование расходной накладной");
                Console.WriteLine("3.Список товаров на складе");
                Console.WriteLine("4.Поиск товара");
                Console.WriteLine("5.Просмотр приходных накладных");
                Console.WriteLine("6.Просмотр расходных накладных");
                Console.WriteLine("7.Выход");
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.D1)
                {
                    Supplier supplier = RegisterSupplier();
                    List<Product> productList = new List<Product>();
                    List<(Product, Product)> duplicateList = new List<(Product, Product)>();

                    while (true)
                    {
                        var product = RegisterProduct(supplier);
                        Product duplicate = warehouse.DuplicateProduct(product);

                        if (duplicate != null)
                        {

                            Console.Write("\nНа складе находится такой же товар.\n" +
                                              "1.Создать новую запись\n" +
                                              "2.Соединить товары в одну запись");

                            if (Console.ReadKey().Key == ConsoleKey.D1)
                            {
                                productList.Add(product);
                            }
                            else
                            {
                                duplicateList.Add((product, duplicate));
                            }
                        }
                        else
                        {
                            productList.Add(product);
                        }

                        Console.Write("\nEnter - добавить еще один товар \n" +
                                      "Esc - вернуться в меню");

                        if (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            break;
                        }
                    }

                    if (productList.Count > 0)
                    {
                        warehouse.AddSupply(new Supply(supplier, productList));
                    }

                    foreach ((Product product, Product duplicate) in duplicateList)
                    {
                        warehouse.AddSomeCountOfProduct(duplicate, product.Count);
                        productList.Add(product);
                    }

                    warehouse.Save();
                    warehouse.PrintPurchaseInvoice(new Supply(supplier, productList));

                    Console.Write(ClickAnyButtonMessage);
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D2)
                {
                    Console.Clear();
                    if (warehouse.Products.Count == 0)
                    {
                        Console.WriteLine("Склад пуст.");
                    }
                    else
                    {

                        string recipient = InputString("Покупатель");
                        List<(Product, int)> productList = new List<(Product, int)>();

                        while (true)
                        {
                            Console.Clear();

                            if (warehouse.Products.Count == 0)
                            {
                                Console.WriteLine("Склад пуст.");
                                Console.WriteLine(ClickAnyButtonMessage);
                                Console.ReadKey();
                                break;
                            }

                            warehouse.PrintAllProducts();
                            int number = InputProductNumber(warehouse.Products.Count);
                            int count = InputSalesProductCount(warehouse.Products[number - 1].Count);
                            Product product = warehouse.Products[number - 1];
                            warehouse.RemoveSomeCountOfProduct(product, count);
                            productList.Add((product, count));

                            if (warehouse.Products.Count == 0)
                            {
                                break;
                            }

                            Console.Write("Enter - продолжить ввод \n" +
                                          "Esc - вернуться в меню");
                            if (Console.ReadKey().Key != ConsoleKey.Enter)
                            {
                                break;
                            }
                        }

                        warehouse.Save();
                        warehouse.PrintSalesInvoice(recipient, productList);
                    }

                    Console.Write(ClickAnyButtonMessage);
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D3)
                {
                    Console.Clear();
                    if (warehouse.Products.Count == 0)
                    {
                        Console.WriteLine("Склад пуст.");
                    }
                    else
                    {
                         warehouse.PrintAllProducts();
                         Console.WriteLine($"\nВсего товаров: {warehouse.Products.Count}\n");
                    }

                    Console.Write(ClickAnyButtonMessage);
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.D4)
                {
                    while (true)
                    {
                        Console.Clear();
                        var name = InputString("Наименование");
                        var products = warehouse.SearchProducts(name);

                        if (products.Count == 0)
                        {
                            Console.WriteLine("Товар не найден.");
                        }
                        else
                        {
                            warehouse.PrintProducts(products);
                        }

                        Console.Write("\nEnter - искать другой товар \n" + 
                                      "Esc - вернуться в меню");
                        if (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            break;
                        }
                    }
                }
                else if (key == ConsoleKey.D5)
                {
                    if (Directory.Exists(PurchaseInvoiceDirectoryPath) && Directory.GetFiles(SalesInvoicesDirectoryPath).Length > 0)
                    {
                        Process.Start(PurchaseInvoiceDirectoryPath);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Список приходных накладных пуст.");
                        Console.Write(ClickAnyButtonMessage);
                        Console.ReadKey();
                    }
                }
                else if (key == ConsoleKey.D6)
                {
                    if (Directory.Exists(SalesInvoicesDirectoryPath) && Directory.GetFiles(SalesInvoicesDirectoryPath).Length > 0)
                    {
                        Process.Start(SalesInvoicesDirectoryPath);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Список расходных накладных пуст.");
                        Console.Write(ClickAnyButtonMessage);
                        Console.ReadKey();
                    }
                }
                else if (key == ConsoleKey.D7)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Регистрирует данные поставщика и возвращает его
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
        /// Регистрирует данные товара и возваращает его
        /// </summary>
        /// <returns></returns>
        private static Product RegisterProduct(Supplier supplier)
        {
            Console.Clear();
            Console.WriteLine("Данные товара:");
            string name = InputString("Наименование");
            string unit = InputString("Единица измерения");
            int count = InputPositiveInt("Количество");
            decimal price = InputPositiveDecimal("Цена (за единицу товара)");

            return new Product(name, unit, count, price, supplier);
        }

        /// <summary>
        /// Проверяет не является ли введенное значение null или пустой строкой и возвращает его
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
                    return value.Trim();
                }

                Console.WriteLine(ErrorMessage);
            }
        }

        /// <summary>
        /// Проверяет является ли введенное значение положительным целым числом и возвращает его
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static decimal InputPositiveDecimal(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");

                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value > 0)
                {
                    return value;
                }

                Console.WriteLine(ErrorMessage);
            }
        }

        /// <summary>
        /// Проверяет является ли введенное значение положительным десятичным числом и возвращает его
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private static int InputPositiveInt(string nameOfField)
        {
            while (true)
            {
                Console.Write($"{nameOfField} - ");

                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                {
                    return value;
                }

                Console.WriteLine(ErrorMessage);
            }
        }

        /// <summary>
        /// Проверяет находится ли введенное значение в диапазоне от нуля до заданного значения и возвращает его
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int InputProductNumber(int count)
        {
            while (true)
            {
                int value = InputPositiveInt("Номер товара");

                if (value < 1 || value > count)
                {
                    Console.WriteLine("Нет товара с таким номером. Введите значение заново.");
                    continue;
                }

                return value;
            }
        }

        /// <summary>
        /// Проверяет не превышает ли введенное значение количество товара на складе и возвращает его
        /// </summary>
        /// <param name="productCount"></param>
        /// <returns></returns>
        private static int InputSalesProductCount(int productCount)
        {
            while (true)
            {
                int value = InputPositiveInt("Количество");

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