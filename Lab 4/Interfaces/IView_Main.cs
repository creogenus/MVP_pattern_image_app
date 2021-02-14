using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace Lab_4.Interfaces
{
    public interface IView_Main
    {
        string pictureName { get; set; }
        int childCounter { get; set; }
        int tag_activeChild { get; set; }
        string imageFormat { get; set; }
        bool can_distinguish { get; set; }

        event EventHandler OpenEvent;
        event EventHandler ActivateChildEvent;
        event EventHandler InformationAboutActiveChildEvent;
        event EventHandler SaveEvent;
        event EventHandler SaveAsEvent;
        event EventHandler CloseChildFromMaintEvent;
        event EventHandler CanDistinguishEvent;
        event EventHandler CutEvent;
        event EventHandler CopyEvent;
        event EventHandler PasteEvent;

        void CloseChild(int number_of_childs);
        void CantCreateChild(string picture_Name);
    }
}
