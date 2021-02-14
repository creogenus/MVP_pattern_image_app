using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lab_4.Interfaces
{
    public interface IView_Childs
    {
        string pictureName { get; set; }
        Image image { get; set; }
        int tag { get; set; }
        bool distinguishable { get; set; }
        Image image_for_copy { get; set; }
        Image image_for_paste { get; set; }
        event EventHandler OnClosingChild;

        void SetActive();
        void GetMetaData(string data_string);
        bool IfActivated();
        void Save();
        void SaveAs();
        bool CheckChange();
        void CloseFromMain();
        void Cut();
        void UpdatePicture();     
        Image Copy();
        void Paste(Image image);
        void Backup();
        int GetX_Current();
        void UploadAfterSaving();
    }
}
