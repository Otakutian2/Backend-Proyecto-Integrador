﻿using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class CustomerPrincipal
    {
        [Required(ErrorMessage = "El campo 'Nombres' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Nombres' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombres' debe tener una longitud máxima de 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo 'Apellidos' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Apellidos' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Apellidos' debe tener una longitud máxima de 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo 'Dni' es requerido")]
        public string Dni { get; set; }
    }

    public class CustomerGet : CustomerPrincipal
    {
        public int Id { get; set; }
    }
}
