using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Lab_4.Presenters
{
    //данный класс является посредником и средством коммуникации между
    //Childs-представлением, моделью - Model_Childs и Application_Controller
    public class Presenter_Childs
    {
        //переменные модели и представления, с которыми работает презентер
        Interfaces.IView_Childs childs_view;
        Models.Model_Childs childs_model;

        //события презентера
        public event EventHandler OnClose;


        //конструктор презентера
        public Presenter_Childs(Interfaces.IView_Childs c_view, Models.Model_Childs c_model)
        {
            //инициализруем модель и отображение 
            childs_view = c_view;
            childs_model = c_model;
            //подписываемся на события Childs
            childs_view.OnClosingChild += new EventHandler(OnClosingChild);
            //подписываемся на события Model_Childs
            childs_model.SaveEvent += new EventHandler(OnSave);
            childs_model.SaveAsEvent += new EventHandler(OnSaveAs);

        }
        public void SetPictureName(string p_name)
        {
            //получаем путь к файлу изображения
            childs_view.pictureName = p_name;
        }
        public void SetTag(int tag)
        {
            //передаем в дочернюю форму тэг для работы с конркетной формой по тэгу
            childs_view.tag = tag;
        }
        public void OnClosingChild(object sender, EventArgs e)
        {
            //вызываем событие при закрытии дочерней формы
            if (OnClose != null) OnClose(EventArgs.Empty, e);
        }
        public int GetTag()
        {
            //возвращаем тэг дочерней формы
            return childs_view.tag;
        }

        public void SetViewActive()
        {
            //определяем эту форму активной
            childs_view.SetActive();
        }

        public void GetInfo()
        {
            //вызываем функцию вывода информации о форме
            childs_view.GetMetaData(childs_model.GetInformation(childs_view.image, childs_view.pictureName));
        }
        public bool IfActive()
        {
            //возвращаем являетлся ли форма активной
            return childs_view.IfActivated();
        }
        public void Save()
        {
            //вызываем функцию сохранения из модели
            childs_model.Save(childs_view.image, childs_view.pictureName);
            //перезагружаем дочернюю форму
           // childs_view.UploadAfterSaving();
        }
        public void OnSave(object sender, EventArgs e)
        {
            //вызываем функциию сохранения из представления
            childs_view.Save();
        }
        public void SaveAs(string image_format)
        {
            //вызываем функцию "сохранить как" из модели
            childs_model.SaveAs(childs_view.image, childs_view.pictureName, image_format);
        }
        public void OnSaveAs(object sender, EventArgs e)
        {
            //вызываем функцию "сохранить как" из модели
            childs_view.SaveAs();
        }
        public bool CheckChange()
        {
            //возвращаем менялась ли функция
            return childs_view.CheckChange();
        }
        public void CloseFromMain(bool dialog_result)
        {
            //закрываем форму, сохраняя или не сохраняя данные
            //в зависимости от dialog_result
            if (dialog_result)
            {
                Save();
                childs_view.CloseFromMain();
            }
            else childs_view.CloseFromMain();
        }
        public void SetDistinguish(bool can_distinguish)
        {
            //устанавливаем возможность выделять область в представлении
            childs_view.distinguishable = can_distinguish;
            if (!childs_view.distinguishable) childs_view.UpdatePicture();
        }
        public void Cut()
        {
            //вырезаем выделенную область
            childs_view.Cut();
        }
        public Image Copy()
        {
            //копируем выделенную область
            return childs_view.Copy();
        }
        public void Paste(Image image)
        { 
            //вставляем выделенную область
            childs_view.Paste(image);
        }
        public string GetPictureName()
        {
            //возвразаем путь к файлу
            return childs_view.pictureName;
        }
        public int GetX_Current()
        {
            //возвращаем х-координату
            return childs_view.GetX_Current();
        }
    }
}
