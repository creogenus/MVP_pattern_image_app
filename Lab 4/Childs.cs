using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Lab_4
{
    //это код дочерней формы, в которой происходят все преобразования изображения
    public partial class Childs : Form, Interfaces.IView_Childs
    {
        #region Interface reaalization
        //переменная для хранения пути
        public string pictureName { get; set; }
        //переменная хранения изображения
        public Image image { get; set; }
        //переменная хранения тэга
        public int tag { get; set; }
        //переменная разрешения выделять область
        public bool distinguishable { get; set; }
        //пременная для хранения изображения копирования
        public Image image_for_copy { get; set; }
        //переменная для хранения изображения вставки
        public Image image_for_paste { get; set; }

        //событие закрытия формы
        public event EventHandler OnClosingChild;

        public void GetMetaData(string data_string)
        {
            //выводим информацию об изображении
            MessageBox.Show(data_string);
        }

        public void SetActive()
        {
            //активируем данную форму
            this.Activate();
        }

        public bool IfActivated()
        {
            //возвращаем активна ли форма
            return this.Focused;
        }

        public void Save()
        {
            //проверяем активна ли эта форма
            if (IfActivated())
            {
                //выводим сообщение о сохранении файла
                MessageBox.Show("Файл сохранен!");
                //осовбождаем ресурсы
                for (int i = 0; i < image_variations.Count; i++)
                {
                    image_variations[i].Dispose();
                }
                //очищаем список вариаций изображения
                image_variations.Clear();
            }
        }
        public void SaveAs()
        {
            //выводим сообщение о сохранении файла
            if (IfActivated())
                MessageBox.Show("Файл сохранен!");
        }
        public bool CheckChange()
        {
            //возвращаем изменялось ли изображение
            if (image_variations.Count > 1) return true;
            else return false;
        }
        public void CloseFromMain()
        {
            //закрываем форму
            this.Close();
        }
        public void Cut()
        {
            //вырезаем выделенную область в изображении
            PaintRectangle(Color.White);
        }
        public void UpdatePicture()
        {
            Rectangle rectf;
            //загружаем последнюю картинку, которая была использована
            grfx = this.CreateGraphics();
            rectf = ClientRectangle;
            grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
            grfx.Dispose();
        }
        public Image Copy()
        {
            Rectangle rct;
            //проверяем координаты курсора и изначального положениия курсора для правильного копирования
            if (x_current > x_initial || y_current > y_initial)
            {
                //выделяем нужный участок и наносим его на bitmap_for_copy
                rct = new Rectangle(x_initial, y_initial, x_current - x_initial, y_current - y_initial);
                bitmap_for_copy = new Bitmap(rct.Width, rct.Height);
                grfx = Graphics.FromImage(bitmap_for_copy);
                grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rct, GraphicsUnit.Pixel);
            }
            else
            {
                rct = new Rectangle(x_current, y_current, x_initial - x_current, y_initial - y_current);
                bitmap_for_copy = new Bitmap(rct.Width, rct.Height);
                grfx = Graphics.FromImage(bitmap_for_copy);
                grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rct, GraphicsUnit.Pixel);
            }
            grfx.Dispose();
            //возвращение изображение из bitmap_for_copy            
            return image_for_copy = Image.FromHbitmap(bitmap_for_copy.GetHbitmap());
        }
        public void Paste(Image img)
        {
            Rectangle rectf;
            //присавиваем image_for_paste значение изображения, которое было скопировано
            image_for_paste = img;
            rectf = ClientRectangle;
            bitmap = new Bitmap(rectf.Width, rectf.Height);
            bitmap_for_paste = new Bitmap(img);
            //выводим изображения
            grfx = Graphics.FromImage(bitmap);
            grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
            grfx = CreateGraphics();
            grfx.DrawImage(img, rectf.Width / 2, rectf.Height / 2, bitmap_for_paste.Width, bitmap_for_paste.Height);
            pen = new Pen(Color.Green);
            grfx.DrawRectangle(pen, rectf.Width / 2, rectf.Height / 2, bitmap_for_paste.Width, bitmap_for_paste.Height);
            //разрешаем двигать втсавленную картинку
            can_drag = true;
            //освобождаем ресурсы
            grfx.Dispose();
            bitmap.Dispose();
            bitmap_for_paste.Dispose();
        }
        public void Backup()
        {
            //проверяем наличие вариаций изображений
            if (image_variations.Count > 1)
            {
                Rectangle rectf;
                rectf = ClientRectangle;
                //удаляем текущее изображение
                image_variations[image_variations.Count - 1].Dispose();
                image_variations.RemoveAt(image_variations.Count - 1);
                grfx = CreateGraphics();
                //выводим предыдущее изображение
                grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
                grfx.Dispose();
            }
        }
        public int GetX_Current()
        {
            return x_current;
        }

        public void UploadAfterSaving()
        {
            //загружаем файл после сохранения
            image = Image.FromFile(pictureName);
            image_variations.Add(image);
            Update();
        }
        #endregion

        #region Extra Variables
        //переменная, сообщающая была ли нажата кнопка мыши
        bool clicked;

        //перо
        Pen pen;

        //переменная, сообщающая можно ли переносить вставленную картинку
        bool can_drag = false;
        //переменная разниц в координатах для корректного перетаскивания вставленной картинки
        int difference_x, difference_y;

        //прямоугольник
        Rectangle rectf;

        //набор биткарт
        Bitmap bitmap;
        Bitmap bitmap_for_copy;
        Bitmap bitmap_for_paste;

        //графика для рисования и изображений
        Graphics g;
        Graphics grfx;

        //список вариаций изображений
        List<Image> image_variations = new List<Image>();

        //наборы координат для расчета области выделения
        int x_initial, y_initial, x_current = -1, y_current = -1;
        #endregion
        public Childs()
        {
            InitializeComponent();
        }
        public void Run()
        {
            //запускаем приложение
            Application.Run(this);
        }

        private void Childs_Load(object sender, EventArgs e)
        {            
            this.Tag = tag;
            //задаем название форме по тэгу
            string[] split_array = pictureName.Split(Convert.ToChar(@"\"));
            this.Text = "Окно " + this.tag + " - " + split_array[split_array.Length - 1];
            //задаем image путь, из которого возьмем картинку
            image = Image.FromFile(pictureName);
            this.ClientSize = new Size(image.Width, image.Height);
            rectf = ClientRectangle;

            //добавляем изображение в список изображений
            image_variations.Add(image);
        }

        private void Childs_KeyDown(object sender, KeyEventArgs e)
        {
            //проверяем комбинацию
            if (e.Control && e.KeyCode == Keys.Z)
            {
                //жедаем бэкап
                Backup();
            }
        }
        private void Childs_MouseMove(object sender, MouseEventArgs e)
        {

            //проверяем зажата ли клавиша мыши и можно ли перетаскивать вставленную картинку
            if (can_drag && clicked)
            {
                //перетаскиваем картинку в нужное место
                bitmap_for_paste = new Bitmap(image_for_paste);
                grfx = CreateGraphics();
                grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
                grfx.DrawImage(image_for_paste, e.X - difference_x, e.Y - difference_y, bitmap_for_paste.Width, bitmap_for_paste.Height);
                grfx.Dispose();
            }
            if (clicked && !can_drag)
            {
                //присваиваем переменным значения координат курсора
                x_current = e.X;
                y_current = e.Y;
                //рисуем выделенную область
                PaintRectangle(Color.Green);
            }
        }
        private void Childs_MouseUp(object sender, MouseEventArgs e)
        {
            //проыверяем можно ли перетаскивать вставленную картинку
            if (can_drag)
            {
                //запрещаем перетаскивать вставленную картинку
                can_drag = false;
                //рисуем изображение со вставленной картинкой на изначальном изображении
                bitmap_for_paste = new Bitmap(image_for_paste);
                bitmap = new Bitmap(rectf.Width, rectf.Height);
                grfx = Graphics.FromImage(bitmap);
                grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
                grfx.DrawImage(image_for_paste, e.X - difference_x, e.Y - difference_y, bitmap_for_paste.Width, bitmap_for_paste.Height);
                image = Image.FromHbitmap(bitmap.GetHbitmap());
                //добавляем полученную картинку в список вариаций изображений
                image_variations.Add(image);
                grfx.Dispose();
            }
            clicked = false;
            PaintRectangle(Color.Green);
        }


        private void Childs_MouseDown(object sender, MouseEventArgs e)
        {
            if (!can_drag)
            {
                clicked = true;
                //задаем изначальные координаты
                x_initial = e.X;
                y_initial = e.Y;
                //рисуем выделенную область
                PaintRectangle(Color.Green);
            }
            if (can_drag)
            {

                clicked = true;
                //измеряем разницу в координатах 
                if ((rectf.Width / 2 < e.X) && (e.X < rectf.Width / 2 + image_for_paste.Width) && (rectf.Height / 2 < e.Y) && (e.Y < rectf.Height / 2 + image_for_paste.Height))
                {
                    difference_x = e.X - rectf.Width / 2;
                    difference_y = e.Y - rectf.Height / 2;
                }
            }
        }

        private void Childs_Paint(object sender, PaintEventArgs e)
        {
            //отображение картинки         
            grfx = e.Graphics;
            Rectangle rectf = ClientRectangle;           
            grfx.DrawImage(image_variations[image_variations.Count-1], 0, 0, rectf.Width, rectf.Height);
            grfx.Dispose();
        }

        private void Childs_FormClosed(object sender, FormClosedEventArgs e)
        {
            //вызываем событие закрытия формы
            this.tag = -1;
            if (OnClosingChild != null) OnClosingChild(EventArgs.Empty, e);
        }
        private void PaintRectangle(Color color)
        {
            //проверяем можно ли выделять
            if (distinguishable)
            {
                //проверяем цвет
                //если цвет зеленый, рисуем выделенную область
                //если цвет белый, заполянем прямоугольник белым цветом (вырезаем)
                if (color == Color.Green)
                {

                    pen = new Pen(color);
                    g = this.CreateGraphics();
                    grfx = this.CreateGraphics();
                    Rectangle rectf = ClientRectangle;
                    //рисуем изначальное изображение
                    grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
                    //сравниваем координаты для правильного отобюражения
                    if (x_current > x_initial || y_current > y_initial) g.DrawRectangle(pen, x_initial, y_initial, x_current - x_initial, y_current - y_initial);
                    else g.DrawRectangle(pen, x_current, y_current, x_initial - x_current, y_initial - y_current);
                    //освобождаем ресурсы
                    grfx.Dispose();
                    g.Dispose();
                    pen.Dispose();
                }
                if (color == Color.White)
                {
                    bitmap = new Bitmap(rectf.Width, rectf.Height);
                    g = this.CreateGraphics();
                    grfx = Graphics.FromImage(bitmap);
                    //рисуем изначальное изображение
                    grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
                    SolidBrush br = new SolidBrush(SystemColors.Control);
                    //сравниваем координаты для правильного отобюражения
                    if (x_current > x_initial || y_current > y_initial) grfx.FillRectangle(br, x_initial, y_initial, x_current - x_initial, y_current - y_initial);
                    else grfx.FillRectangle(br, x_current, y_current, x_initial - x_current, y_initial - y_current);
                    grfx.DrawImage(bitmap, 0, 0, rectf.Width, rectf.Height);
                    //получаем изображение из битовой карты
                    image_variations.Add(Image.FromHbitmap(bitmap.GetHbitmap()));
                    //полученное изображение добавляем его в спсок вариаций изображений
                    image = image_variations[image_variations.Count - 1];
                    bitmap.Dispose();
                    grfx.Dispose();
                    grfx = this.CreateGraphics();
                    grfx.DrawImage(image_variations[image_variations.Count - 1], 0, 0, rectf.Width, rectf.Height);
                    //освобождаем ресурсы
                    grfx.Dispose();
                    g.Dispose();
                    pen.Dispose();

                }
            }
        }
    }
}
