using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Lab_4.Presenters
{
    //данный класс является посредником и средством коммуникации между
    //Main-представлением, моделью - Model_Main и Application_Controller
    public class Presenter_Main
    {
        //переменные модели и представления, с которыми работает презентер
        Interfaces.IView_Main main_view;
        Models.Model_Main main_model;

        //события презентера
        public event EventHandler OnOpen;
        public event EventHandler ActivateChild;
        public event EventHandler InformationEvent;
        public event EventHandler OnSave;
        public event EventHandler OnSaveAs;
        public event EventHandler OnCloseFromMain;
        public event EventHandler OnCanDistinguish;
        public event EventHandler OnCut;
        public event EventHandler OnCopy;
        public event EventHandler OnPaste;


        //конструктор презентера
        public Presenter_Main(Interfaces.IView_Main m_view, Models.Model_Main m_model)
        {
            //инициализруем модель и представление 
            main_view = m_view;
            main_model = m_model;
            //подписываемся на события формы Main
            main_view.OpenEvent += new EventHandler(OnOpenEvent);           
            main_view.ActivateChildEvent += new EventHandler(OnActivateChild);
            main_view.InformationAboutActiveChildEvent += new EventHandler(OnInformation);
            main_view.SaveEvent += new EventHandler(OnSaveEvent);
            main_view.SaveAsEvent += new EventHandler(OnSaveAsEvent);
            main_view.CloseChildFromMaintEvent += new EventHandler(CloseFromMain);
            main_view.CanDistinguishEvent += new EventHandler(OnCanDistinguishEvent);
            main_view.CutEvent += new EventHandler(OnCutEvent);
            main_view.CopyEvent += new EventHandler(OnCopyEvent);
            main_view.PasteEvent += new EventHandler(OnPasteEvent);
        }

        #region Methods for Event communication between Application_Controller and Main form
        private void OnOpenEvent(object sender, EventArgs e)
        {
            if (OnOpen != null) OnOpen(this, e);
        }

        public void OnActivateChild(object sender, EventArgs e)
        {
            if (ActivateChild != null) ActivateChild(EventArgs.Empty, e);
        }

        public void OnInformation(object sender, EventArgs e)
        {
            if (InformationEvent != null) InformationEvent(EventArgs.Empty, e);
        }

        public void OnSaveEvent(object sender, EventArgs e)
        {
            if (OnSave != null) OnSave(EventArgs.Empty, e);
        }

        public void OnSaveAsEvent(object sender, EventArgs e)
        {
            if (OnSaveAs != null) OnSaveAs(EventArgs.Empty, e);
        }

        public void CloseFromMain(object sender, EventArgs e)
        {
            if (OnCloseFromMain != null) OnCloseFromMain(EventArgs.Empty, e);
        }

        public void OnCanDistinguishEvent(object sender, EventArgs e)
        {
            if (OnCanDistinguish != null) OnCanDistinguish(EventArgs.Empty, e);
        }

        public void OnCutEvent(object sender, EventArgs e)
        {
            if (OnCut != null) OnCut(EventArgs.Empty, e);
        }

        public void OnCopyEvent(object sender, EventArgs e)
        {
            if (OnCopy != null) OnCopy(EventArgs.Empty, e);
        }

        public void OnPasteEvent(object sender, EventArgs e)
        {
            if (main_model.image_for_copy != null)
                if (OnPaste != null) OnPaste(EventArgs.Empty, e);
        }
        #endregion

        #region Methods for communication using varibles between Application_Controller and Main form
        public string GetPictureName()
        {
            //передаем путь к файлу дочерней форме
            return main_view.pictureName;
        }

        public int GetChildCounter()
        {
            //передаем номер дочерней формы, чтобы использовать его в качестве тэга
            return main_view.childCounter;
        }
        public void ClosingChild(int number_of_child)
        {
            //удаляем лишнюю информацию о закрытой дочерней форме
            main_view.CloseChild(number_of_child);
        }
        public int GetActiveChild()
        {
            //возвращаем тэг активного дочернего окна
            return main_view.tag_activeChild;
        }
        public void SetActiveChild(int child_active)
        {
            //передаем в главвную форму номер активной дочерней формы
            //для последующей работе с ней
            main_view.tag_activeChild = child_active;
        }

        public string GetImageFormat()
        {
            //возвращаем формат файла для сохранения 
            return main_view.imageFormat;
        }
        public void CopyImage(Image imageCopy)
        {
            //передаем в модель изображение для копирования 
            main_model.image_for_copy = imageCopy;
        }
        public Image GetCopyImage()
        {
            //возвращаем из модели изображение для копирования 
            return main_model.image_for_copy;
        }
        public void CantCreateChild(string pictureName)
        {
            //вызываем метод о неудачной попытке создать новую дочернюю форму с уже открытым файлом 
            main_view.CantCreateChild(pictureName);
        }
        public bool GetCanDistinguish()
        {
            //возвращаем переменную о возможности выделения области в дочерней форме
            return main_view.can_distinguish;
        }
        #endregion
    }
}
