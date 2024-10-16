using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FabMauiForca
{
	public partial class MainPage : ContentPage, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#region UI Properties
		public string Destaque
		{
			get => destaque;
			set
			{
				destaque = value;
				OnPropertyChanged(nameof(Destaque));
			}
		}
		public List<char> Letras
		{
			get => letras;
			set
			{
				letras = value;
				OnPropertyChanged(nameof(Letras));
			}
		}

		public string Mensagem
		{
			get => mensagem;
			set
			{
				mensagem = value;
				OnPropertyChanged(nameof(Mensagem));
			}
		}
		public string StatusJogo
		{
			get => statusJogo;
			set
			{
				statusJogo = value;
				OnPropertyChanged(nameof(StatusJogo));
			}
		}
		public string ImagemAtual
		{
			get => imagemAtual;
			set
			{
				imagemAtual = value;
				OnPropertyChanged(nameof(ImagemAtual));
			}
		}
		#endregion


		#region Fields
		List<string> palavras = new List<string>()
	 {
		"cinel",
		"formadores",
		"caderno",
		"canetas",
		"estudar",
		"provas",
		"trabalhos",
		"word",
		"excel",
		"powerpoint",
		"abacaxi",
		"windows",
		"computador"
	 };

		string resposta = "";
		private string destaque;
		List<char> palpite = new List<char>();
		private List<char> letras = new List<char>();
		private string mensagem;
		int erros = 0;
		int maximoErros = 6;
		private string statusJogo;
		private string imagemAtual = "img0.jpg";

		#endregion

		public MainPage()
		{
			InitializeComponent();
			IniciaApp();
		}

		

		#region Jogo
		private void EscolherPalavra()
		{
			resposta =
				 palavras[new Random().Next(0, palavras.Count)];
			Debug.WriteLine(resposta);
		}

		private void CalcularPalavra(string resposta, List<char> palpite)
		{
			var temp =
				 resposta.Select(x => (palpite.IndexOf(x) >= 0 ? x : '_'))
				 .ToArray();

			Destaque = string.Join(' ', temp);
		}

		private void TratarPalpite(char letra)
		{
			if (palpite.IndexOf(letra) == -1)
			{
				palpite.Add(letra);
			}
			if (resposta.IndexOf(letra) >= 0)
			{
				CalcularPalavra(resposta, palpite);
				VerificaSeGanhou();
			}
			else if (resposta.IndexOf(letra) == -1)//erro
			{
				erros++;
				AtualizaStatus();
				VerificaSePerdeu();
				ImagemAtual = $"img{erros}.jpg";
			}
		}

		private void VerificaSePerdeu()
		{
			if (erros == maximoErros)
			{
				Mensagem = "Você Perdeu!!";
				DesabilitarLetras();
				
			}
		}

		private void DesabilitarLetras()
		{
			foreach (var children in LetrasContainer.Children)
			{
				var btn = children as Button;
				if (btn != null)
				{
					btn.IsEnabled = false;
				}
				btnReset.IsEnabled = true;
			}
		}
		private void HabilitarLetras()
		{
			foreach (var children in LetrasContainer.Children)
			{
				var btn = children as Button;
				if (btn != null)
				{
					btn.IsEnabled = true;
				}
			}
			btnReset.IsEnabled = true;
		}

		private void VerificaSeGanhou()
		{
			if (Destaque.Replace(" ", "") == resposta)
			{
				Mensagem = "Você Ganhou!";
				DesabilitarLetras();
				btnReset.IsEnabled = true;
			}
		}

		private void AtualizaStatus()
		{
			StatusJogo = $"Erros: {erros} de {maximoErros}";
		}

		#endregion

		private void Button_Clicked(object sender, EventArgs e)
		{
			var btn = sender as Button;
			if (btn != null)
			{
				var letra = btn.Text;
				btn.IsEnabled = false;
				TratarPalpite(letra[0]);
			}
		}
		private void IniciaApp()
		{
			ImagemAtual = "img0.jpg";
			palpite = new List<char>();
			erros = 0;
			Letras.Clear();
			Letras.AddRange("abcdefghijklmnopqrstuvwxyz");
			BindingContext = this;
			EscolherPalavra();
			CalcularPalavra(resposta, palpite);
			Mensagem = "";
			AtualizaStatus();
			HabilitarLetras();
		}

		private void Button_Clicked_1(object sender, EventArgs e)
		{			
			IniciaApp();			
		}
	}

}
