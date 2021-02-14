using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    //класс, предназначенный для коммуникации между триадами с помощью
    //вызова делегатов событий
    public class Application_Controller
    {
        #region Models
        //инициализруем модели
        public Models.Model_Main model_main = new Models.Model_Main();
        public Models.Model_Childs model_childs = new Models.Model_Childs();
        public Models.Model_AskToSaveForm model_atsf = new Models.Model_AskToSaveForm();
        #endregion

        #region Presenters
        //объявляем презентеры
        public Presenters.Presenter_Main presenter_main;
        public Presenters.Presenter_Childs presenter_childs;
        public Presenters.Presenter_AskToSaveForm presenter_atsf;
        #endregion

        #region Views
        //инициализруем форму Main
        Main main_view = new Main();
        #endregion

        #region Extra Variables
        //инициализируем список презентеров Presenter_Childs дочерних форм для дальнейших манипуляций с ними
        List<Presenters.Presenter_Childs> list_presenters_childs = new List<Presenters.Presenter_Childs>();
        #endregion

        public Application_Controller()
        {
            //инициализируем презентер Main формы
            presenter_main = new Presenters.Presenter_Main(main_view, model_main);
            //подписываемся на события Main формы
            presenter_main.OnOpen += new EventHandler(CreationChild);
            presenter_main.ActivateChild += new EventHandler(OnActivateChild);
            presenter_main.InformationEvent += new EventHandler(OnInformation);
            presenter_main.OnSave += new EventHandler(OnSave);
            presenter_main.OnSaveAs += new EventHandler(OnSaveAs);
            presenter_main.OnCloseFromMain += new EventHandler(OnCloseFromMain);
            presenter_main.OnCanDistinguish += new EventHandler(OnCanDistinguish);
            presenter_main.OnCut += new EventHandler(OnCut);
            presenter_main.OnCopy += new EventHandler(OnCopy);
            presenter_main.OnPaste += new EventHandler(OnPaste);
        }

        #region Methods
        public void CreateChildMethod()
        {
            //создаем новую форму Childs
            Childs childs_view = new Childs();
            childs_view.MdiParent = main_view;
            //инициализруем презентер формы Childs 
            presenter_childs = new Presenters.Presenter_Childs(childs_view, model_childs);
            //добавляем презентер в список
            list_presenters_childs.Add(presenter_childs);
            //подписываемся на события презнтера
            presenter_childs.OnClose += new EventHandler(Close_Child);
            //передаем название файла, который загружаем в форме childs_view
            presenter_childs.SetPictureName(presenter_main.GetPictureName());
            //задаем тэг по количеству дочерних форм у главной формы
            presenter_childs.SetTag(presenter_main.GetChildCounter());
            //запускаем форму
            childs_view.Show();
            //устанавливаем текущую возможность выделения области
            presenter_childs.SetDistinguish(presenter_main.GetCanDistinguish());
        }
        public int FindActiveChild()
        {
            //находим и возвращаем активную дочернюю форму
            int active_number = 0;
            for (int i = 0; i < list_presenters_childs.Count; i++)
            {
                if (list_presenters_childs[i].IfActive() == true)
                {
                    active_number = i;
                }
            }
            return active_number;
        }
        #endregion

        #region Evoke Functions
        public void CreationChild(object sender, EventArgs e)
        {           
            //создаем переменную, которая служит для разрешения создания новой дочерней формы
            bool can_create = true;
            //если список пустой, сразу создаем новую дочернюю форму
            //если в нем имеется, хоть один презентер, проверяем на наличие
            //совпадений в используемых файлах
            if (list_presenters_childs.Count > 0)
            {
                for (int i = 0; i < list_presenters_childs.Count; i++)
                { 
                    //проверяем совпадение файлов
                    if (presenter_main.GetPictureName() == list_presenters_childs[i].GetPictureName())
                    {                      
                        //если совпадение есть, запрещаем создание формы
                        can_create = false;
                        //уведомляем пользователя о произошедшем 
                        presenter_main.CantCreateChild(presenter_main.GetPictureName());
                        break;
                    }
                    
                }   
                if(can_create == true)
                {
                    CreateChildMethod();
                }
            }
            else
            {
                CreateChildMethod();
            }
        }

        public void Close_Child(object sender, EventArgs e)
        {
            //проверяем тэг каждой формы
            for (int i = 0; i < list_presenters_childs.Count; i++)
            {
                //если он равен -1, то передаем i, чтобы удалить его из подменю Окно
                if (list_presenters_childs[i].GetTag() == -1)
                {
                    
                    presenter_main.ClosingChild(i);
                    //удаляем презентер без представления из списка презентеров дочерних форм
                    list_presenters_childs[i] = null;
                    list_presenters_childs.RemoveAt(i);                   
                    break;
                }
            }
        }

        public void OnActivateChild(object sender, EventArgs e)
        { 
            //проверяем совпадение форм по тэгу
            for(int i = 0; i < list_presenters_childs.Count; i++)
            {
                if(list_presenters_childs[i].GetTag()==presenter_main.GetActiveChild())
                {
                    //делаем активной форму, которую выбрал пользователь в главной форме
                    list_presenters_childs[i].SetViewActive();
                }
            }
              
        }       

        public void OnInformation(object sender, EventArgs e)
        {
            //вызываем информацию об активной дочерней форме
            if (list_presenters_childs.Count > 0)
            {
                int active = FindActiveChild();
                    list_presenters_childs[active].GetInfo();
            }
        }

        public void OnSave(object sender, EventArgs e)
        {
            //сохраняем изображение активной дочерней форме
            if (list_presenters_childs.Count > 0)
            {
                int active = FindActiveChild();
                    list_presenters_childs[FindActiveChild()].Save();
            }           
        }

        public void OnSaveAs(object sender, EventArgs e)
        {
            //сохраняем изображение активной дочерней форме в нужном формате
            if (list_presenters_childs.Count > 0)
            {
                int active = FindActiveChild();
                    list_presenters_childs[FindActiveChild()].SaveAs(presenter_main.GetImageFormat());
            }           
        }

        public void OnCloseFromMain(object sender, EventArgs e)
        {
            //закрываем активную дочернюю форму
            if (list_presenters_childs.Count > 0)
            {
                int i = FindActiveChild();
                //проверяем вносились ли изменения в исходную картинку
                if (!list_presenters_childs[i].CheckChange())
                {
                    //если изменения не вносились, просто закрываем форму
                    list_presenters_childs[i].CloseFromMain(false);
                }
                else
                {
                    //если изменения вносились, то вызываем форму с вопросом о сохранении изменений
                    AskToSaveForm atsf_view = new AskToSaveForm();
                    presenter_atsf = new Presenters.Presenter_AskToSaveForm(atsf_view, model_atsf);
                    //если пользователь нажал "да", то сохраняем файл, если "нет" - просто закрываем форму
                    list_presenters_childs[i].CloseFromMain(presenter_atsf.DialogResult());
                    presenter_atsf = null;
                }
            }
        }
        
        public void OnCanDistinguish(object sender, EventArgs e)
        {
            //устанавливаем возможность выделять область в активной дочерней форме
            for (int i = 0; i < list_presenters_childs.Count; i++)
            {
                list_presenters_childs[i].SetDistinguish(presenter_main.GetCanDistinguish());
            }
        }

        public void OnCut(object sender, EventArgs e)
        {
            //вырезаем нужную выделенную область в активной форме
            if (list_presenters_childs.Count > 0)
            {
                int active = FindActiveChild();
                //проверяем, существует ли выделенная область
                if (list_presenters_childs[active].GetX_Current() != -1)
                    list_presenters_childs[active].Cut();
            }
        }

        public void OnCopy(object sender, EventArgs e)
        {
            //копируем выделенную область активной дочерней формы в специальную переменную в Model_Main
            if (list_presenters_childs.Count > 0)
            {
                int active = FindActiveChild();
                //проверяем, существует ли выделенная область
                if (list_presenters_childs[active].GetX_Current() != -1)
                    presenter_main.CopyImage(list_presenters_childs[active].Copy());
            }
        }

        public void OnPaste(object sender, EventArgs e)
        {
            if (list_presenters_childs.Count > 0)
            {
                //вставляем скопированное изображение в активную дочернюю форму  
                list_presenters_childs[FindActiveChild()].Paste(presenter_main.GetCopyImage());
            }   
        }
        #endregion

        public void Run()
        {
            //запускаем форму Main при запуске программы
            main_view.Run();
        }
    }
}
