using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    class Message
    {
        public string Text
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public Message(string text, string title, string color, int duration = 3000)
        {
            this.Text = text;
            this.Title = title;
            this.Color = color;
            this.Duration = duration;
        }

    }
}
