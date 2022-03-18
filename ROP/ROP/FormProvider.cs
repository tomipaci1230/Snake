using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROP
{
    class FormProvider
    {
        public static Form1 MainMenu
        {
            get
            {
                if (_mainMenu == null)
                {
                    _mainMenu = new Form1();
                }
                return _mainMenu;
            }
        }
        private static Form1 _mainMenu;
    }
}
