namespace NewLauncher.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public class NewsModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime PostTime { get; set; }

        public string Title { get; set; }
    }
}

