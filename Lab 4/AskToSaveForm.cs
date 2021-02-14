using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_4
{
    //форма, предназначенная для коммуникации с пользвователем
    //"нужно ли сохранять изменения?"
    public partial class AskToSaveForm : Form, Interfaces.IView_AskToSaveForm
    {
        #region Interface realization
        public bool Dialog()
        {
            //спрашиваем пользователя, нужно ли сохранять изменения
            if (this.ShowDialog() == DialogResult.Yes) return true;
            else return false;
        }
        #endregion
        public AskToSaveForm()
        {
            InitializeComponent();
        }
        
    }
}
