using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazingMemoryGame.Models
{
    public class CardModel
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public bool IsMatched { get; set; }
        public bool BindingContext { get; set; }
        public string image_path { get; set; }

    }


}
