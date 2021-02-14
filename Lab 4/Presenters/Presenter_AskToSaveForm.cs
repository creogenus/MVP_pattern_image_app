using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4.Presenters
{
    //данный класс является посредником и средством коммуникации между
    //AskToSaveForm-представлением, моделью - Model_AskToSaveForm и Application_Controller
    public class Presenter_AskToSaveForm
    {
        //переменные модели и представления, с которыми работает презентер
        Interfaces.IView_AskToSaveForm askToSaveForm_view;
        Models.Model_AskToSaveForm askToSaveForm_models;


        //конструктор презентера
        public Presenter_AskToSaveForm(Interfaces.IView_AskToSaveForm atsf_view, Models.Model_AskToSaveForm atsf_model)
        {
            //инициализруем модель и представление
            askToSaveForm_view = atsf_view;
            askToSaveForm_models = atsf_model;
        }

        public bool DialogResult()
        {
            //возвращает информацию, хочет ли пользователь сохранить данные
            //в измененной картинке
            return askToSaveForm_view.Dialog();
        }
    }
}
