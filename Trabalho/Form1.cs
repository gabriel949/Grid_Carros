using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MaterialSkin.Controls;
using MaterialSkin;

//Gabriel Victor Oliveira de Paula Costa
//Lucas Duarte Rocha
//Felipe Bessa

namespace Trabalho
{
    public partial class Inicio : MaterialForm
    {
        ArrayList Carros = new ArrayList();//Cria um Array List chamado Carros
        ArrayList Vendidos = new ArrayList();//Cria um Array List chamado Vendidos
        
        public Inicio()//Incicializa o Form1
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        
        }
        public void CarregaGrid()//Função para carregar o Grid dos carros cadastrados,usa o array list Carros para alimentar o Grid
        {
            dgvCarro.DataSource = null;
            dgvCarro.DataSource = Carros;

            dgvCarro.ClearSelection();

        }
        public void CarregaGridVend()//Função para carregar o Grid dos carros vendidos,usa o array list Vendidos para alimentar o Grid
        {
            dgvVendidos.DataSource = null;
            dgvVendidos.DataSource = Vendidos;

            dgvVendidos.ClearSelection();

        }
        private void Form1_Load(object sender, EventArgs e)//Evento que carrega o Grid de carros cadastrados e vendidos só quando houver 1 objeto ou mais.
        {
            if (Carros.Count != 0)
            {
                CarregaGrid();


            }
            if (Vendidos.Count != 0)
            {

                CarregaGridVend();

            }
        }
        private void btnCadstrar_Click(object sender, EventArgs e)//Evento para quando clicar no botão "Cadastrar",
                                                                  //cria as colinas no grid e adciona oque o usuário digitar no vetor Carros e carrega o novo grid
        {
            Carro x = new Carro();

            x.Placa = "Digite a Placa ...";
            x.Modelo = "";
            x.Marca = "";
            x.Ano = "";

            Carros.Add(x);
            CarregaGrid();
            dgvCarro.CurrentCell = dgvCarro.Rows[dgvCarro.RowCount - 1].Cells[0];

        }
        private void dgvCarro_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Carro x = new Carro();

            x.Placa = dgvCarro.CurrentRow.Cells[0].Value.ToString();
            x.Modelo = dgvCarro.CurrentRow.Cells[1].Value.ToString();
            x.Marca = dgvCarro.CurrentRow.Cells[2].Value.ToString();
            x.Ano = dgvCarro.CurrentRow.Cells[3].Value.ToString();

            Carros[dgvCarro.CurrentRow.Index] = x;

        }

        private void btnProcurar_Click(object sender, EventArgs e)//Quando clica no botão "Procurar" procura no Data Grid de carros cadastrados e
                                                                  //no de vendidos para ver se há algum carro com a placa que o usuário digitou
        {
            int cont = 0;

            string Nome = txtNome.Text;

            txtNome.Text = "";

            dgvCarro.ClearSelection();
            dgvVendidos.ClearSelection();


            for (int i = 0; i < dgvCarro.RowCount; i++)//Percorre o Data Grid de carros Cadastrados
            {
                if (dgvCarro.Rows[i].Cells[0].Value.ToString() == Nome)
                {
                    dgvCarro.CurrentCell = dgvCarro.Rows[i].Cells[0];

                    cont++;
                }

            }

            for (int i = 0; i < dgvVendidos.RowCount; i++)//Percorre o Data Grid de carros Vendidos
            {
                if (dgvVendidos.Rows[i].Cells[0].Value.ToString() == Nome)
                {
                    dgvVendidos.CurrentCell = dgvVendidos.Rows[i].Cells[0];

                    cont++;
                }

            }


            if (cont == 0)//Se não achar placa igual a digitada pelo usuário mostra este MessageBox
            {
                DialogResult Erro = new DialogResult();

                string Texto = "Não existe carro com esta placa !!";
                string Título = "Carro Inexistente!";
                MessageBoxButtons BotãoMensagem = MessageBoxButtons.OK;
                MessageBoxIcon ÍconeMensagem = MessageBoxIcon.Exclamation;

                Erro = MessageBox.Show(Texto, Título, BotãoMensagem, ÍconeMensagem);
            }
        }
        private void button4_Click(object sender, EventArgs e)//Ao clicar no botão sair,mostra uma Messagebox se o usuário deseja reamente sair
                                                              // Se SIM,sai do programa,se NÃO,continua no programa
        {
            DialogResult Resp = new DialogResult();

            string TextoMensagem = "Deseja SAIR do Programa?";
            string TítuloMensagem = "Confirmação de Saída...";
            MessageBoxButtons BotãoMensagem = MessageBoxButtons.YesNo;
            MessageBoxIcon ÍconeMensagem = MessageBoxIcon.Question;

            Resp = MessageBox.Show(TextoMensagem, TítuloMensagem, BotãoMensagem, ÍconeMensagem);        // Uma forma de uso...

            if (Resp == DialogResult.Yes)         // Se o usuário confirmou...
                Application.Exit();
        }
        private void btnExcluir_Click(object sender, EventArgs e)//Ao clicar no botão excluir apresenta messageBox perguntando se o usuário deseja vender o carro,
                                                                 // e passa o objeto a ser vendido para o o array list Vendidos e é mostrado no data Grid de Vendidos 
        {
            DialogResult Excluir = new DialogResult();

            string Texto = "Deseja realmente vender este carro ?";
            string Título = "Vender ?";
            MessageBoxButtons BotãoMensagem = MessageBoxButtons.YesNo;
            MessageBoxIcon ÍconeMensagem = MessageBoxIcon.Exclamation;

            Excluir = MessageBox.Show(Texto, Título, BotãoMensagem, ÍconeMensagem);
            if (Excluir == DialogResult.Yes)
            {

                Carro x = new Carro();
                try
                {

                    x.Placa = dgvCarro.CurrentRow.Cells[0].Value.ToString();
                    x.Modelo = dgvCarro.CurrentRow.Cells[1].Value.ToString();
                    x.Marca = dgvCarro.CurrentRow.Cells[2].Value.ToString();
                    x.Ano = dgvCarro.CurrentRow.Cells[3].Value.ToString();

                    Vendidos.Add(x);
                    CarregaGridVend();
                    Carros.RemoveAt(dgvCarro.CurrentRow.Index);
                    CarregaGrid();
                    CarregaGridVend();
                }

                catch (NullReferenceException Erro)
                {

                    string TextoExc = "Selecione algo para excluir!";
                    MessageBoxButtons BotãoExc = MessageBoxButtons.OK;
                    MessageBoxIcon ÍconeExc = MessageBoxIcon.Exclamation;

                    MessageBox.Show(Erro.Message, TextoExc, BotãoExc, ÍconeExc);
                }
            }
            
        }

        private void dgvVendidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)//Salva os dados do arrayList Carros e Vendidos em um arquivo XML
        {
            TextWriter Meuwriter = new StreamWriter(@"C:\POO\Carro.xml");
            Carro[] CarroVetor = (Carro[])Carros.ToArray(typeof(Carro));
            XmlSerializer serialização = new XmlSerializer(CarroVetor.GetType());
            serialização.Serialize(Meuwriter, CarroVetor);
            Meuwriter.Close();


            TextWriter Meuwriter2 = new StreamWriter(@"C:\POO\Vendidos.xml");
            Carro[] VendidosVetor = (Carro[])Vendidos.ToArray(typeof(Carro));
            XmlSerializer serialização2 = new XmlSerializer(VendidosVetor.GetType());
            serialização2.Serialize(Meuwriter2, VendidosVetor);
            Meuwriter2.Close();

            DialogResult salvo = new DialogResult();

            string Texto = "XML Salvo com sucesso!";
            string Título = "Salvo";
            MessageBoxButtons BotãoMensagem = MessageBoxButtons.OK;
            MessageBoxIcon ÍconeMensagem = MessageBoxIcon.Exclamation;

            salvo = MessageBox.Show(Texto, Título, BotãoMensagem, ÍconeMensagem);

        }

        private void btnLer_Click(object sender, EventArgs e)//Ao cicar no botão "Ler XML" lê os arquivos XML e escreve nos Data Grids as informações do XML
        {
            FileStream Arquivo = new FileStream(@"C:\POO\Carro.xml", FileMode.Open);
            Carro[] VetorCarro = (Carro[])Carros.ToArray(typeof(Carro));
            XmlSerializer Serialização = new XmlSerializer(VetorCarro.GetType());
            VetorCarro = (Carro[])Serialização.Deserialize(Arquivo);
            Carros.Clear();
            Carros.AddRange(VetorCarro);
            Arquivo.Close();
            CarregaGrid();

            FileStream Arquivo2 = new FileStream(@"C:\POO\Vendidos.xml", FileMode.Open);
            Carro[] VendidosCarro = (Carro[])Vendidos.ToArray(typeof(Carro));
            XmlSerializer Serialização2 = new XmlSerializer(VendidosCarro.GetType());
            VendidosCarro = (Carro[])Serialização2.Deserialize(Arquivo2); 
            Vendidos.Clear();
            Vendidos.AddRange(VendidosCarro);
            Arquivo2.Close();
            CarregaGridVend();
        }
    }
}