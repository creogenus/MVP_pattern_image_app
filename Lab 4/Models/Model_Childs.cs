using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace Lab_4.Models
{
    //данный класс предназначен для реализации бизнес-логики дочерней формы
    public class Model_Childs
    {
        #region Variables for Save() function
        //события модели
        public event EventHandler SaveEvent;
        public event EventHandler SaveAsEvent;
        #endregion
        public string GetInformation(Image image, string picture_name)
        {
            //формируем строку информации о файле
            string data_string = "";
            data_string += "Путь к файлу: " + picture_name + "\n";
            string[] split_array = picture_name.Split(Convert.ToChar(@"\"));
            data_string += "Имя: " + split_array[split_array.Length-1]+"\n";
            data_string += "Формат файла: " + image.RawFormat + "\n";
            data_string += "Высота: " + image.Height + "\n";
            data_string += "Ширина: " + image.Width + "\n";
            data_string += "Вертикальное разрешение: " + image.VerticalResolution + "\n";
            data_string += "Горизонтальное разрешение: " + image.HorizontalResolution + "\n";
            data_string += "Физические размеры: " + image.PhysicalDimension + "\n";
            data_string += "Использованный формат пикселей: " + image.PixelFormat + "\n";
            data_string += "Использование бита или байта прозрачности: " + Image.IsAlphaPixelFormat(image.PixelFormat) + "\n";
            data_string += "Число бит на пиксель: "+ Image.GetPixelFormatSize(image.PixelFormat) + "\n";
            return data_string;
        }
        public void Save(Image image, string picture_name)
        {          
            //разбиваем строку
            string[] split_array = picture_name.Split(Convert.ToChar(@"\"));
            //создаем временный файл для временного хранения данных
            image.Save("new_"+split_array[split_array.Length - 1]);
            //вызываем событие сохранение, которое запускает функции освобождения ресурсов
            if (SaveEvent != null) SaveEvent(EventArgs.Empty, EventArgs.Empty);
            //загружаем информацию из временного файла
            image = Image.FromFile("new_" + split_array[split_array.Length - 1]);
            //сохраняем информацию 
            image.Save(split_array[split_array.Length - 1]);
            //освобождаем ресурсы
            image.Dispose();
            //удаляем временный файл
            File.Delete("new_" + split_array[split_array.Length - 1]);
        }

        public void SaveAs(Image image, string picture_name, string image_format)
        {
            //оазбиваем строку
            string[] split_array = picture_name.Split(Convert.ToChar(@"\"));
            string[] name_check_array = split_array[split_array.Length - 1].Split('.');
            //провереям является ли выбранный формат изначальным форматом
            //если да, то просто сохраняем
            //если нет, то сохраняем в определенном формате
            if(name_check_array[0]+image_format != split_array[split_array.Length - 1])
            switch (image_format)
            {
                case ".bmp":
                    {
                        image.Save(name_check_array[0] + image_format, ImageFormat.Bmp);
                        if (SaveAsEvent != null) SaveAsEvent(EventArgs.Empty, EventArgs.Empty);
                        break;
                    }
                case ".jpg":
                    {
                        image.Save(name_check_array[0] + image_format, ImageFormat.Jpeg);
                        if (SaveAsEvent != null) SaveAsEvent(EventArgs.Empty, EventArgs.Empty);    
                        break;
                    }
                case ".png":
                    {
                        image.Save(name_check_array[0] + image_format, ImageFormat.Png);
                        if (SaveAsEvent != null) SaveAsEvent(EventArgs.Empty, EventArgs.Empty);
                        break;
                    }
                case ".gif":
                    {
                        image.Save(name_check_array[0] + image_format, ImageFormat.Gif);
                        if (SaveAsEvent != null) SaveAsEvent(EventArgs.Empty, EventArgs.Empty);
                        break;
                    }
                case ".tiff":
                    {
                        image.Save(name_check_array[0] + image_format, ImageFormat.Tiff);
                        if (SaveAsEvent != null) SaveAsEvent(EventArgs.Empty, EventArgs.Empty);
                        break;
                    }
            }
            else
            {
                Save(image, picture_name);
            }
        }
    }
}
