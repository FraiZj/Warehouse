﻿using System;

namespace WarehouseLibrary.Models
{
    public class Product
    {
        public int Id { get; set; }
        private string Name { get; set; }
        private string Unit { get; set; }
        private int Count { get; set; }
        private double Price { get; set; }
        private DateTime ReceiptDate { get; set; }

        public Product()
        {

        }

        public Product(string name, string unit, int count, double price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Имя не может быть пустой строкой.");
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new ArgumentNullException(nameof(name), "Единица измерения не может быть пустой строкой.");
            }

            if (count < 1)
            {
                throw new ArgumentException("Количество не может быть меньше или равно нулю.", nameof(count));
            }

            if (price <= 0)
            {
                throw new ArgumentException("Цена не может быть меньше или равно нулю.", nameof(price));
            }


            Name = name;
            Unit = unit;
            Count = count;
            Price = price;
            ReceiptDate = DateTime.Today;
        }

        public override string ToString()
        {
            return $"{Id,5}{Name,15}{Unit,15}{Count,15}{Price,15}{ReceiptDate.ToShortDateString(),20}";
        }
    }
}