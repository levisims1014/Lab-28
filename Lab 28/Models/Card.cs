﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab28.Models
{
    public class Card
    {
        public Card()
        {
            Image = "";
            Value = "";
            Suit = "";
        }
        public Card(string image, string value, string suit)
        {
            Image = image;
            Value = value;
            Suit = suit;
        }

        public string Image { get; set; }
        public string Value { get; set; }
        public string Suit { get; set; }
    }
}