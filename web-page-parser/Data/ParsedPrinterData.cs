﻿namespace web_page_parser.Data
{
    public class ParsedPrinterData
    {
        private int numberOfPages;
        private int drumLevel;
        private int tonerLevel;

        public int NumberOfPages { get => numberOfPages; set => numberOfPages = value; }
        public int DrumLevel { get => drumLevel; set => drumLevel = value; }
        public int TonerLevel { get => tonerLevel; set => tonerLevel = value; }
    }
}