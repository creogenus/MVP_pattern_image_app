using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace Lab_4
{
    //это код основной формы, которая является Mdi-контейнером для дочерних форм Childs
    //из этой формы осуществялется вызов всех основных функций приложения
    public partial class Main : Form, Interfaces.IView_Main
    {
        #region Interface realization
        //переменная для хранения и передачи имени выбраного файла в дочернюю форму
        public string pictureName { get; set; } 
        //переменная для нумерации открытых  окон 
        public int childCounter { get; set; }
        //переменная для хранения тэга активного дочернего окна
        public int tag_activeChild { get; set; }
        //переменная для хранения формата, в уотором нужно сохранить изображение из актинвого дочернего окна
        public string imageFormat { get; set; }
        //переменная для хранения информации о возмоэности выделения области в дочерних окнах
        public bool can_distinguish { get; set; }

        //события 
        public event EventHandler OpenEvent;
        public event EventHandler ActivateChildEvent;
        public event EventHandler InformationAboutActiveChildEvent;
        public event EventHandler SaveEvent;
        public event EventHandler SaveAsEvent;
        public event EventHandler CloseChildFromMaintEvent;
        public event EventHandler CanDistinguishEvent;
        public event EventHandler CutEvent;
        public event EventHandler CopyEvent;
        public event EventHandler PasteEvent;

        public void CloseChild(int number_of_child)
        {
            //удаляем лишнее окно
            WindowToolStripMenuItem.DropDownItems.Remove(WindowToolStripMenuItem.DropDownItems[number_of_child]);
        }

        public void CantCreateChild(string picture_Name)
        {
            //удаляем лишнее окно
            WindowToolStripMenuItem.DropDownItems.RemoveAt(WindowToolStripMenuItem.DropDownItems.Count-1);
            childCounter--;
            //выводим сообщение о том, что файл открыт
            MessageBox.Show("Файл: " + picture_Name + "уже открыт!");
        }
        #endregion

        public Main()
        {
            //задаем изначальное количество дочерних форм
            childCounter = 1;
            InitializeComponent();
        }

        public void Run()
        {
            //запускаем форму
            Application.Run(this);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //выходим из программы
            this.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //открываем файловый браузер для выбора нужной картинки, если нажата кнокпка ОК,
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureName = openFileDialog.FileName;
                //создаем новый элемент подменю
                ToolStripMenuItem new_window = new ToolStripMenuItem();
                new_window.Tag = childCounter;
                new_window.Text = "Окно " + new_window.Tag;
                new_window.Click += new EventHandler(OnWindow_Click);
                WindowToolStripMenuItem.DropDownItems.Add(new_window);
                //вызываем событие, которое создает дочернюю форму и отображает картинку
                if (OpenEvent != null) OpenEvent(this, EventArgs.Empty);                
                childCounter++;
            }
        }

        public void OnWindow_Click(object sender, EventArgs e)
        {

            //переводим фокус на нужное нам окно
            tag_activeChild = Convert.ToInt32((sender as ToolStripMenuItem).Tag);
            if (ActivateChildEvent != null) ActivateChildEvent(EventArgs.Empty, e);
        }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вызыввем событие получения информации об активном окне
            if (InformationAboutActiveChildEvent != null) InformationAboutActiveChildEvent(EventArgs.Empty, e);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вызываем событие сохранения файла         
            if (SaveEvent != null) SaveEvent(EventArgs.Empty, e);
        }

        private void SaveAS_Click(object sender, EventArgs e)
        {
            //проверяем какой именно элемент подменю "Сохранить как" нажат           
            for (int i = 0; i < SaveAsToolStripMenuItem.DropDownItems.Count; i++)
                if (SaveAsToolStripMenuItem.DropDownItems[i].Pressed)
                {
                    //записываем формат в переменную
                    imageFormat = Convert.ToString(SaveAsToolStripMenuItem.DropDownItems[i].Tag);
                }
            //вызываем событие "сохранить как"
            if (SaveAsEvent != null) SaveAsEvent(EventArgs.Empty, e);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //подписываем каждую кнопку из "сохранить как" на событие 
            for (int i = 0; i < SaveAsToolStripMenuItem.DropDownItems.Count; i++)
                SaveAsToolStripMenuItem.DropDownItems[i].Click += new EventHandler(SaveAS_Click);
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вызываем событие закрытия активной формы
            if (CloseChildFromMaintEvent != null) CloseChildFromMaintEvent(EventArgs.Empty, e);
        }

        private void выделениеПрямоугольнойОбластиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //отмечаем или снимаем отметку, по которой смотрим разрешение на выделение области
            выделениеПрямоугольнойОбластиToolStripMenuItem.Checked = !выделениеПрямоугольнойОбластиToolStripMenuItem.Checked;
            if (выделениеПрямоугольнойОбластиToolStripMenuItem.Checked) can_distinguish = true;
            else
            {
                can_distinguish = false;
            }
            //вызываем событие, которое разрешит или запретит всем дочерним формам выделять область
            if (CanDistinguishEvent != null) CanDistinguishEvent(EventArgs.Empty, e);
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вызываем событие для вырезания выделенной области в активной дочерней форме
            if (CutEvent != null) CutEvent(EventArgs.Empty, e);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вызываем событие для копирование выделенной области в активной дочерней форме
            if (CopyEvent != null) CopyEvent(EventArgs.Empty, e);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вызываем событие для вставки скопированной области в активную дочернюю форму
            if (PasteEvent != null) PasteEvent(EventArgs.Empty, e);
        }
    }
}
