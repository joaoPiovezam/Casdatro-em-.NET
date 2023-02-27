using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teste.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage ="Campo Obrigatório")]
        public string Name { get; set; }

        public string? Rg { get; set; }

        [DisplayName("CPF / CNPJ")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string CPF { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Address { get; set; }

        [DisplayName("Endereço Adicional")]
        public string? AddAddress { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Email { get; set; }

        [DisplayName("Telefone")]
        public string? Phone { get; set; }
    }
}
