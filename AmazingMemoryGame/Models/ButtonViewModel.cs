using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazingMemoryGame.Models
{
    public class ButtonViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public bool IsMatched { get; set; }
        public bool BindingContext { get; set; }
    }


}
