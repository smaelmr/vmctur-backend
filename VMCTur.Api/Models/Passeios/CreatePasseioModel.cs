namespace VMCTur.Api.Models.Passeios
{
    public class CreatePasseioModel
    {
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Roteiro { get; set; }
        public string HorarioAbertura { get; set; }
        public string HorarioFechamento { get; set; }
        public bool Inativo { get; set; }
        public string Obs { get; set; }
    }
}