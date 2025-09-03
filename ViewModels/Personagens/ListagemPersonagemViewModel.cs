using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Services.Personagens;
using AppRpgEtec.Models;
using System.Windows.Input;
using AppRpgEtec.Views.Personagens;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ObservableCollection<Personagem> Personagens {  get; set; }
        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            Personagens = new ObservableCollection<Personagem>();
            _ = ObterPersonagens();

            NovoPersonagemCommand = new Command(async () => { await ExibirCadastroPersonagem(); });
        }

        public ICommand NovoPersonagemCommand { get; }

        public async Task ObterPersonagens()
        {
            try // junto com catch evitara que erros fechem o aplicativo
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));// Informara a view que houve carregamento
            }
            catch (Exception ex)
            {
                //Capturara o erro para exibir em tela
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
