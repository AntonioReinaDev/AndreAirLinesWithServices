namespace Models
{
    public class Usuario : Pessoa
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Setor { get; set; }
        public Funcao Funcao { get; set; }
    }
}
